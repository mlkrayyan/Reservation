using SpaReservation.Domain.SharedKernel;
namespace SpaReservation.Domain.Bookings.Events;
public record BookingConfirmed(Guid BookingId, Guid CustomerId) : IDomainEvent;
