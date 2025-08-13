using ClinicSystem.DAL.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.DAL;

public class Appointment:BaseEntity
{
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

    [ForeignKey(nameof(Doctor))]
    public int DoctorId { get; set; }
    public Doctor? Doctor { get; set; }

    [ForeignKey(nameof(Patient))]
    public int PatientId { get; set; }
    public Patient? Patient { get; set; }

    [ForeignKey(nameof(Schedule))]
    public int ScheduleId { get; set; }
    public Schedule? Schedule { get; set; }

    public Review? Review { get; set; }
}
