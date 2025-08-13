using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.DAL;

public class UpdateDoctorProfileDto
{
    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(1500)]
    public string? Bio { get; set; }

    [Required]
    public decimal ConsultationFee { get; set; }

    [Range(0, 60)]
    public int ExperienceYears { get; set; }

    [MaxLength(250)]
    public string? Address { get; set; }
}
