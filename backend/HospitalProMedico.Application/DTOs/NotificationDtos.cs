using HospitalProMedico.Domain.Enums;

namespace HospitalProMedico.Application.DTOs;

public class NotificationDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public bool Read { get; set; }
    public DateTime CreatedAt { get; set; }
}
