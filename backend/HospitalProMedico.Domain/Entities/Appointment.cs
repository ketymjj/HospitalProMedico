using HospitalProMedico.Domain.Enums;

namespace HospitalProMedico.Domain.Entities;

public class Appointment : BaseEntity
{
    public int PatientId { get; set; }
    public Patient Patient { get; set; } = null!;

    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;

    public DateTime Date { get; set; }
    public string Time { get; set; } = string.Empty;
    public AppointmentType Type { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.AGENDADA;
    public string? Notes { get; set; }
}
