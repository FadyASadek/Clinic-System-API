using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.DAL;

public class CreateSpecialtyDto
{
    [Required]
    [MinLength(3),MaxLength(50)]
    public required string name { get; set; }
    [MinLength(3), MaxLength(1500)]
    public required string description { get; set; }
}
