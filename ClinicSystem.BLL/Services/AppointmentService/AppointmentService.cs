using ClinicSystem.DAL;
using ClinicSystem.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.BLL;

public class AppointmentService : IAppointmentService
{
    private readonly IUnitOfWork _unitOfWork;

    public AppointmentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<AppointmentDetailDto>>> GetMyAppointments(string userId, string userRole)
    {
        IEnumerable<Appointment> appointments;

        if (userRole == "Doctor")
        {
            var doctor = await _unitOfWork.Doctor.GetByUserId(userId);
            if (doctor == null) return Result<IEnumerable<AppointmentDetailDto>>.Failure("Doctor profile not found.");
            appointments = await _unitOfWork.Appointment.GetByDoctorId(doctor.Id);
        }
        else if (userRole == "Patient")
        {
            var patient = await _unitOfWork.Patient.GetByUserId(userId); 
            if (patient == null) return Result<IEnumerable<AppointmentDetailDto>>.Failure("Patient profile not found.");
            appointments = await _unitOfWork.Appointment.GetByPatientId(patient.Id);
        }
        else
        {
            return Result<IEnumerable<AppointmentDetailDto>>.Failure("User role is not supported.");
        }

        var appointmentDtos = appointments.Select(a => new AppointmentDetailDto
        {
            Id = a.Id,
            Status = a.Status.ToString(),
            Day = a.Schedule!.Day,
            StartTime = a.Schedule.StartTime,
            DoctorInfo = new DoctorDto
            {
                Id = a.Doctor!.Id,
                FullName = a.Doctor.FullName,
                SpecialtyName = a.Doctor.Specialty?.Name! 
            },
            PatientInfo = new PatientDto
            {
                Id = a.Patient!.Id,
                FullName = a.Patient.FullName
            }
        });

        return Result<IEnumerable<AppointmentDetailDto>>.Success(appointmentDtos);
    }


    public async Task<Result<AppointmentDetailDto>> CreateAppointment(string patientUserId, CreateAppointmentDto dto)
    {
        var patient = await _unitOfWork.Patient.GetByUserId(patientUserId);
        if (patient == null)
            return Result<AppointmentDetailDto>.Failure("Patient profile not found.");

        var scheduleSlot = await _unitOfWork.Schedules.GetById(dto.ScheduleId);
        if (scheduleSlot == null)
            return Result<AppointmentDetailDto>.Failure("The selected time slot is not available.");

        if (scheduleSlot.AppointmentId != null)
            return Result<AppointmentDetailDto>.Failure("This time slot has just been booked by another user.");

        var newAppointment = new Appointment
        {
            PatientId = patient.Id,
            DoctorId = scheduleSlot.DoctorId,
            ScheduleId = scheduleSlot.Id,
            Status = AppointmentStatus.Pending 
        };

        await _unitOfWork.Appointment.Add(newAppointment);
        await _unitOfWork.Complete(); 
        scheduleSlot.AppointmentId = newAppointment.Id;
        await _unitOfWork.Complete();

        var createdAppointment = await _unitOfWork.Appointment.GetByIdWithDetails(newAppointment.Id);

        var appointmentDto = new AppointmentDetailDto
        {
            Id = createdAppointment!.Id,
            Status = createdAppointment.Status.ToString(),
            Day = createdAppointment.Schedule!.Day,
            StartTime = createdAppointment.Schedule.StartTime,
            PatientInfo = new PatientDto
            {
                Id = createdAppointment.Patient!.Id,
                FullName = createdAppointment.Patient.FullName
            },
            DoctorInfo = new DoctorDto
            {
                Id = createdAppointment.Doctor!.Id,
                FullName = createdAppointment.Doctor.FullName,
                SpecialtyName = createdAppointment.Doctor.Specialty!.Name,
                ConsultationFee = createdAppointment.Doctor.ConsultationFee,
                ExperienceYears = createdAppointment.Doctor.ExperienceYears
            }
        };

        return Result<AppointmentDetailDto>.Success(appointmentDto);
    }

    public async Task<Result<bool>> ConfirmAppointment(int appointmentId, string doctorUserId)
    {
        var doctor = await _unitOfWork.Doctor.GetByUserId(doctorUserId);
        if (doctor == null)
            return Result<bool>.Failure("Doctor profile not found.");

        var appointment = await _unitOfWork.Appointment.GetById(appointmentId);
        if (appointment == null)
            return Result<bool>.Failure("Appointment not found.");

        if (appointment.DoctorId != doctor.Id)
            return Result<bool>.Failure("You are not authorized to confirm this appointment.");

        appointment.Status = AppointmentStatus.Confirmed;
        await _unitOfWork.Complete();

        return Result<bool>.Success(true);
    }
    public async Task<Result<bool>> CancelAppointment(int appointmentId, string userId, string userRole)
    {
        var appointment = await _unitOfWork.Appointment.GetById(appointmentId);
        if (appointment == null)
            return Result<bool>.Failure("Appointment not found.");

        var isAuthorized = false;
        if (userRole == "Doctor")
        {
            var doctor = await _unitOfWork.Doctor.GetByUserId(userId);
            if (doctor != null && appointment.DoctorId == doctor.Id)
            {
                isAuthorized = true;
            }
        }
        else if (userRole == "Patient")
        {
            var patient = await _unitOfWork.Patient.GetByUserId(userId);
            if (patient != null && appointment.PatientId == patient.Id)
            {
                isAuthorized = true;
            }
        }

        if (!isAuthorized)
            return Result<bool>.Failure("You are not authorized to cancel this appointment.");

        appointment.Status = AppointmentStatus.Cancelled;
        await _unitOfWork.Complete();

        return Result<bool>.Success(true);
    }
    public async Task<Result<bool>> CompleteAppointment(int appointmentId, string doctorUserId)
    {
        var doctor = await _unitOfWork.Doctor.GetByUserId(doctorUserId);
        if (doctor == null)
            return Result<bool>.Failure("Doctor profile not found.");

        var appointment = await _unitOfWork.Appointment.GetById(appointmentId);
        if (appointment == null)
            return Result<bool>.Failure("Appointment not found.");

        if (appointment.DoctorId != doctor.Id)
            return Result<bool>.Failure("You are not authorized to complete this appointment.");

        appointment.Status = AppointmentStatus.Completed;
        await _unitOfWork.Complete();

        return Result<bool>.Success(true);
    }
}
