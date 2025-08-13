using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.DAL;

public class CreateAppointmentDto
{
    [Required]
    public int ScheduleId { get; set; }
}