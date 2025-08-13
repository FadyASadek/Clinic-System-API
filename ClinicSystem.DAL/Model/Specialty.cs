using ClinicSystem.DAL.Model;
using System.ComponentModel.DataAnnotations;

namespace ClinicSystem.DAL;

public class Specialty : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public ICollection<Doctor> Doctors { get; set; } = new HashSet<Doctor>();
}