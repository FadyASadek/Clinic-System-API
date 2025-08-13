using ClinicSystem.DAL.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicSystem.DAL;

public class Patient : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(10)]
    public string? Gender { get; set; }

    public DateTime? DateOfBirth { get; set; }

    [MaxLength(15)]
    public string? PhoneNumber { get; set; }

    [Required]
    public string ApplicationUserId { get; set; } = string.Empty;
    public ApplicationUser? ApplicationUser { get; set; }

    public ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
    public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
}