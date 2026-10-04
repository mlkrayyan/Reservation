using Microsoft.EntityFrameworkCore;
using SpaReservation.Domain.Staff;
using SpaReservation.Infrastructure.Data;

namespace SpaReservation.Infrastructure.Repositories;

public class StaffRepository : Repository<StaffMember>, IStaffRepository
{
    public StaffRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<StaffMember>> GetActiveAsync(CancellationToken ct = default)
    {
        return await _context.Staff
            .Include(s => s.WorkingSchedules)
            .Include(s => s.TimeOffs)
            .Where(s => s.IsActive)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<StaffMember>> GetByServiceAsync(Guid serviceId, CancellationToken ct = default)
    {
        return await _context.Staff
            .Include(s => s.WorkingSchedules)
            .Include(s => s.TimeOffs)
            .Where(s => s.IsActive && s.SpecialtyServiceIds.Contains(serviceId))
            .ToListAsync(ct);
    }

    public async Task<(IEnumerable<StaffMember> Items, int TotalCount)> GetPagedActiveAsync(int pageNumber, int pageSize, CancellationToken ct = default)
    {
        var query = _context.Staff
            .Include(s => s.WorkingSchedules)
            .Include(s => s.TimeOffs)
            .Where(s => s.IsActive);

        int totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }
}
