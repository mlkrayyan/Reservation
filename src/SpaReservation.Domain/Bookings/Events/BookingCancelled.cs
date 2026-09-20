using SpaReservation.Domain.SharedKernel;
namespace SpaReservation.Domain.Bookings.Events;
public record BookingCancelled(Guid BookingId, Guid CustomerId, string Reason) : IDomainEvent;
