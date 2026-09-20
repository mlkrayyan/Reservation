using SpaReservation.Domain.Bookings.Events;
using SpaReservation.Domain.SharedKernel;
namespace SpaReservation.Domain.Bookings;

public class Booking : AggregateRoot
{
    private Booking() { } // EF Core

    public Guid CustomerId { get; private set; }
    public Guid StaffId { get; private set; }
    public Guid ServiceId { get; private set; }
    public BookingSlot Slot { get; private set; } = null!;
    public BookingStatus Status { get; private set; }
    public string? Notes { get; private set; }
    public int LoyaltyPointsEarned { get; private set; }
    public string? CancellationReason { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static Booking Create(Guid customerId, Guid staffId, Guid serviceId, BookingSlot slot, string? notes)
    {
        var booking = new Booking
        {
            CustomerId = customerId,
            StaffId = staffId,
            ServiceId = serviceId,
            Slot = slot,
            Status = BookingStatus.Pending,
            Notes = notes,
            CreatedAt = DateTime.UtcNow
        };
        booking.AddDomainEvent(new BookingCreated(booking.Id, customerId, staffId));
        return booking;
    }

    public void Confirm()
    {
        if (Status != BookingStatus.Pending)
            throw new InvalidOperationException("Only pending bookings can be confirmed.");
        Status = BookingStatus.Confirmed;
        AddDomainEvent(new BookingConfirmed(Id, CustomerId));
    }

    public void Cancel(string reason)
    {
        if (Status == BookingStatus.Cancelled || Status == BookingStatus.Completed)
            throw new InvalidOperationException("Booking cannot be cancelled in its current state.");
        if (Slot.StartDateTime - DateTime.UtcNow < TimeSpan.FromHours(2))
            throw new InvalidOperationException("Booking cannot be cancelled less than 2 hours before the appointment.");
        Status = BookingStatus.Cancelled;
        CancellationReason = reason;
        AddDomainEvent(new BookingCancelled(Id, CustomerId, reason));
    }

    public void Complete()
    {
        if (Status != BookingStatus.Confirmed)
            throw new InvalidOperationException("Only confirmed bookings can be completed.");
        Status = BookingStatus.Completed;
        LoyaltyPointsEarned = 10;
        AddDomainEvent(new BookingCompleted(Id, CustomerId, LoyaltyPointsEarned));
    }

    public void MarkNoShow()
    {
        if (Status != BookingStatus.Confirmed)
            throw new InvalidOperationException("Only confirmed bookings can be marked as no-show.");
        Status = BookingStatus.NoShow;
    }
}
