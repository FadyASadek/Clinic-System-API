using ClinicSystem.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.BLL;

public interface IDashboardService
{
    Task<Result<DoctorDashboardDto>> GetDoctorDashboard(string doctorUserId);
    Task<Result<AdminDashboardDto>> GetAdminDashboard();
    Task<Result<IEnumerable<TopDoctorDto>>> GetTopDoctors(int num);
    Task<Result<MonthlyRegistrationsDto>> GetMonthlyRegistrations();
    Task<Result<IEnumerable<DayOfWeekDistributionDto>>> GetDoctorAppointmentDistribution(string doctorUserId);

}
