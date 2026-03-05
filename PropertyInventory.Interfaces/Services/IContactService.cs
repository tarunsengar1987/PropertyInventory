using PropertyInventory.Models;
using PropertyInventory.Models.Entities;

namespace PropertyInventory.Interfaces.Services;

/// <summary>
/// Contact business logic contract.
/// </summary>
public interface IContactService
{
    Task<PagedResult<Contact>> GetAllAsync(int page, int pageSize, string? filter, CancellationToken cancellationToken = default);
    Task<Contact?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Contact> CreateAsync(Contact contact, CancellationToken cancellationToken = default);
    Task<Contact> UpdateAsync(Guid id, Contact contact, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
