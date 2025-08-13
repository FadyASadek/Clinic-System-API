using ClinicSystem.DAL.Data;
using ClinicSystem.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.DAL;

public class WorkingHourRepository : GenericRepository<WorkingHour>, IWorkingHourRepository
{
    public WorkingHourRepository(MyContext context) : base(context) { }

    public async Task<IEnumerable<WorkingHour>> GetByDoctorId(int doctorId)
    {
        return await _context.WorkingHours
            .Where(wh => wh.DoctorId == doctorId)
            .ToListAsync();
    }

    public void RemoveRange(IEnumerable<WorkingHour> entities)
    {
        _context.WorkingHours.RemoveRange(entities);
    }
    public async Task<WorkingHour?> GetByDoctorIdAndDay(int doctorId, DayOfWeek day)
    {
        return await _context.WorkingHours
            .FirstOrDefaultAsync(wh => wh.DoctorId == doctorId && wh.DayOfWeek == day);
    }
}
