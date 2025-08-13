using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.DAL;

public class AppointmentDetailDto
{
    public int Id { get; set; }
    public required string Status { get; set; } 
    public DateTime Day { get; set; }
    public TimeSpan StartTime { get; set; }
    public required DoctorDto DoctorInfo { get; set; }
    public required PatientDto PatientInfo { get; set; }
}