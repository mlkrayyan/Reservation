using MediatR;

namespace SpaReservation.Application.Bookings.Commands;

public record CancelBookingCommand(Guid BookingId, string Reason) : IRequest;
