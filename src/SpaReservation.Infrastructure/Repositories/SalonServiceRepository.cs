using Microsoft.EntityFrameworkCore;
using SpaReservation.Domain.Services;
using SpaReservation.Infrastructure.Data;

namespace SpaReservation.Infrastructure.Repositories;

public class SalonServiceRepository : Repository<SalonService>, ISalonServiceRepository
{
    public SalonServiceRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<SalonService>> GetActiveAsync(CancellationToken ct = default)
    {
        return await _context.Services.Where(s => s.IsActive).ToListAsync(ct);
    }

    public async Task<IEnumerable<SalonService>> GetByCategoryAsync(ServiceCategory category, CancellationToken ct = default)
    {
        return await _context.Services.Where(s => s.Category == category && s.IsActive).ToListAsync(ct);
    }
}
