using SpaReservation.Domain.SharedKernel;
namespace SpaReservation.Domain.Customers;
public class LoyaltyTransaction : Entity
{
    private LoyaltyTransaction() { }
    public Guid CustomerId { get; private set; }
    public int PointsDelta { get; private set; }
    public string Reason { get; private set; } = string.Empty;
    public Guid? BookingId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static LoyaltyTransaction Create(Guid customerId, int pointsDelta, string reason, Guid? bookingId = null)
        => new() { CustomerId = customerId, PointsDelta = pointsDelta, Reason = reason, BookingId = bookingId, CreatedAt = DateTime.UtcNow };
}
