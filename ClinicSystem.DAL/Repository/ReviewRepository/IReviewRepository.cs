using ClinicSystem.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.DAL;

public interface IReviewRepository : IGenericRepository<Review>
{
    Task<IEnumerable<Review>> GetByDoctorId(int doctorId);
    Task<double> GetAverageRatingForDoctor(int doctorId);
    Task<Review?> GetByIdWithDetails(int reviewId);

}
