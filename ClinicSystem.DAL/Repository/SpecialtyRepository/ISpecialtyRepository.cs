using ClinicSystem.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.DAL;

public interface ISpecialtyRepository : IGenericRepository<Specialty>
{
    Task<IEnumerable<SpecialtyDto>> GetAllAsDto();
    Task<SpecialtyDto?> GetByIdAsDto(int id);
}
