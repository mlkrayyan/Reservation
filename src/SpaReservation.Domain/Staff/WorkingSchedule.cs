using SpaReservation.Domain.SharedKernel;
namespace SpaReservation.Domain.Staff;
public class WorkingSchedule : Entity
{
    private WorkingSchedule() { }
    public Guid StaffMemberId { get; private set; }
    public DayOfWeek DayOfWeek { get; private set; }
    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }
    public bool IsOff { get; private set; }

    public static WorkingSchedule Create(Guid staffId, DayOfWeek day, TimeOnly start, TimeOnly end, bool isOff = false)
        => new() { StaffMemberId = staffId, DayOfWeek = day, StartTime = start, EndTime = end, IsOff = isOff };

    public void Update(TimeOnly start, TimeOnly end, bool isOff) { StartTime = start; EndTime = end; IsOff = isOff; }
}
