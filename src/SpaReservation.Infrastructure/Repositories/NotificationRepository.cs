using Microsoft.EntityFrameworkCore;
using SpaReservation.Domain.Notifications;
using SpaReservation.Infrastructure.Data;

namespace SpaReservation.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly ApplicationDbContext _context;
    
    public NotificationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Notification>> GetPendingAsync(CancellationToken ct = default)
    {
        return await _context.Notifications
            .Where(n => n.Status == "Pending" && n.RetryCount < 3)
            .ToListAsync(ct);
    }

    public async Task AddAsync(Notification notification, CancellationToken ct = default)
    {
        await _context.Notifications.AddAsync(notification, ct);
    }

    public Task UpdateAsync(Notification notification, CancellationToken ct = default)
    {
        _context.Notifications.Update(notification);
        return Task.CompletedTask;
    }
}
