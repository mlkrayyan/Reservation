using SpaReservation.Domain.SharedKernel;
namespace SpaReservation.Domain.Services;
public interface ISalonServiceRepository : IRepository<SalonService>
{
    Task<IEnumerable<SalonService>> GetActiveAsync(CancellationToken ct = default);
    Task<IEnumerable<SalonService>> GetByCategoryAsync(ServiceCategory category, CancellationToken ct = default);
}
