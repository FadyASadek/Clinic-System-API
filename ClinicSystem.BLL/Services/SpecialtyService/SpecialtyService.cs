using ClinicSystem.DAL;
using ClinicSystem.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClinicSystem.BLL;

public class SpecialtyService : ISpecialtyService 
{
    private readonly IUnitOfWork _unitOfWork;

    public SpecialtyService(IUnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
    }


    public async Task<Result<IEnumerable<SpecialtyDto>>> GetAll()
    {
        var result =await _unitOfWork.Specialty.GetAllAsDto();
        return Result<IEnumerable<SpecialtyDto>>.Success(result);
    }

    public async Task<Result<SpecialtyDto>> GetById(int id)
    {
        var result = await _unitOfWork.Specialty.GetByIdAsDto(id);
        if (result is null)
        {
            return Result<SpecialtyDto>.Failure("There are currently no specialties available in the system.");
        }
        return Result<SpecialtyDto>.Success(result);
    }


    public  async Task<Result<SpecialtyDto>> Add(CreateSpecialtyDto createSpecialtyDto)
    {
        var nameExists = await _unitOfWork.Specialty.Any(s => s.Name == createSpecialtyDto.name);
        if (nameExists)
        {
            return Result<SpecialtyDto>.Failure("A specialty with this name already exists.");
        }
        var specialty = new Specialty
        {
            Name = createSpecialtyDto.name,
            Description = createSpecialtyDto.description,   
            CreatedAt = DateTime.Now,
        };
        await _unitOfWork.Specialty.Add(specialty);
        await _unitOfWork.Complete();

        var resultDto = new SpecialtyDto
        {
            id = specialty.Id, 
            name = specialty.Name,
            description = specialty.Description,
            CreateAt = DateTime.UtcNow,           
        };

        return Result<SpecialtyDto>.Success(resultDto);
    }

    public async Task<Result<bool>> Update(int id,CreateSpecialtyDto specialtyDto)
    {
        var specialty = await _unitOfWork.Specialty.GetById(id);

        if (specialty is null)
        {
            return Result<bool>.Failure("Specialty not found.");
        }
        var nameExists = await _unitOfWork.Specialty
            .Any(s => s.Name == specialtyDto.name && s.Id != id);

        if (nameExists)
            return Result<bool>.Failure("A specialty with this name already exists.");

        specialty.Name = specialtyDto.name;
        specialty.Description = specialtyDto.description;
        specialty.UpdatedAt = DateTime.Now;

        await _unitOfWork.Specialty.Update(specialty);
        await _unitOfWork.Complete();

        return Result<bool>.Success(true);
    }


    public async Task<Result<bool>> DeleteById(int id)
    {
        var result = await _unitOfWork.Specialty.GetById(id);
        if (result is null)
        {
            return Result<bool>.Failure("Specialty not found.");
        }
        await _unitOfWork.Specialty.Delete(result);
        await _unitOfWork.Complete();
        return Result<bool>.Success(true);
    }

   
}
