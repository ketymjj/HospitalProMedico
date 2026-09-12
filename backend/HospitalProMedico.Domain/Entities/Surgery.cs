using HospitalProMedico.Domain.Enums;

namespace HospitalProMedico.Domain.Entities;

public class Surgery : BaseEntity
{
    public int PatientId { get; set; }
    public Patient Patient { get; set; } = null!;

    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;

    public string Type { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Time { get; set; } = string.Empty;
    public string Room { get; set; } = string.Empty;
    public int EstimatedDurationMinutes { get; set; }
    public SurgeryStatus Status { get; set; } = SurgeryStatus.AGENDADA;
    public string? Notes { get; set; }
}
