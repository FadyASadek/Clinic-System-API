using ClinicSystem.DAL;
using ClinicSystem.DAL.DTOs.WorkingHourDto;

namespace ClinicSystem.BLL.Interfaces;

public interface IDoctorService
{
    Task<Result<IEnumerable<DoctorDto>>> GetAll(DoctorFilterDto dto);
    Task<Result<DoctorDetailDto>> GetById(int doctorId);
    Task<Result<IEnumerable<ScheduleSlotDto>>> GetAvailableSlots(int doctorId, DateTime day);
    Task<Result<DoctorDetailDto>> GetMyProfile(string userId);   
    Task<Result<DoctorDetailDto>> UpdateMyProfile(string userId, UpdateDoctorProfileDto dto);   
    Task<Result<bool>> SetWorkingHours(string userId, IEnumerable<WorkingHourDto> dto);
}