using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SpaReservation.Application.BackgroundJobs;
using SpaReservation.Domain.Bookings;
using SpaReservation.Domain.Notifications;
using SpaReservation.Domain.Waitlist;
using SpaReservation.Infrastructure.Data;

namespace SpaReservation.Infrastructure.BackgroundJobs;

public class BookingReminderJob : IBookingReminderJob
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<BookingReminderJob> _logger;

    public BookingReminderJob(ApplicationDbContext context, ILogger<BookingReminderJob> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Send24hRemindersAsync()
    {
        var targetDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));
        _logger.LogInformation("Executing Hangfire 24h booking reminder check for date: {Date}", targetDate);

        var upcomingBookings = await _context.Bookings
            .Where(b => b.Status == BookingStatus.Confirmed && b.Slot.Date == targetDate)
            .ToListAsync();

        foreach (var booking in upcomingBookings)
        {
            var notification = Notification.Create(
                booking.Id,
                booking.CustomerId,
                NotificationChannel.Sms,
                NotificationType.BookingReminder24h
            );
            notification.MarkSent();
            await _context.Notifications.AddAsync(notification);
        }

        if (upcomingBookings.Any())
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Dispatched 24h reminder notifications for {Count} bookings", upcomingBookings.Count);
        }
    }

    public async Task Send1hRemindersAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var nextHour = TimeOnly.FromDateTime(DateTime.UtcNow.AddHours(1));
        _logger.LogInformation("Executing Hangfire 1h booking reminder check for date: {Date} at ~{Time}", today, nextHour);

        var upcomingBookings = await _context.Bookings
            .Where(b => b.Status == BookingStatus.Confirmed && b.Slot.Date == today)
            .ToListAsync();

        foreach (var booking in upcomingBookings)
        {
            var notification = Notification.Create(
                booking.Id,
                booking.CustomerId,
                NotificationChannel.Push,
                NotificationType.BookingReminder1h
            );
            notification.MarkSent();
            await _context.Notifications.AddAsync(notification);
        }

        if (upcomingBookings.Any())
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Dispatched 1h reminder notifications for {Count} bookings", upcomingBookings.Count);
        }
    }

    public async Task ExpireWaitlistEntriesAsync()
    {
        _logger.LogInformation("Executing Hangfire waitlist expiration job...");

        var cutoffDate = DateTime.UtcNow.AddDays(-2);
        var staleEntries = await _context.Waitlist
            .Where(w => w.Status == WaitlistStatus.Offered && w.CreatedAt < cutoffDate)
            .ToListAsync();

        foreach (var entry in staleEntries)
        {
            entry.Expire();
        }

        if (staleEntries.Any())
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Expired {Count} stale waitlist entries older than 48 hours", staleEntries.Count);
        }
    }
}
