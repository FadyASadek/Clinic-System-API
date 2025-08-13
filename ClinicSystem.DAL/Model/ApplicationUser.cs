using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.DAL.Model;

public class ApplicationUser : IdentityUser
{
    public string Address { get; set; } = string.Empty;
    public Doctor? Doctor { get; set; }
    public Patient? Patient { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
