using ClinicSystem.DAL.Data;
using ClinicSystem.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.DAL;

public class AppointmentRepository : GenericRepository<Appointment> , IAppointmentRepository
{
    public AppointmentRepository(MyContext context) : base(context) { }
    public async Task<IEnumerable<Appointment>> GetByDoctorId(int doctorId)
    {
        return await _context.Appointments
            .Include(a => a.Patient) 
            .Include(a => a.Schedule) 
            .Where(a => a.DoctorId == doctorId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetByPatientId(int patientId)
    {
        return await _context.Appointments
            .Include(a => a.Doctor) 
                .ThenInclude(d => d!.Specialty) 
            .Include(a => a.Schedule) 
            .Where(a => a.PatientId == patientId)
            .ToListAsync();
    }
    public async Task<Appointment?> GetByIdWithDetails(int appointmentId)
    {
        return await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Schedule)
            .Include(a => a.Doctor)           
                .ThenInclude(d => d!.Specialty) 
            .FirstOrDefaultAsync(a => a.Id == appointmentId);
    }
    public async Task<int> GetTotalCountForDoctor(int doctorId)
    {
        return await _context.Appointments.CountAsync(a => a.DoctorId == doctorId);
    }

    public async Task<int> GetUpcomingCountForDoctor(int doctorId)
    {
        return await _context.Appointments
            .CountAsync(a => a.DoctorId == doctorId && a.Schedule!.Day >= DateTime.Today && a.Status == AppointmentStatus.Confirmed);
    }

    public async Task<decimal> GetTotalRevenueForDoctor(int doctorId)
    {
        return await _context.Appointments
            .Where(a => a.DoctorId == doctorId && a.Status == AppointmentStatus.Completed)
            .SumAsync(a => a.Doctor!.ConsultationFee);
    }
    public async Task<int> GetCountByStatus(AppointmentStatus status)
    {
        return await _context.Appointments.CountAsync(a => a.Status == status);
    }
    public async Task<int> Count()
    {
        return await _context.Appointments.CountAsync();
    }
    public async Task<IEnumerable<DayOfWeekDistributionDto>> GetAppointmentDistribution(int doctorId)
    {
        return await _context.Appointments
            .Where(a => a.DoctorId == doctorId
                     && a.Status == AppointmentStatus.Completed
                     && a.Schedule != null)
            .GroupBy(a => a.Schedule!.Day.DayOfWeek)
            .Select(g => new DayOfWeekDistributionDto
            {
                Day = g.Key.ToString(),
                AppointmentsCount = g.Count()
            })
            .ToListAsync();
    }

}
