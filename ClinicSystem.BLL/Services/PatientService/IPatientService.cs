using ClinicSystem.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.BLL;

public interface IPatientService
{
    Task<Result<PatientDetailDto>> GetMyProfile(string userId);

    Task<Result<PatientDetailDto>> UpdateMyProfile(string userId, UpdatePatientProfileDto dto);
}
