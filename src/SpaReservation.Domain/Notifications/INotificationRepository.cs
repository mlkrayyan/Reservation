using SpaReservation.Domain.SharedKernel;
namespace SpaReservation.Domain.Notifications;
public interface INotificationRepository
{
    Task<IEnumerable<Notification>> GetPendingAsync(CancellationToken ct = default);
    Task AddAsync(Notification notification, CancellationToken ct = default);
    Task UpdateAsync(Notification notification, CancellationToken ct = default);
}
