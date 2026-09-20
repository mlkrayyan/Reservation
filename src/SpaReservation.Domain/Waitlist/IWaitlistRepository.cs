using SpaReservation.Domain.SharedKernel;
namespace SpaReservation.Domain.Waitlist;
public interface IWaitlistRepository : IRepository<WaitlistEntry>
{
    Task<IEnumerable<WaitlistEntry>> GetByCustomerIdAsync(Guid customerId, CancellationToken ct = default);
    Task<IEnumerable<WaitlistEntry>> GetWaitingByServiceAsync(Guid serviceId, CancellationToken ct = default);
    Task<IEnumerable<WaitlistEntry>> GetExpiredOffersAsync(CancellationToken ct = default);
}
