using SpaReservation.Domain.SharedKernel;
namespace SpaReservation.Domain.Bookings.Events;
public record BookingCreated(Guid BookingId, Guid CustomerId, Guid StaffId) : IDomainEvent;
