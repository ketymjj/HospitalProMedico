using HospitalProMedico.Domain.Enums;

namespace HospitalProMedico.Application.DTOs;

public class AppointmentDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public Specialty Specialty { get; set; }
    public DateTime Date { get; set; }
    public string Time { get; set; } = string.Empty;
    public AppointmentType Type { get; set; }
    public AppointmentStatus Status { get; set; }
    public string? Notes { get; set; }
}

public class CreateAppointmentDto
{
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime Date { get; set; }
    public string Time { get; set; } = string.Empty;
    public AppointmentType Type { get; set; }
    public string? Notes { get; set; }
}

public class UpdateAppointmentDto : CreateAppointmentDto
{
    public AppointmentStatus Status { get; set; }
}

public class UpdateAppointmentStatusDto
{
    public AppointmentStatus Status { get; set; }
}
