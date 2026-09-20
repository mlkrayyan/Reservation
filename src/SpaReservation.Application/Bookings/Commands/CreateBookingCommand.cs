using MediatR;
using SpaReservation.Domain.Bookings;

namespace SpaReservation.Application.Bookings.Commands;

public record CreateBookingCommand(
    Guid CustomerId,
    Guid StaffId,
    Guid ServiceId,
    DateOnly Date,
    TimeOnly StartTime,
    TimeOnly EndTime,
    string? Notes
) : IRequest<Guid>;
