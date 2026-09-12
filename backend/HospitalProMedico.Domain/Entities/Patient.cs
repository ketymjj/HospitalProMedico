using HospitalProMedico.Domain.Enums;

namespace HospitalProMedico.Domain.Entities;

public class Patient : BaseEntity
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
    public HealthPlan? HealthPlan { get; set; }
    public string? HealthPlanCardNumber { get; set; }

    public BloodType? BloodType { get; set; }
    public string? Allergies { get; set; }
    public string? Notes { get; set; }
    public RegisterStatus Status { get; set; } = RegisterStatus.ATIVO;

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<Hospitalization> Hospitalizations { get; set; } = new List<Hospitalization>();
    public ICollection<Surgery> Surgeries { get; set; } = new List<Surgery>();
    public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
