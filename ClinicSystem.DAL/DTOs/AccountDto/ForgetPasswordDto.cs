using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.DAL;

public class ForgetPasswordDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
}
