using ClinicSystem.DAL;
using ClinicSystem.DAL.UnitOfWork;

namespace ClinicSystem.BLL.Services;

public class PatientService : IPatientService
{
    private readonly IUnitOfWork _unitOfWork;

    public PatientService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PatientDetailDto>> GetMyProfile(string userId)
    {
        var patient = await _unitOfWork.Patient.GetByUserId(userId);
        if (patient == null)
            return Result<PatientDetailDto>.Failure("Patient profile not found.");

        var patientDto = new PatientDetailDto
        {
            Id = patient.Id,
            FullName = patient.FullName,
            Gender = patient.Gender,
            DateOfBirth = patient.DateOfBirth,
            PhoneNumber = patient.PhoneNumber
        };

        return Result<PatientDetailDto>.Success(patientDto);
    }

    public async Task<Result<PatientDetailDto>> UpdateMyProfile(string userId, UpdatePatientProfileDto dto)
    {
        var patient = await _unitOfWork.Patient.GetByUserId(userId);
        if (patient == null)
            return Result<PatientDetailDto>.Failure("Patient profile not found.");

        patient.FullName = dto.FullName;
        patient.Gender = dto.Gender;
        patient.DateOfBirth = dto.DateOfBirth;
        patient.PhoneNumber = dto.PhoneNumber;
        patient.UpdatedAt = DateTime.Now;

        await _unitOfWork.Complete();

        var updatedDto = new PatientDetailDto
        {
            Id = patient.Id,
            FullName = patient.FullName,
            Gender = patient.Gender,
            DateOfBirth = patient.DateOfBirth,
            PhoneNumber = patient.PhoneNumber
        };

        return Result<PatientDetailDto>.Success(updatedDto);
    }
}