using ClinicSystem.BLL.Interfaces;
using ClinicSystem.DAL;
using ClinicSystem.DAL.DTOs.WorkingHourDto;
using ClinicSystem.DAL.Model;
using ClinicSystem.DAL.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace ClinicSystem.BLL.Services;

public class DoctorService : IDoctorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;

    public DoctorService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<Result<IEnumerable<DoctorDto>>> GetAll(DoctorFilterDto filter)
    {
        var doctors = await _unitOfWork.Doctor.Filter(filter);

        var doctorsDto = doctors.Select(d => new DoctorDto
        {
            Id = d.Id,
            FullName = d.FullName,
            ImageUrl = d.ImageUrl,
            ConsultationFee = d.ConsultationFee,
            SpecialtyName = d.Specialty!.Name,
        });

        return Result<IEnumerable<DoctorDto>>.Success(doctorsDto);
    }

    public async Task<Result<DoctorDetailDto>> GetById(int doctorId)
    {
        var doctor = await _unitOfWork.Doctor.GetByIdWithSpecialty(doctorId);
        if (doctor is null)
            return Result<DoctorDetailDto>.Failure("Doctor not found.");

        var doctorDto = new DoctorDetailDto
        {
            Id = doctor.Id,
            FullName = doctor.FullName,
            ImageUrl = doctor.ImageUrl,
            ConsultationFee = doctor.ConsultationFee,
            SpecialtyName = doctor.Specialty!.Name,
            Bio = doctor.Bio,
            ExperienceYears = doctor.ExperienceYears,
            Address = doctor.Address
        };
        return Result<DoctorDetailDto>.Success(doctorDto);
    }

    public async Task<Result<DoctorDetailDto>> GetMyProfile(string userId)
    {
        var doctor = await _unitOfWork.Doctor.GetByUserId(userId);
        if (doctor == null)
            return Result<DoctorDetailDto>.Failure("Doctor profile not found for the current user.");

        var doctorDto = new DoctorDetailDto
        {
            Id = doctor.Id,
            FullName = doctor.FullName,
            ImageUrl = doctor.ImageUrl,
            ConsultationFee = doctor.ConsultationFee,
            SpecialtyName = doctor.Specialty!.Name,
            Bio = doctor.Bio,
            ExperienceYears = doctor.ExperienceYears,
            Address = doctor.Address
        };
        return Result<DoctorDetailDto>.Success(doctorDto);
    }

    public async Task<Result<DoctorDetailDto>> UpdateMyProfile(string userId, UpdateDoctorProfileDto dto)
    {
        var doctor = await _unitOfWork.Doctor.GetByUserId(userId);
        if (doctor == null)
            return Result<DoctorDetailDto>.Failure("Doctor profile not found.");

        doctor.FullName = dto.FullName;
        doctor.Bio = dto.Bio!;
        doctor.ConsultationFee = dto.ConsultationFee;
        doctor.ExperienceYears = dto.ExperienceYears;
        doctor.Address = dto.Address;
        doctor.UpdatedAt = DateTime.Now;

        await _unitOfWork.Complete();

        var updatedDto = new DoctorDetailDto
        {
            Id = doctor.Id,
            FullName = doctor.FullName,
            ImageUrl = doctor.ImageUrl,
            ConsultationFee = doctor.ConsultationFee,
            SpecialtyName = doctor.Specialty!.Name,
            Bio = doctor.Bio,
            ExperienceYears = doctor.ExperienceYears,
            Address = doctor.Address
        };
        return Result<DoctorDetailDto>.Success(updatedDto);
    }

    public async Task<Result<bool>> SetWorkingHours(string userId, IEnumerable<WorkingHourDto> dto)
    {
        var doctor = await _unitOfWork.Doctor.GetByUserId(userId);
        if (doctor == null)
            return Result<bool>.Failure("Doctor profile not found.");

        var existingHours = await _unitOfWork.WorkingHours.GetByDoctorId(doctor.Id);

        _unitOfWork.WorkingHours.RemoveRange(existingHours);

        foreach (var hour in dto)
        {
            var newHour = new WorkingHour
            {
                DoctorId = doctor.Id,
                DayOfWeek = hour.DayOfWeek,
                StartTime = hour.StartTime,
                EndTime = hour.EndTime
            };
            await _unitOfWork.WorkingHours.Add(newHour);
        }

        await _unitOfWork.Complete();

        return Result<bool>.Success(true);
    }

    public async Task<Result<IEnumerable<ScheduleSlotDto>>> GetAvailableSlots(int doctorId, DateTime day)
    {
        var existingSlots = await _unitOfWork.Schedules.GetByDoctorIdAndDay(doctorId, day);
        if (!existingSlots.Any())
        {
            
            var workingHour = await _unitOfWork.WorkingHours.GetByDoctorIdAndDay(doctorId, day.DayOfWeek);
            if (workingHour == null)
            {
                return Result<IEnumerable<ScheduleSlotDto>>.Success(new List<ScheduleSlotDto>());
            }

            var slotDuration = TimeSpan.FromMinutes(30);
            var currentTime = workingHour.StartTime;
            while (currentTime < workingHour.EndTime)
            {
                var newSlot = new Schedule
                {
                    DoctorId = doctorId,
                    Day = day.Date,
                    StartTime = currentTime,
                    EndTime = currentTime.Add(slotDuration)
                };
                await _unitOfWork.Schedules.Add(newSlot);
                currentTime = currentTime.Add(slotDuration);
            }

            await _unitOfWork.Complete();

            existingSlots = await _unitOfWork.Schedules.GetByDoctorIdAndDay(doctorId, day);
        }

        var availableSlots = existingSlots
            .Where(s => s.AppointmentId == null) 
            .Select(s => new ScheduleSlotDto
            {
                ScheduleId = s.Id,
                StartTime = s.StartTime,
                EndTime = s.EndTime
            });

        return Result<IEnumerable<ScheduleSlotDto>>.Success(availableSlots);
    }
}