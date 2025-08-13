using ClinicSystem.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.BLL;

public interface ISpecialtyService
{
    Task<Result<IEnumerable<SpecialtyDto>>> GetAll();
    Task<Result<SpecialtyDto>> GetById(int id);
    Task<Result<SpecialtyDto>> Add(CreateSpecialtyDto createSpecialtyDto);
    Task<Result<bool>> Update(int id,CreateSpecialtyDto specialtyDto);
    Task<Result<bool>> DeleteById(int id);
}
