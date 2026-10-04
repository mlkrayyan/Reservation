using SpaReservation.Domain.SharedKernel;

namespace SpaReservation.Domain.Staff;

public interface IStaffRepository : IRepository<StaffMember>
{
    Task<IEnumerable<StaffMember>> GetActiveAsync(CancellationToken ct = default);
    Task<IEnumerable<StaffMember>> GetByServiceAsync(Guid serviceId, CancellationToken ct = default);
    Task<(IEnumerable<StaffMember> Items, int TotalCount)> GetPagedActiveAsync(int pageNumber, int pageSize, CancellationToken ct = default);
}
