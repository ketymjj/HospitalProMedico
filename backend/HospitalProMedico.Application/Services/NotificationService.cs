using HospitalProMedico.Application.Common;
using HospitalProMedico.Application.DTOs;
using HospitalProMedico.Application.Interfaces;
using HospitalProMedico.Application.Mappings;
using HospitalProMedico.Domain.Entities;
using HospitalProMedico.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalProMedico.Application.Services;

public class NotificationService : INotificationService
{
    private readonly IRepository<Notification> _notifications;
    private readonly ICurrentUserService _currentUser;

    public NotificationService(IRepository<Notification> notifications, ICurrentUserService currentUser)
    {
        _notifications = notifications;
        _currentUser = currentUser;
    }

    public async Task<List<NotificationDto>> GetForCurrentUserAsync()
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedAppException("Usuário não autenticado.");

        var list = await _notifications.Query()
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Take(30)
            .ToListAsync();

        return list.Select(n => n.ToDto()).ToList();
    }

    public async Task MarkAsReadAsync(int id)
    {
        var notification = await _notifications.GetByIdAsync(id) ?? throw new NotFoundException("Notificação", id);
        notification.Read = true;
        _notifications.Update(notification);
        await _notifications.SaveChangesAsync();
    }

    public async Task MarkAllAsReadAsync()
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedAppException("Usuário não autenticado.");
        var list = await _notifications.Query().Where(n => n.UserId == userId && !n.Read).ToListAsync();

        foreach (var n in list)
        {
            n.Read = true;
            _notifications.Update(n);
        }

        await _notifications.SaveChangesAsync();
    }
}
