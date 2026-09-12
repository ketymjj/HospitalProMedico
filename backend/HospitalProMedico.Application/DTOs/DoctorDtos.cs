using HospitalProMedico.Domain.Enums;

namespace HospitalProMedico.Application.DTOs;

public class DoctorDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Crm { get; set; } = string.Empty;
    public Specialty Specialty { get; set; }
    public string Cpf { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Address { get; set; } = string.Empty;
    public RegisterStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class DoctorDetailDto : DoctorDto
{
    public int AppointmentsCount { get; set; }
    public int SurgeriesCount { get; set; }
    public int HospitalizationsCount { get; set; }
}

public class CreateDoctorDto
{
    public string Name { get; set; } = string.Empty;
    public string Crm { get; set; } = string.Empty;
    public Specialty Specialty { get; set; }
    public string Cpf { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Address { get; set; } = string.Empty;
}

public class UpdateDoctorDto : CreateDoctorDto
{
    public RegisterStatus Status { get; set; }
}
