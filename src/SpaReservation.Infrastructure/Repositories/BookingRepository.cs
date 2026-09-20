using Microsoft.EntityFrameworkCore;
using SpaReservation.Domain.Bookings;
using SpaReservation.Infrastructure.Data;

namespace SpaReservation.Infrastructure.Repositories;

public class BookingRepository : Repository<Booking>, IBookingRepository
{
    public BookingRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Booking>> GetByCustomerIdAsync(Guid customerId, CancellationToken ct = default)
    {
        return await _context.Bookings
            .Where(b => b.CustomerId == customerId)
            .OrderByDescending(b => b.Slot.Date)
            .ThenByDescending(b => b.Slot.StartTime)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<Booking>> GetByStaffAndDateAsync(Guid staffId, DateOnly date, CancellationToken ct = default)
    {
        return await _context.Bookings
            .Where(b => b.StaffId == staffId && b.Slot.Date == date && b.Status != BookingStatus.Cancelled)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<Booking>> GetAllAsync(DateTime from, DateTime to, CancellationToken ct = default)
    {
        return await _context.Bookings
            .Where(b => b.CreatedAt >= from && b.CreatedAt <= to)
            .ToListAsync(ct);
    }
}
