using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.DAL;

public class SpecialtyDto
{
    public int id { get; set; }
    public required string name { get; set; }
    public required string description { get; set; }
    public  DateTime CreateAt { get; set; }
    public  DateTime LastUpdate { get; set; }
}
