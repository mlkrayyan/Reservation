using SpaReservation.Domain.SharedKernel;
namespace SpaReservation.Domain.Waitlist;
public class WaitlistEntry : AggregateRoot
{
    private WaitlistEntry() { }
    public Guid CustomerId { get; private set; }
    public Guid ServiceId { get; private set; }
    public Guid? PreferredStaffId { get; private set; }
    public DateOnly? PreferredDate { get; private set; }
    public WaitlistStatus Status { get; private set; } = WaitlistStatus.Waiting;
    public DateTime? OfferedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static WaitlistEntry Create(Guid customerId, Guid serviceId, Guid? preferredStaffId, DateOnly? preferredDate)
        => new() { CustomerId = customerId, ServiceId = serviceId, PreferredStaffId = preferredStaffId, PreferredDate = preferredDate, CreatedAt = DateTime.UtcNow };

    public void Offer() { Status = WaitlistStatus.Offered; OfferedAt = DateTime.UtcNow; }
    public void MarkBooked() => Status = WaitlistStatus.Booked;
    public void Expire() => Status = WaitlistStatus.Expired;
}
