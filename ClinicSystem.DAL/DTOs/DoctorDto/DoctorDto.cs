using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem;

public class DoctorDto
{
    public int Id { get; set; }
    public required string FullName { get; set; } = string.Empty;
    public  string? ImageUrl { get; set; }
    public  string SpecialtyName { get; set; } = string.Empty;
    public  decimal ConsultationFee { get; set; }
    public int ExperienceYears { get; set; }
}
