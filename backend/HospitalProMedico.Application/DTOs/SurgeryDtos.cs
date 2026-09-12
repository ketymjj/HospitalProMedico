using HospitalProMedico.Domain.Enums;

namespace HospitalProMedico.Application.DTOs;

public class SurgeryDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Time { get; set; } = string.Empty;
    public string Room { get; set; } = string.Empty;
    public int EstimatedDurationMinutes { get; set; }
    public SurgeryStatus Status { get; set; }
    public string? Notes { get; set; }
}

public class CreateSurgeryDto
{
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public string Type { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Time { get; set; } = string.Empty;
    public string Room { get; set; } = string.Empty;
    public int EstimatedDurationMinutes { get; set; }
    public string? Notes { get; set; }
}

public class UpdateSurgeryDto : CreateSurgeryDto
{
    public SurgeryStatus Status { get; set; }
}
