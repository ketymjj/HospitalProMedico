using HospitalProMedico.Domain.Enums;

namespace HospitalProMedico.Domain.Entities;

public class Doctor : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Crm { get; set; } = string.Empty;
    public Specialty Specialty { get; set; }
    public string Cpf { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Address { get; set; } = string.Empty;
    public RegisterStatus Status { get; set; } = RegisterStatus.ATIVO;

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<Hospitalization> Hospitalizations { get; set; } = new List<Hospitalization>();
    public ICollection<Surgery> Surgeries { get; set; } = new List<Surgery>();
    public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
}
