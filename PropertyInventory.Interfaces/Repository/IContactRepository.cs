using PropertyInventory.Models;
using PropertyInventory.Models.Entities;

namespace PropertyInventory.Interfaces.Repository;

/// <summary>
/// Contact-specific repository contract.
/// </summary>
public interface IContactRepository : IGenericRepository<Contact>
{
    Task<PagedResult<Contact>> GetAllAsync(int page, int pageSize, string? filter, CancellationToken cancellationToken = default);
}
