using ClinicSystem.BLL.Services;
using ClinicSystem.DAL;
using ClinicSystem.DAL.DTOs.AccountDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result =await _authService.Login(loginDto);
            if (result.Succeeded)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Error);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _authService.Register(dto);

            if (result.Succeeded)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Error);
        }
        [HttpPost("Forget-Password")]
        public async Task<ActionResult> FrogetPassword(ForgetPasswordDto forgetPasswordDto)
        {
            var result = await _authService.ForgetPassword(forgetPasswordDto);
            if (result.Succeeded)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Error);
        }
        [HttpPost("Reset-Password")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var result = await _authService.ResetPasswprd(resetPasswordDto);
            if (result.Succeeded)
            {
                return Ok(new MassegeResponnse
                {
                    Massege = "Password has been reset successfully."
                });
            }
                return BadRequest(result.Error);
        }

    }
}
