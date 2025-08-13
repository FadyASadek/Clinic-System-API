using ClinicSystem.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.DAL;

public interface IAppointmentRepository : IGenericRepository<Appointment>
{
    Task<IEnumerable<Appointment>> GetByDoctorId(int doctorId);
    Task<IEnumerable<Appointment>> GetByPatientId(int patientId);
    Task<Appointment?> GetByIdWithDetails(int appointmentId);
    Task<int> GetTotalCountForDoctor(int doctorId);
    Task<int> GetUpcomingCountForDoctor(int doctorId);
    Task<decimal> GetTotalRevenueForDoctor(int doctorId);
    Task<int> GetCountByStatus(AppointmentStatus status);
    Task<IEnumerable<DayOfWeekDistributionDto>> GetAppointmentDistribution(int doctorId);
    Task<int> Count();

}
