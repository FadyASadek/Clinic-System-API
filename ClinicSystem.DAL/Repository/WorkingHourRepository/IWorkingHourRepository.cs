using ClinicSystem.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.DAL;

public interface IWorkingHourRepository :  IGenericRepository<WorkingHour>
{
    Task<IEnumerable<WorkingHour>> GetByDoctorId(int doctorId);

    void RemoveRange(IEnumerable<WorkingHour> entities);

    Task<WorkingHour?> GetByDoctorIdAndDay(int doctorId, DayOfWeek day);


}
