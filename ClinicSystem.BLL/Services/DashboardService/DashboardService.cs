
using ClinicSystem.DAL;
using ClinicSystem.DAL.Model;
using ClinicSystem.DAL.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.BLL;

public class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;

    public DashboardService(IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager )
    {
        this._unitOfWork = unitOfWork;
        this._userManager = userManager;
    }
    public async Task<Result<DoctorDashboardDto>> GetDoctorDashboard(string doctorUserId)
    {
        var doctor = await _unitOfWork.Doctor.GetByUserId(doctorUserId);
        if (doctor == null)
            return Result<DoctorDashboardDto>.Failure("Doctor profile not found.");

        var totalAppointments = await _unitOfWork.Appointment.GetTotalCountForDoctor(doctor.Id);
        var upcomingAppointments = await _unitOfWork.Appointment.GetUpcomingCountForDoctor(doctor.Id);
        var averageRating = await _unitOfWork.Review.GetAverageRatingForDoctor(doctor.Id);
        var totalRevenue = await _unitOfWork.Appointment.GetTotalRevenueForDoctor(doctor.Id);

        var dashboardDto = new DoctorDashboardDto
        {
            TotalAppointments = totalAppointments,
            UpcomingAppointments = upcomingAppointments,
            AverageRating = Math.Round(averageRating, 2),
            TotalRevenue = totalRevenue
        };

        return Result<DoctorDashboardDto>.Success(dashboardDto);
    }
    public async Task<Result<AdminDashboardDto>> GetAdminDashboard()
    {
        var totalDoctors = await _unitOfWork.Doctor.Count();
        var totalPatients = await _unitOfWork.Patient.Count();
        var totalAppointments = await _unitOfWork.Appointment.Count(); 
        var pendingAppointments = await _unitOfWork.Appointment.GetCountByStatus(AppointmentStatus.Pending);

        var dashboardDto = new AdminDashboardDto
        {
            TotalDoctors = totalDoctors,
            TotalPatients = totalPatients,
            TotalAppointments = totalAppointments,
            PendingAppointments = pendingAppointments
        };

        return Result<AdminDashboardDto>.Success(dashboardDto);
    }
    public async Task<Result<IEnumerable<TopDoctorDto>>> GetTopDoctors(int num)
    {
        var topDoctors = await _unitOfWork.Doctor.GetTopDoctors(num); 
        return Result<IEnumerable<TopDoctorDto>>.Success(topDoctors);
    }
    public async Task<Result<MonthlyRegistrationsDto>> GetMonthlyRegistrations()
    {
        var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var now = DateTime.Now;

        var usersThisMonth = await _userManager.Users
            .Where(u => u.CreatedAt >= startOfMonth && u.CreatedAt <= now) 
            .ToListAsync();

        int newDoctorsCount = 0;
        int newPatientsCount = 0;

        foreach (var user in usersThisMonth)
        {
            if (await _userManager.IsInRoleAsync(user, "Doctor"))
            {
                newDoctorsCount++;
            }
            else if (await _userManager.IsInRoleAsync(user, "Patient"))
            {
                newPatientsCount++;
            }
        }

        var resultDto = new MonthlyRegistrationsDto
        {
            NewDoctorsCount = newDoctorsCount,
            NewPatientsCount = newPatientsCount
        };

        return Result<MonthlyRegistrationsDto>.Success(resultDto);
    }
    public async Task<Result<IEnumerable<DayOfWeekDistributionDto>>> GetDoctorAppointmentDistribution(string doctorUserId)
    {
        var doctor = await _unitOfWork.Doctor.GetByUserId(doctorUserId);
        if (doctor == null)
            return Result<IEnumerable<DayOfWeekDistributionDto>>.Failure("Doctor profile not found.");

        var distribution = await _unitOfWork.Appointment.GetAppointmentDistribution(doctor.Id);

        return Result<IEnumerable<DayOfWeekDistributionDto>>.Success(distribution);
    }
}
