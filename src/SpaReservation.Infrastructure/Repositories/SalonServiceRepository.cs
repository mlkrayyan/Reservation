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

    public async Task<(IEnumerable<SalonService> Items, int TotalCount)> GetPagedActiveAsync(int pageNumber, int pageSize, ServiceCategory? category = null, CancellationToken ct = default)
    {
        var query = _context.Services.Where(s => s.IsActive);
        if (category.HasValue)
        {
            query = query.Where(s => s.Category == category.Value);
        }

        int totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }
}
