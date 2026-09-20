using MediatR;

namespace SpaReservation.Application.Bookings.Commands;

public record ConfirmBookingCommand(Guid BookingId) : IRequest;
