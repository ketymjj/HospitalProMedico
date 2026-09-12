using HospitalProMedico.Domain.Enums;

namespace HospitalProMedico.Application.DTOs;

public class PatientDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string? SusCardNumber { get; set; }
    public int? HealthPlanId { get; set; }
    public string? HealthPlanName { get; set; }
    public string? HealthPlanCardNumber { get; set; }
    public BloodType? BloodType { get; set; }
    public string? Allergies { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public RegisterStatus Status { get; set; }
}

public class PatientListItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? HealthPlanName { get; set; }
    public DateTime BirthDate { get; set; }
    public RegisterStatus Status { get; set; }
}

public class PatientDetailDto : PatientDto
{
    public List<AppointmentDto> Appointments { get; set; } = new();
    public List<HospitalizationDto> Hospitalizations { get; set; } = new();
    public List<SurgeryDto> Surgeries { get; set; } = new();
    public List<MedicalRecordDto> MedicalRecords { get; set; } = new();
    public List<PaymentDto> Payments { get; set; } = new();
}

public class CreatePatientDto
{
    public string Name { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string? SusCardNumber { get; set; }
    public int? HealthPlanId { get; set; }
    public string? HealthPlanCardNumber { get; set; }
    public BloodType? BloodType { get; set; }
    public string? Allergies { get; set; }
    public string? Notes { get; set; }
}

public class UpdatePatientDto : CreatePatientDto
{
    public RegisterStatus Status { get; set; }
}
