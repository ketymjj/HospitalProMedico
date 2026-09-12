using HospitalProMedico.Domain.Enums;

namespace HospitalProMedico.Application.DTOs;

public class HospitalizationDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public DateTime AdmissionDate { get; set; }
    public DateTime? ExpectedDischargeDate { get; set; }
    public DateTime? DischargeDate { get; set; }
    public string Room { get; set; } = string.Empty;
    public string Bed { get; set; } = string.Empty;
    public HospitalizationType Type { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public HospitalizationStatus Status { get; set; }
}

public class CreateHospitalizationDto
{
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime AdmissionDate { get; set; }
    public DateTime? ExpectedDischargeDate { get; set; }
    public string Room { get; set; } = string.Empty;
    public string Bed { get; set; } = string.Empty;
    public HospitalizationType Type { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public string? Notes { get; set; }
}

public class UpdateHospitalizationDto : CreateHospitalizationDto
{
    public DateTime? DischargeDate { get; set; }
    public HospitalizationStatus Status { get; set; }
}
