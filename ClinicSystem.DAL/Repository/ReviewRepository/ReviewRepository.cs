using ClinicSystem.DAL.Data;
using ClinicSystem.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.DAL;

public class ReviewRepository : GenericRepository<Review> , IReviewRepository
{
    public ReviewRepository(MyContext context) :base(context) { }
    public async Task<IEnumerable<Review>> GetByDoctorId(int doctorId)
    {
        return await _context.Reviews
            .Include(r => r.Patient) 
            .Where(r => r.DoctorId == doctorId)
            .OrderByDescending(r => r.CreatedAt) 
            .ToListAsync();
    }
    public async Task<double> GetAverageRatingForDoctor(int doctorId)
    {
        return await _context.Reviews
            .Where(r => r.DoctorId == doctorId)
            .AverageAsync(r => (double?)r.Rating) ?? 0;
    }
    public async Task<Review?> GetByIdWithDetails(int reviewId)
    {
        return await _context.Reviews
            .Include(r => r.Patient)
            .FirstOrDefaultAsync(r => r.Id == reviewId);
    }
}
