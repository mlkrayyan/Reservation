using SpaReservation.Domain.SharedKernel;
namespace SpaReservation.Domain.Bookings.Events;
public record BookingCompleted(Guid BookingId, Guid CustomerId, int LoyaltyPointsEarned) : IDomainEvent;
