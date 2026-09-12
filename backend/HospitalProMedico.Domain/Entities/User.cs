using HospitalProMedico.Domain.Enums;

namespace HospitalProMedico.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public bool IsActive { get; set; } = true;

    public int? DoctorId { get; set; }
    public Doctor? Doctor { get; set; }
}
