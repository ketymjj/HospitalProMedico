namespace HospitalProMedico.Domain.Entities;

public class MedicalRecord : BaseEntity
{
    public int PatientId { get; set; }
    public Patient Patient { get; set; } = null!;

    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;

    public DateTime Date { get; set; }
    public string ChiefComplaint { get; set; } = string.Empty;
    public string History { get; set; } = string.Empty;
    public string Diagnosis { get; set; } = string.Empty;
    public string Prescription { get; set; } = string.Empty;
    public string? Exams { get; set; }
    public string? Notes { get; set; }
}
