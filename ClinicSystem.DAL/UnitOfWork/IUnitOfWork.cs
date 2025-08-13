using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
         ISpecialtyRepository Specialty { get;  }
         IDoctorRepository Doctor { get;  }
         IWorkingHourRepository WorkingHours { get; }
         IScheduleRepository Schedules { get; }
         IAppointmentRepository Appointment { get; }
         IPatientRepository Patient { get; }
         IReviewRepository Review { get; }
        Task<int> Complete();
    }
}
