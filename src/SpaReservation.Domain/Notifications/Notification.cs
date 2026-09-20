using SpaReservation.Domain.SharedKernel;
namespace SpaReservation.Domain.Notifications;
public class Notification : Entity
{
    private Notification() { }
    public Guid BookingId { get; private set; }
    public Guid CustomerId { get; private set; }
    public NotificationChannel Channel { get; private set; }
    public NotificationType Type { get; private set; }
    public string Status { get; private set; } = "Pending";
    public DateTime? SentAt { get; private set; }
    public string? Error { get; private set; }
    public int RetryCount { get; private set; }

    public static Notification Create(Guid bookingId, Guid customerId, NotificationChannel channel, NotificationType type)
        => new() { BookingId = bookingId, CustomerId = customerId, Channel = channel, Type = type };

    public void MarkSent() { Status = "Sent"; SentAt = DateTime.UtcNow; }
    public void MarkFailed(string error) { Status = "Failed"; Error = error; RetryCount++; }
}
