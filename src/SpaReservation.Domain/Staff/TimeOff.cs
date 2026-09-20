using SpaReservation.Domain.SharedKernel;
namespace SpaReservation.Domain.Staff;
public class TimeOff : Entity
{
    private TimeOff() { }
    public Guid StaffMemberId { get; private set; }
    public DateOnly Date { get; private set; }
    public string? Reason { get; private set; }

    public static TimeOff Create(Guid staffId, DateOnly date, string? reason)
        => new() { StaffMemberId = staffId, Date = date, Reason = reason };
}
