using ClinicSystem.DAL.Data;
using ClinicSystem.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.DAL;

public class SpecialtyRepository : GenericRepository<Specialty>, ISpecialtyRepository 
{
    public SpecialtyRepository(MyContext context) : base(context) 
    {
        
    }

    public async Task<IEnumerable<SpecialtyDto>> GetAllAsDto()
    {
        return await _context.Specialties
            .AsNoTracking()
            .Select(s => new SpecialtyDto
            {
                id = s.Id,
                name = s.Name,
                description = s.Description ?? "",
                CreateAt = DateTime.Now,
            })
            .ToListAsync();
    }

    public async Task<SpecialtyDto?> GetByIdAsDto(int id)
    {
        return await _context.Specialties
            .Where(s => s.Id == id)
            .Select(s => new SpecialtyDto 
            {
                id = s.Id,
                name = s.Name,
                description = s.Description ?? "",
                CreateAt = DateTime.Now,
            })
            .FirstOrDefaultAsync();
    }
}
