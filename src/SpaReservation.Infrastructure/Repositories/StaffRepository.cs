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
}
