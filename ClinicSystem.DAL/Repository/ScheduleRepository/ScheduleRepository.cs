using ClinicSystem.DAL.Data;
using ClinicSystem.DAL.Model;
using ClinicSystem.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.DAL;

public class ScheduleRepository : GenericRepository<Schedule>, IScheduleRepository
{
    public ScheduleRepository(MyContext context) : base(context) { }

    public async Task<IEnumerable<Schedule>> GetByDoctorIdAndDay(int doctorId, DateTime day)
    {
        return await _context.Schedules
            .Where(s => s.DoctorId == doctorId && s.Day.Date == day.Date)
            .ToListAsync();
    }
}
