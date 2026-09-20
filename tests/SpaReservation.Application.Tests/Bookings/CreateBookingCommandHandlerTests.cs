using FluentAssertions;
using Moq;
using Microsoft.EntityFrameworkCore;
using SpaReservation.Application.Bookings.Commands;
using SpaReservation.Infrastructure.Data;
using SpaReservation.Domain.Bookings;

namespace SpaReservation.Application.Tests.Bookings;

public class CreateBookingCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldAddBookingAndReturnId()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestSpaDb")
            .Options;
            
        using var context = new ApplicationDbContext(options);
        var handler = new CreateBookingCommandHandler(context);
        var command = new CreateBookingCommand(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            TimeOnly.FromTimeSpan(TimeSpan.FromHours(10)),
            TimeOnly.FromTimeSpan(TimeSpan.FromHours(11)),
            "Test"
        );
        
        var id = await handler.Handle(command, CancellationToken.None);
        
        id.Should().NotBeEmpty();
        var booking = await context.Bookings.FindAsync(id);
        booking.Should().NotBeNull();
        booking!.Notes.Should().Be("Test");
        booking.Status.Should().Be(BookingStatus.Pending);
    }
}
