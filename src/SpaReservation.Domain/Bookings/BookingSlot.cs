namespace SpaReservation.Domain.Bookings;
public record BookingSlot(DateOnly Date, TimeOnly StartTime, TimeOnly EndTime)
{
    public DateTime StartDateTime => Date.ToDateTime(StartTime);
    public DateTime EndDateTime => Date.ToDateTime(EndTime);
    public bool OverlapsWith(BookingSlot other) =>
        StartDateTime < other.EndDateTime && EndDateTime > other.StartDateTime;
}
