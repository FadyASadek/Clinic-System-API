using ClinicSystem.DAL.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicSystem.DAL;

public class Doctor : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(1500)]
    public string Bio { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal ConsultationFee { get; set; }

    [Range(0, 60)]
    public int ExperienceYears { get; set; }

    public bool IsVerified { get; set; } = false;

    [MaxLength(250)]
    public string? Address { get; set; }

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    [Required]
    public string ApplicationUserId { get; set; } = string.Empty;
    public ApplicationUser? ApplicationUser { get; set; }

    public int SpecialtyId { get; set; }
    public Specialty? Specialty { get; set; }

    public ICollection<WorkingHour> WorkingHours { get; set; } = new HashSet<WorkingHour>();
    public ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
    public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
}