using ClinicSystem.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.DAL;

public interface IPatientRepository : IGenericRepository<Patient>
{
    Task<Patient?> GetByUserId(string userId);
    Task<int> Count();

}
