using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem;

public class PromoteToDoctorDto
{
    [Required]
    public required string UserId { get; set; } 

    [Required]
    public int SpecialtyId { get; set; }

    [Required]
    [Range(0, 10000)]
    public decimal ConsultationFee { get; set; }

    [Required]
    [Range(0, 60)]
    public int ExperienceYears { get; set; }

    [MaxLength(1500)]
    public string Bio { get; set; } = string.Empty;
}
