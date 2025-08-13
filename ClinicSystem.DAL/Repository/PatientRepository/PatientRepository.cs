using ClinicSystem.DAL.Data;
using ClinicSystem.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.DAL;

public class PatientRepository : GenericRepository<Patient> , IPatientRepository
{
    public PatientRepository(MyContext context) : base(context) { }

    public async Task<Patient?> GetByUserId(string userId)
    {
        return await _context.Patients.FirstOrDefaultAsync(p => p.ApplicationUserId == userId);
    }
    public async Task<int> Count()
    {
        return await _context.Patients.CountAsync();
    }
}
