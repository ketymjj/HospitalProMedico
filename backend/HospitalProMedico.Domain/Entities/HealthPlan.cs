using HospitalProMedico.Domain.Enums;

namespace HospitalProMedico.Domain.Entities;

public class HealthPlan : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public HealthPlanType Type { get; set; }
    public RegisterStatus Status { get; set; } = RegisterStatus.ATIVO;

    public ICollection<Patient> Patients { get; set; } = new List<Patient>();
}
