using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.DAL.DTOs;

public class LoginResponseDto
{
    public required string Token { get; set; }
    public required DateTime ExpiresOn { get; set; }
}
