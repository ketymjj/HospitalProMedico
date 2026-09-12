namespace HospitalProMedico.Domain.Entities;

public class Hospital : BaseEntity
{
    public string Name { get; set; } = "Hospital ProMédico";
    public string Cnpj { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
}
