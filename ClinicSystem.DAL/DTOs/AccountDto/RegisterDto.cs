using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.DAL.DTOs.AccountDTO;

public class RegisterDto
{
    [Required]
    public required string UserName { get; set; } =string.Empty;
    [Required]
    [EmailAddress]
    public required string Email { get; set; } =string.Empty ;
    [Required]
    public required string Address { get; set; } = string.Empty;
    [Required]
    public required string Password { get; set; } = string.Empty;
}
