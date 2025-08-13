using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.DAL;

public class TopDoctorDto
{
    public int DoctorId { get; set; }
    public required string FullName { get; set; }
    public required string SpecialtyName { get; set; }
    public int CompletedAppointments { get; set; }
}
