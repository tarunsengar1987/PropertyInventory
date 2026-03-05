using PropertyInventory.Models;
using PropertyInventory.Models.Entities;

namespace PropertyInventory.Interfaces.Repository;

/// <summary>
/// Property-specific repository contract (extends generic data access with domain operations).
/// </summary>
public interface IPropertyRepository : IGenericRepository<Property>
{
    Task<PagedResult<Property>> GetAllAsync(int page, int pageSize, string? filter, CancellationToken cancellationToken = default);
    Task TransferOwnershipAsync(Guid propertyId, Guid contactId, decimal acquisitionPrice, string currency, CancellationToken cancellationToken = default);
    Task UpdatePriceAsync(Guid propertyId, decimal newPrice, string currency, CancellationToken cancellationToken = default);
    Task<IEnumerable<DashboardVM>> GetDashboardDataAsync(CancellationToken cancellationToken = default);
}
