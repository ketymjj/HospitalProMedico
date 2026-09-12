namespace HospitalProMedico.Application.DTOs;

public class MedicalRecordDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string ChiefComplaint { get; set; } = string.Empty;
    public string History { get; set; } = string.Empty;
    public string Diagnosis { get; set; } = string.Empty;
    public string Prescription { get; set; } = string.Empty;
    public string? Exams { get; set; }
    public string? Notes { get; set; }
}

public class CreateMedicalRecordDto
{
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime Date { get; set; }
    public string ChiefComplaint { get; set; } = string.Empty;
    public string History { get; set; } = string.Empty;
    public string Diagnosis { get; set; } = string.Empty;
    public string Prescription { get; set; } = string.Empty;
    public string? Exams { get; set; }
    public string? Notes { get; set; }
}

public class UpdateMedicalRecordDto : CreateMedicalRecordDto
{
}
