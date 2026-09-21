namespace SpaReservation.Application.BackgroundJobs;

public interface IBookingReminderJob
{
    Task Send24hRemindersAsync();
    Task Send1hRemindersAsync();
    Task ExpireWaitlistEntriesAsync();
}
