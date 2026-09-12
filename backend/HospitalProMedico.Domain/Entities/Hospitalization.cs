using HospitalProMedico.Domain.Enums;

namespace HospitalProMedico.Domain.Entities;

public class Hospitalization : BaseEntity
{
    public int PatientId { get; set; }
    public Patient Patient { get; set; } = null!;

    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;

    public DateTime AdmissionDate { get; set; }
    public DateTime? ExpectedDischargeDate { get; set; }
    public DateTime? DischargeDate { get; set; }
    public string Room { get; set; } = string.Empty;
    public string Bed { get; set; } = string.Empty;
    public HospitalizationType Type { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public HospitalizationStatus Status { get; set; } = HospitalizationStatus.INTERNADO;
}
