using ClinicSystem.DAL;
using ClinicSystem.DAL.DTOs;
using ClinicSystem.DAL.DTOs.AccountDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.BLL.Services;

public interface  IAuthService
{
    Task<Result<LoginResponseDto>> Register(RegisterDto dto);
    Task<Result<LoginResponseDto>> Login(LoginDto loginDto);
    Task<Result<string>> ForgetPassword(ForgetPasswordDto forgetPasswordDto);
    Task<Result<bool>> ResetPasswprd(ResetPasswordDto resetPasswordDto);
}
