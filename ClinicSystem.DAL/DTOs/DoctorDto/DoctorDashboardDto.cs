using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.DAL;

public class DoctorDashboardDto
{
    public int TotalAppointments { get; set; }
    public int UpcomingAppointments { get; set; }
    public double AverageRating { get; set; }
    public decimal TotalRevenue { get; set; }
}
