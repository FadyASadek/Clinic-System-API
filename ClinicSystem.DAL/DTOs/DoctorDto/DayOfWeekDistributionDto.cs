using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.DAL;

public class DayOfWeekDistributionDto
{
    public required string Day { get; set; }
    public int AppointmentsCount { get; set; }
}
