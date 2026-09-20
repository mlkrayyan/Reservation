using SpaReservation.Domain.SharedKernel;
namespace SpaReservation.Domain.Bookings;
public interface IBookingRepository : IRepository<Booking>
{
    Task<IEnumerable<Booking>> GetByCustomerIdAsync(Guid customerId, CancellationToken ct = default);
    Task<IEnumerable<Booking>> GetByStaffAndDateAsync(Guid staffId, DateOnly date, CancellationToken ct = default);
    Task<IEnumerable<Booking>> GetAllAsync(DateTime from, DateTime to, CancellationToken ct = default);
}
