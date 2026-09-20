using MediatR;
using Microsoft.EntityFrameworkCore;
using SpaReservation.Infrastructure.Data;

namespace SpaReservation.Application.Bookings.Queries;

public class GetBookingQueryHandler : IRequestHandler<GetBookingQuery, BookingDto?>
{
    private readonly ApplicationDbContext _context;

    public GetBookingQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BookingDto?> Handle(GetBookingQuery request, CancellationToken cancellationToken)
    {
        var booking = await _context.Bookings
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == request.BookingId, cancellationToken);

        if (booking == null) return null;

        return new BookingDto(
            booking.Id,
            booking.CustomerId,
            booking.StaffId,
            booking.ServiceId,
            booking.Slot.Date,
            booking.Slot.StartTime,
            booking.Slot.EndTime,
            booking.Status.ToString(),
            booking.Notes
        );
    }
}
