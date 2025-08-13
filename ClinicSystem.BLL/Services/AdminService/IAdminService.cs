using ClinicSystem.DAL;
using ClinicSystem.DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.BLL.Services;

public interface IAdminService
{
    Task<Result<IEnumerable<UsersDto>>> GetUsers();
    Task<Result<bool>> PromoteToDoctor(PromoteToDoctorDto dto);
    Task<Result<bool>> BlockUser(string userId);
    Task<Result<bool>> UnblockUser(string userId);
    Task<Result<bool>> DeleteReview(int reviewId);
    Task<Result<ReviewDto>> UpdateReview(int reviewId, UpdateReviewDto dto);
}
