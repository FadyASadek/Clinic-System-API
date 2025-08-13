using ClinicSystem.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.BLL;

public interface IReviewService
{
    Task<Result<ReviewDto>> CreateReview(string patientUserId, CreateReviewDto dto);
    Task<Result<IEnumerable<ReviewDto>>> GetReviewsForDoctor(int doctorId);
    Task<Result<ReviewDto>> UpdateMyReview(int reviewId, string patientUserId, UpdateReviewDto dto);
    Task<Result<bool>> DeleteMyReview(int reviewId, string patientUserId);

}
