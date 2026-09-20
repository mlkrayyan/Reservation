using FluentAssertions;
using Moq;
using SpaReservation.Application.Bookings.Commands;
using SpaReservation.Domain.Bookings;
using SpaReservation.Domain.SharedKernel;

namespace SpaReservation.Application.Tests.Bookings;

public class CreateBookingCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldAddBookingAndReturnId()
    {
        var bookingRepositoryMock = new Mock<IBookingRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
            
        var handler = new CreateBookingCommandHandler(bookingRepositoryMock.Object, unitOfWorkMock.Object);
        var command = new CreateBookingCommand(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            TimeOnly.FromTimeSpan(TimeSpan.FromHours(10)),
            TimeOnly.FromTimeSpan(TimeSpan.FromHours(11)),
            "Test"
        );
        
        var id = await handler.Handle(command, CancellationToken.None);
        
        id.Should().NotBeEmpty();
        bookingRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
