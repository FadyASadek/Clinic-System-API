using ClinicSystem.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.DAL;

public interface IDoctorRepository : IGenericRepository<Doctor>
{
    Task<IEnumerable<Doctor>> GetAllWithSpecialty();
    Task<Doctor?> GetByIdWithSpecialty(int doctorId);
    Task<Doctor?> GetByUserId(string userId);
    Task<int> Count();
    Task<IEnumerable<Doctor>> Filter(DoctorFilterDto filter);
    Task<IEnumerable<TopDoctorDto>> GetTopDoctors(int count);
}
