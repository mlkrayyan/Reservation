using Microsoft.EntityFrameworkCore;
using SpaReservation.Domain.Customers;
using SpaReservation.Infrastructure.Data;

namespace SpaReservation.Infrastructure.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Customer?> GetByExternalAuthIdAsync(string externalAuthId, string provider, CancellationToken ct = default)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.ExternalAuthId == externalAuthId && c.AuthProvider == provider, ct);
    }

    public async Task<IEnumerable<Customer>> GetAllAsync(int page, int pageSize, CancellationToken ct = default)
    {
        return await _context.Customers
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }
}
