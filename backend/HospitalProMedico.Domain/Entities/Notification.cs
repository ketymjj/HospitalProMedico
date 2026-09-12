using HospitalProMedico.Domain.Enums;

namespace HospitalProMedico.Domain.Entities;

public class Notification : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; } = NotificationType.INFO;
    public bool Read { get; set; } = false;
}
