using ClinicSystem.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyContext _context;
        public ISpecialtyRepository Specialty {  get; private set; }
        public IDoctorRepository Doctor {  get; private set; }
        public IWorkingHourRepository WorkingHours { get; private set; }
        public IScheduleRepository Schedules { get; private set; } 
        public IAppointmentRepository Appointment { get; private set; }
        public IPatientRepository Patient { get; private set; }
        public IReviewRepository Review { get; private set; }


        public UnitOfWork(MyContext context)
        {
            this._context = context;
            Specialty = new SpecialtyRepository(_context);
            Doctor = new DoctorRepository(_context);
            WorkingHours = new WorkingHourRepository(_context);
            Schedules = new ScheduleRepository(_context);
            Appointment = new AppointmentRepository(_context);
            Patient = new PatientRepository(_context);
            Review = new ReviewRepository(_context);
        }


        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync(); 
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
