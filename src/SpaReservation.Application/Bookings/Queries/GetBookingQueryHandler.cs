using MediatR;
using SpaReservation.Domain.Bookings;

namespace SpaReservation.Application.Bookings.Queries;

public class GetBookingQueryHandler : IRequestHandler<GetBookingQuery, BookingDto?>
{
    private readonly IBookingRepository _bookingRepository;

    public GetBookingQueryHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<BookingDto?> Handle(GetBookingQuery request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(request.BookingId, cancellationToken);

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
