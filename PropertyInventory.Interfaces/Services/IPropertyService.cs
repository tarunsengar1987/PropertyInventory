using PropertyInventory.Models;
using PropertyInventory.Models.Entities;

namespace PropertyInventory.Interfaces.Services;

/// <summary>
/// Property business logic contract (SOLID - Dependency Inversion).
/// </summary>
public interface IPropertyService
{
    Task<PagedResult<Property>> GetAllAsync(int page, int pageSize, string? filter, CancellationToken cancellationToken = default);
    Task<Property?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Property> CreateAsync(Property property, CancellationToken cancellationToken = default);
    Task<Property> UpdateAsync(Guid id, Property property, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task TransferOwnershipAsync(Guid propertyId, Guid contactId, decimal acquisitionPrice, string currency, CancellationToken cancellationToken = default);
    Task UpdatePriceAsync(Guid propertyId, decimal newPrice, string currency, CancellationToken cancellationToken = default);
    Task<IEnumerable<DashboardVM>> GetDashboardDataAsync(CancellationToken cancellationToken = default);
}
