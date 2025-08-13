using ClinicSystem.DAL.Data;
using ClinicSystem.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ClinicSystem.DAL;

public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
{

    public DoctorRepository(MyContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Doctor>> GetAllWithSpecialty()
    {
        return await _context.Doctors.Include(d => d.Specialty).ToListAsync();
    }

    public async Task<Doctor?> GetByIdWithSpecialty(int doctorId)
    {
        return await _context.Doctors.Include(d => d.Specialty).FirstOrDefaultAsync(d => d.Id == doctorId);
    }

    public async Task<Doctor?> GetByUserId(string userId)
    {
        return await _context.Doctors.Include(d => d.Specialty)
            .FirstOrDefaultAsync(d => d.ApplicationUserId == userId);
    }
    public async Task<int> Count()
    {
        return await _context.Doctors.CountAsync();
    }

    public async Task<IEnumerable<Doctor>> Filter(DoctorFilterDto filter)
    {
        var query = _context.Doctors.Include(s => s.Specialty).AsQueryable();
        if (!string.IsNullOrEmpty(filter.Search))
        {
            query = query.Where(d => d.FullName.ToLower().Contains(filter.Search.ToLower()));
        }
        if (filter.SpecialtyId.HasValue)
        {
            query = query.Where(d => d.SpecialtyId == filter.SpecialtyId.Value);
        }
        return await query
        .Skip((filter.PageNumber - 1) * filter.PageSize)
        .Take(filter.PageSize)
        .ToListAsync();
    }
    public async Task<IEnumerable<TopDoctorDto>> GetTopDoctors(int count)
    {
        var topDoctors = await _context.Doctors
            .Include(d => d.Specialty) 
            .Select(d => new
            {
                Doctor = d,
                CompletedCount = d.Appointments.Count(a => a.Status == AppointmentStatus.Completed)
            })
            .OrderByDescending(x => x.CompletedCount)
            .Take(count)
            .Select(x => new TopDoctorDto
            {
                DoctorId = x.Doctor.Id,
                FullName = x.Doctor.FullName,
                SpecialtyName = x.Doctor.Specialty!.Name,
                CompletedAppointments = x.CompletedCount
            })
            .ToListAsync();

        return topDoctors;
    }
}
