using HospitalProMedico.Domain.Enums;

namespace HospitalProMedico.Application.DTOs;

public class HealthPlanDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public HealthPlanType Type { get; set; }
    public RegisterStatus Status { get; set; }
    public int PatientsCount { get; set; }
}

public class CreateHealthPlanDto
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public HealthPlanType Type { get; set; }
}

public class UpdateHealthPlanDto : CreateHealthPlanDto
{
    public RegisterStatus Status { get; set; }
}
