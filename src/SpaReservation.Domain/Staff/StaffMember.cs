using SpaReservation.Domain.SharedKernel;
namespace SpaReservation.Domain.Staff;
public class StaffMember : AggregateRoot
{
    private StaffMember() { }
    public string FullName { get; private set; } = string.Empty;
    public string? Bio { get; private set; }
    public string? PhotoUrl { get; private set; }
    public bool IsActive { get; private set; } = true;
    private readonly List<Guid> _specialtyServiceIds = new();
    public IReadOnlyList<Guid> SpecialtyServiceIds => _specialtyServiceIds.AsReadOnly();
    private readonly List<WorkingSchedule> _workingSchedules = new();
    public IReadOnlyList<WorkingSchedule> WorkingSchedules => _workingSchedules.AsReadOnly();
    private readonly List<TimeOff> _timeOffs = new();
    public IReadOnlyList<TimeOff> TimeOffs => _timeOffs.AsReadOnly();

    public static StaffMember Create(string fullName, string? bio, string? photoUrl)
        => new() { FullName = fullName, Bio = bio, PhotoUrl = photoUrl };

    public void Update(string fullName, string? bio, string? photoUrl)
    { FullName = fullName; Bio = bio; PhotoUrl = photoUrl; }

    public void AddWorkingSchedule(WorkingSchedule schedule) => _workingSchedules.Add(schedule);
    public void ClearWorkingSchedules() => _workingSchedules.Clear();
    public void AddTimeOff(TimeOff timeOff) => _timeOffs.Add(timeOff);
    public void AddSpecialty(Guid serviceId) { if (!_specialtyServiceIds.Contains(serviceId)) _specialtyServiceIds.Add(serviceId); }
    public void Deactivate() => IsActive = false;

    public bool IsAvailableOn(DateOnly date, TimeOnly startTime, TimeOnly endTime)
    {
        if (_timeOffs.Any(t => t.Date == date)) return false;
        var schedule = _workingSchedules.FirstOrDefault(s => s.DayOfWeek == date.DayOfWeek);
        if (schedule == null || schedule.IsOff) return false;
        return startTime >= schedule.StartTime && endTime <= schedule.EndTime;
    }
}
