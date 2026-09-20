using SpaReservation.Domain.SharedKernel;
namespace SpaReservation.Domain.Customers;
public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByExternalAuthIdAsync(string externalAuthId, string provider, CancellationToken ct = default);
    Task<IEnumerable<Customer>> GetAllAsync(int page, int pageSize, CancellationToken ct = default);
}
