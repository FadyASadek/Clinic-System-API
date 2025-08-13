using ClinicSystem.DAL;
using ClinicSystem.DAL.Data;
using ClinicSystem.DAL.DTOs;
using ClinicSystem.DAL.Model;
using ClinicSystem.DAL.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.BLL.Services;

public class AdminService : IAdminService
{
    private readonly MyContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminService
        (UserManager<ApplicationUser> userManager, MyContext context,IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _context = context;
        this._unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<UsersDto>>> GetUsers()
    {
        var usersWithRoles = await _context.Users
            .Select(user => new UsersDto
            {
                Id = user.Id,
                Email = user.Email!,
                Name = user.UserName!,
                Role = (from userRole in _context.UserRoles
                        join role in _context.Roles on userRole.RoleId equals role.Id
                        where userRole.UserId == user.Id
                        select role.Name).ToList()
            })
            .ToListAsync();

        if (!usersWithRoles.Any())
        {
            return Result<IEnumerable<UsersDto>>.Failure("No users found.");
        }

        return Result<IEnumerable<UsersDto>>.Success(usersWithRoles);
    }
    public async Task<Result<bool>> PromoteToDoctor(PromoteToDoctorDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId);
        if (user == null)
            return Result<bool>.Failure("User not found.");

        await _userManager.RemoveFromRoleAsync(user, "Patient");
        await _userManager.AddToRoleAsync(user, "Doctor");

        var doctorProfile = new Doctor
        {
            ApplicationUserId = user.Id,
            FullName = user.UserName!,
            SpecialtyId = dto.SpecialtyId,
            Bio = dto.Bio,
            ConsultationFee = dto.ConsultationFee,
            ExperienceYears = dto.ExperienceYears,
            IsVerified = true
        };

        await _unitOfWork.Doctor.Add(doctorProfile);
        await _unitOfWork.Complete();

        return Result<bool>.Success(true);
    }
    public async Task<Result<bool>> BlockUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return Result<bool>.Failure("User not found.");

        var result = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);

        if (!result.Succeeded)
            return Result<bool>.Failure("Failed to block user.");

        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> UnblockUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return Result<bool>.Failure("User not found.");

        var result = await _userManager.SetLockoutEndDateAsync(user, null);

        if (!result.Succeeded)
            return Result<bool>.Failure("Failed to unblock user.");

        return Result<bool>.Success(true);
    }
    public async Task<Result<bool>> DeleteReview(int reviewId)
    {
        var review = await _unitOfWork.Review.GetById(reviewId);
        if (review == null)
            return Result<bool>.Failure("Review not found.");

        await _unitOfWork.Review.Delete(review);
        await _unitOfWork.Complete();

        return Result<bool>.Success(true);
    }

    public async Task<Result<ReviewDto>> UpdateReview(int reviewId, UpdateReviewDto dto)
    {
        var review = await _unitOfWork.Review.GetByIdWithDetails(reviewId); 
        if (review == null)
            return Result<ReviewDto>.Failure("Review not found.");

        review.Rating = dto.Rating;
        review.Comment = dto.Comment;
        review.UpdatedAt = DateTime.Now;

        await _unitOfWork.Complete();

        var reviewDto = new ReviewDto
        {
            Id = review.Id,
            Rating = review.Rating,
            Comment = review.Comment,
            CreatedAt = review.CreatedAt,
            PatientName = review.Patient!.FullName
        };

        return Result<ReviewDto>.Success(reviewDto);
    }
}
