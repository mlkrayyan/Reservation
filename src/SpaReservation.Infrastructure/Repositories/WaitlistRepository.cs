using Microsoft.EntityFrameworkCore;
using SpaReservation.Domain.Waitlist;
using SpaReservation.Infrastructure.Data;

namespace SpaReservation.Infrastructure.Repositories;

public class WaitlistRepository : Repository<WaitlistEntry>, IWaitlistRepository
{
    public WaitlistRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<WaitlistEntry>> GetByCustomerIdAsync(Guid customerId, CancellationToken ct = default)
    {
        return await _context.Waitlist
            .Where(w => w.CustomerId == customerId)
            .OrderByDescending(w => w.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<WaitlistEntry>> GetWaitingByServiceAsync(Guid serviceId, CancellationToken ct = default)
    {
        return await _context.Waitlist
            .Where(w => w.ServiceId == serviceId && w.Status == WaitlistStatus.Waiting)
            .OrderBy(w => w.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<WaitlistEntry>> GetExpiredOffersAsync(CancellationToken ct = default)
    {
        var cutoff = DateTime.UtcNow.AddMinutes(-30);
        return await _context.Waitlist
            .Where(w => w.Status == WaitlistStatus.Offered && w.OfferedAt <= cutoff)
            .ToListAsync(ct);
    }
}
