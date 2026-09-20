using MediatR;
using SpaReservation.Infrastructure.Data;

namespace SpaReservation.Application.Bookings.Commands;

public class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand>
{
    private readonly ApplicationDbContext _context;

    public CancelBookingCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(CancelBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await _context.Bookings.FindAsync(new object[] { request.BookingId }, cancellationToken);
        if (booking == null) throw new Exception("Booking not found.");
        
        booking.Cancel(request.Reason);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
