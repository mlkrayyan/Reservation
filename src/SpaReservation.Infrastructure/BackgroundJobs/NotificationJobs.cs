using SpaReservation.Domain.Notifications;

namespace SpaReservation.Infrastructure.BackgroundJobs;

public interface INotificationJobs
{
    Task ProcessPendingNotificationsAsync(CancellationToken ct = default);
}

public class NotificationJobs : INotificationJobs
{
    private readonly INotificationRepository _notificationRepository;
    
    public NotificationJobs(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task ProcessPendingNotificationsAsync(CancellationToken ct = default)
    {
        var pending = await _notificationRepository.GetPendingAsync(ct);
        foreach (var notification in pending)
        {
            // Simulate sending
            notification.MarkSent();
            await _notificationRepository.UpdateAsync(notification, ct);
        }
    }
}
