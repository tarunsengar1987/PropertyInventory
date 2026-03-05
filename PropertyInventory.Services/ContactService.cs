using PropertyInventory.Interfaces.Repository;
using PropertyInventory.Interfaces.Services;
using PropertyInventory.Models;
using PropertyInventory.Models.Entities;

namespace PropertyInventory.Services;

/// <summary>
/// Contact business logic (SOLID - Single Responsibility).
/// </summary>
public class ContactService(IContactRepository contactRepository) : IContactService
{
    private readonly IContactRepository _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));

    public Task<PagedResult<Contact>> GetAllAsync(int page, int pageSize, string? filter, CancellationToken cancellationToken = default)
        => _contactRepository.GetAllAsync(page, pageSize, filter, cancellationToken);

    public Task<Contact?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _contactRepository.GetByIdAsync(id, cancellationToken);

    public Task<Contact> CreateAsync(Contact contact, CancellationToken cancellationToken = default)
        => _contactRepository.AddAsync(contact, cancellationToken);

    public async Task<Contact> UpdateAsync(Guid id, Contact contact, CancellationToken cancellationToken = default)
    {
        if (id != contact.Id)
            throw new ArgumentException("ID in URL does not match the ID in the request body.", nameof(id));
        return await _contactRepository.UpdateAsync(contact, cancellationToken);
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        => _contactRepository.DeleteAsync(id, cancellationToken);
}
