using PropertyInventory.Data;
using PropertyInventory.Interfaces.Repository;
using PropertyInventory.Models;
using Microsoft.EntityFrameworkCore;
using PropertyInventory.Models.Entities;

namespace PropertyInventory.Repository;

public class ContactRepository(PropertyInventoryDbContext context) : GenericRepository<Contact>(context), IContactRepository
{
    public async Task<PagedResult<Contact>> GetAllAsync(int page, int pageSize, string? filter, CancellationToken cancellationToken = default)
    {
        var query = Context.Contacts.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter))
        {
            var lower = filter.ToLower();
            query = query.Where(c =>
                c.FirstName.Contains(lower, StringComparison.CurrentCultureIgnoreCase) ||
                c.LastName.Contains(lower, StringComparison.CurrentCultureIgnoreCase) ||
                c.Email.Contains(lower, StringComparison.CurrentCultureIgnoreCase));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(c => c.LastName).ThenBy(c => c.FirstName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Contact>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public override async Task<Contact> AddAsync(Contact contact, CancellationToken cancellationToken = default)
    {
        contact.Id = Guid.NewGuid();
        return await base.AddAsync(contact, cancellationToken);
    }

    public override async Task<Contact> UpdateAsync(Contact contact, CancellationToken cancellationToken = default)
    {
        var existing = await Context.Contacts.FindAsync([contact.Id], cancellationToken)
            ?? throw new KeyNotFoundException($"Contact with ID {contact.Id} was not found.");

        existing.FirstName = contact.FirstName;
        existing.LastName = contact.LastName;
        existing.PhoneNumber = contact.PhoneNumber;
        existing.Email = contact.Email;

        await Context.SaveChangesAsync(cancellationToken);
        return existing;
    }
}
