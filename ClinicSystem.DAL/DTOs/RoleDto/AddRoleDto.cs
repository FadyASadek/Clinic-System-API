using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.DAL.DTOs;

public class AddRoleDto
{
    [Required]
    [MinLength(3),MaxLength(30)]
    public required string RoleName { get; set; }
}
