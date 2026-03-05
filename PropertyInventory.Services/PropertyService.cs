using PropertyInventory.Interfaces.Repository;
using PropertyInventory.Interfaces.Services;
using PropertyInventory.Models;
using PropertyInventory.Models.Entities;

namespace PropertyInventory.Services;

/// <summary>
/// Property business logic (SOLID - Single Responsibility).
/// </summary>
public class PropertyService(IPropertyRepository propertyRepository) : IPropertyService
{
    private readonly IPropertyRepository _propertyRepository = propertyRepository ?? throw new ArgumentNullException(nameof(propertyRepository));

    public Task<PagedResult<Property>> GetAllAsync(int page, int pageSize, string? filter, CancellationToken cancellationToken = default)
        => _propertyRepository.GetAllAsync(page, pageSize, filter, cancellationToken);

    public Task<Property?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _propertyRepository.GetByIdAsync(id, cancellationToken);

    public Task<Property> CreateAsync(Property property, CancellationToken cancellationToken = default)
        => _propertyRepository.AddAsync(property, cancellationToken);

    public async Task<Property> UpdateAsync(Guid id, Property property, CancellationToken cancellationToken = default)
    {
        if (id != property.Id)
            throw new ArgumentException("ID in URL does not match the ID in the request body.", nameof(id));
        return await _propertyRepository.UpdateAsync(property, cancellationToken);
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        => _propertyRepository.DeleteAsync(id, cancellationToken);

    public Task TransferOwnershipAsync(Guid propertyId, Guid contactId, decimal acquisitionPrice, string currency, CancellationToken cancellationToken = default)
        => _propertyRepository.TransferOwnershipAsync(propertyId, contactId, acquisitionPrice, currency, cancellationToken);

    public Task UpdatePriceAsync(Guid propertyId, decimal newPrice, string currency, CancellationToken cancellationToken = default)
        => _propertyRepository.UpdatePriceAsync(propertyId, newPrice, currency, cancellationToken);

    public Task<IEnumerable<DashboardVM>> GetDashboardDataAsync(CancellationToken cancellationToken = default)
        => _propertyRepository.GetDashboardDataAsync(cancellationToken);
}
