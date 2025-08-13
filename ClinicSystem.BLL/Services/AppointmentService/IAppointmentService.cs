using ClinicSystem.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.BLL;

public interface IAppointmentService
{
    Task<Result<AppointmentDetailDto>> CreateAppointment(string patientUserId, CreateAppointmentDto dto);
    Task<Result<bool>> ConfirmAppointment(int appointmentId, string doctorUserId);
    Task<Result<bool>> CancelAppointment(int appointmentId, string userId, string userRole);
    Task<Result<bool>> CompleteAppointment(int appointmentId, string doctorUserId);
    Task<Result<IEnumerable<AppointmentDetailDto>>> GetMyAppointments(string userId, string userRole);
}
