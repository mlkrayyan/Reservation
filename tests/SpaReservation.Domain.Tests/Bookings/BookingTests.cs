using FluentAssertions;
using SpaReservation.Domain.Bookings;
using SpaReservation.Domain.Bookings.Events;

namespace SpaReservation.Domain.Tests.Bookings;

public class BookingTests
{
    private static Booking CreateTestBooking() => Booking.Create(
        customerId: Guid.NewGuid(),
        staffId: Guid.NewGuid(),
        serviceId: Guid.NewGuid(),
        slot: new BookingSlot(DateOnly.FromDateTime(DateTime.Today.AddDays(3)), TimeOnly.FromTimeSpan(TimeSpan.FromHours(10)), TimeOnly.FromTimeSpan(TimeSpan.FromHours(11))),
        notes: "Test booking"
    );

    [Fact]
    public void Create_ShouldSetStatusToPending()
    {
        var booking = CreateTestBooking();
        booking.Status.Should().Be(BookingStatus.Pending);
    }

    [Fact]
    public void Create_ShouldRaiseBookingCreatedEvent()
    {
        var booking = CreateTestBooking();
        booking.DomainEvents.Should().ContainSingle(e => e is BookingCreated);
    }

    [Fact]
    public void Confirm_ShouldSetStatusToConfirmed()
    {
        var booking = CreateTestBooking();
        booking.Confirm();
        booking.Status.Should().Be(BookingStatus.Confirmed);
    }

    [Fact]
    public void Cancel_WhenMoreThan2HoursBefore_ShouldSucceed()
    {
        var booking = Booking.Create(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            new BookingSlot(DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                TimeOnly.FromTimeSpan(TimeSpan.FromHours(14)),
                TimeOnly.FromTimeSpan(TimeSpan.FromHours(15))),
            null);
        booking.Confirm();
        var act = () => booking.Cancel("Customer request");
        act.Should().NotThrow();
        booking.Status.Should().Be(BookingStatus.Cancelled);
    }

    [Fact]
    public void Cancel_WhenLessThan2HoursBefore_ShouldThrow()
    {
        var nearFuture = DateTime.UtcNow.AddMinutes(30);
        var booking = Booking.Create(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            new BookingSlot(DateOnly.FromDateTime(nearFuture.Date),
                TimeOnly.FromTimeSpan(nearFuture.TimeOfDay),
                TimeOnly.FromTimeSpan(nearFuture.TimeOfDay.Add(TimeSpan.FromHours(1)))),
            null);
        booking.Confirm();
        var act = () => booking.Cancel("Too late");
        act.Should().Throw<InvalidOperationException>().WithMessage("*2 hours*");
    }

    [Fact]
    public void Complete_ShouldAwardLoyaltyPoints()
    {
        var booking = CreateTestBooking();
        booking.Confirm();
        booking.Complete();
        booking.LoyaltyPointsEarned.Should().Be(10);
        booking.Status.Should().Be(BookingStatus.Completed);
    }
}
