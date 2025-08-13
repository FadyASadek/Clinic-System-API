
using ClinicSystem.DAL;
using ClinicSystem.DAL.DTOs;
using ClinicSystem.DAL.DTOs.AccountDTO;
using ClinicSystem.DAL.Model;
using ClinicSystem.DAL.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClinicSystem.BLL.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService
        (UserManager<ApplicationUser> userManager,
        IConfiguration configuration,
        IUnitOfWork unitOfWork
        )
    {
        _userManager = userManager;
        _configuration = configuration;
        this._unitOfWork = unitOfWork;
    }

    public async Task<Result<LoginResponseDto>> Register(RegisterDto dto)
    {
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser is not null)
        {
            return Result<LoginResponseDto>.Failure("Email is already registered.");
        }

        var user = new ApplicationUser
        {
            UserName = dto.UserName,
            Email = dto.Email,
            Address = dto.Address
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result<LoginResponseDto>.Failure(errors);
        }

        await _userManager.AddToRoleAsync(user, "Patient");

        var newPatientProfile = new Patient
        {
            ApplicationUserId = user.Id, 
            FullName = dto.UserName 
        };

        await _unitOfWork.Patient.Add(newPatientProfile);
        await _unitOfWork.Complete(); 

        var jwtToken = await GenerateJwtToken(user);
        var response = new LoginResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
            ExpiresOn = jwtToken.ValidTo
        };

        return Result<LoginResponseDto>.Success(response);
    }

    public async Task<Result<LoginResponseDto>> Login(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is null || !await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            return Result<LoginResponseDto>.Failure("Invalid email or password.");
        }

        if (await _userManager.IsLockedOutAsync(user))
        {
            return Result<LoginResponseDto>.Failure("This account has been blocked.");
        }


        var jwtToken = await GenerateJwtToken(user);
        var response = new LoginResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
            ExpiresOn = jwtToken.ValidTo
        };

        return Result<LoginResponseDto>.Success(response);
    }

    private async Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Name, user.UserName!),
        };

        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:SecretKey"]!));

        var token = new JwtSecurityToken(
            expires: DateTime.Now.AddDays(15),
            claims: claims,
            signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }

    public async Task<Result<string>> ForgetPassword(ForgetPasswordDto forgetPasswordDto)
    {
        var user = await _userManager.FindByEmailAsync(forgetPasswordDto.Email);
        if (user is null)
        {
            return Result<string>.Failure("If an account with this email exists, a password reset link has been sent.");
        }
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        return Result<string>.Success(token);
    }

    public async Task<Result<bool>> ResetPasswprd(ResetPasswordDto resetPasswordDto)
    {
        var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
        if (user is null)
        {
            return Result<bool>.Failure("Invalid request.");
        }
        var result = await _userManager
            .ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.NewPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result<bool>.Failure(errors);
        }

        return Result<bool>.Success(true);
    }
}