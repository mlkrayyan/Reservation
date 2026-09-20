using MediatR;
using SpaReservation.Domain.Bookings;

namespace SpaReservation.Application.Bookings.Queries;

public record GetBookingQuery(Guid BookingId) : IRequest<BookingDto?>;

public record BookingDto(
    Guid Id,
    Guid CustomerId,
    Guid StaffId,
    Guid ServiceId,
    DateOnly Date,
    TimeOnly StartTime,
    TimeOnly EndTime,
    string Status,
    string? Notes
);
