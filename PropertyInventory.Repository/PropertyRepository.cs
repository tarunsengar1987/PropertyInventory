using Microsoft.EntityFrameworkCore;
using PropertyInventory.Data;
using PropertyInventory.Interfaces.Repository;
using PropertyInventory.Models;
using PropertyInventory.Models.Entities;

namespace PropertyInventory.Repository;

public class PropertyRepository : GenericRepository<Property>, IPropertyRepository
{
    private readonly decimal _eurToUsdRate;

    public PropertyRepository(PropertyInventoryDbContext context, Microsoft.Extensions.Configuration.IConfiguration configuration) : base(context)
    {
        var rateStr = configuration["CurrencySettings:EurToUsdRate"];
        _eurToUsdRate = decimal.TryParse(rateStr, System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture, out var parsed) ? parsed : 1.09m;
    }

    public async Task<PagedResult<Property>> GetAllAsync(int page, int pageSize, string? filter, CancellationToken cancellationToken = default)
    {
        var query = Context.Properties
            .Include(p => p.Ownerships).ThenInclude(o => o.Contact)
            .Include(p => p.PriceHistories)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter))
        {
            var lower = filter.ToLower();
            query = query.Where(p =>
                p.Name.Contains(lower, StringComparison.CurrentCultureIgnoreCase) ||
                p.Address.Contains(lower, StringComparison.CurrentCultureIgnoreCase));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Property>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public override async Task<Property?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Context.Properties
            .Include(p => p.Ownerships).ThenInclude(o => o.Contact)
            .Include(p => p.PriceHistories)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public override async Task<Property> AddAsync(Property property, CancellationToken cancellationToken = default)
    {
        property.Id = Guid.NewGuid();
        property.DateOfRegistration = DateTime.UtcNow;

        Context.Properties.Add(property);
        Context.PropertyPriceHistories.Add(new PropertyPriceHistory
        {
            Id = Guid.NewGuid(),
            PropertyId = property.Id,
            Price = property.CurrentPrice,
            Currency = "EUR",
            ChangedOn = DateTime.UtcNow
        });

        await Context.SaveChangesAsync(cancellationToken);
        return property;
    }

    public override async Task<Property> UpdateAsync(Property property, CancellationToken cancellationToken = default)
    {
        var existing = await Context.Properties.FindAsync([property.Id], cancellationToken)
            ?? throw new KeyNotFoundException($"Property with ID {property.Id} was not found.");

        existing.Name = property.Name;
        existing.Address = property.Address;
        existing.CurrentPrice = property.CurrentPrice;
        existing.DateOfRegistration = property.DateOfRegistration;

        await Context.SaveChangesAsync(cancellationToken);
        return existing;
    }

    public override async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var property = await Context.Properties
            .Include(p => p.Ownerships)
            .Include(p => p.PriceHistories)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"Property with ID {id} was not found.");

        Context.PropertyOwnerships.RemoveRange(property.Ownerships);
        Context.PropertyPriceHistories.RemoveRange(property.PriceHistories);
        Context.Properties.Remove(property);
        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task TransferOwnershipAsync(Guid propertyId, Guid contactId, decimal acquisitionPrice, string currency, CancellationToken cancellationToken = default)
    {
        var propertyExists = await Context.Properties.AnyAsync(p => p.Id == propertyId, cancellationToken);
        if (!propertyExists)
            throw new KeyNotFoundException($"Property with ID {propertyId} was not found.");

        var contactExists = await Context.Contacts.AnyAsync(c => c.Id == contactId, cancellationToken);
        if (!contactExists)
            throw new KeyNotFoundException($"Contact with ID {contactId} was not found.");

        var current = await Context.PropertyOwnerships
            .FirstOrDefaultAsync(o => o.PropertyId == propertyId && o.EffectiveTill == null, cancellationToken);

        if (current != null)
            current.EffectiveTill = DateTime.UtcNow;

        Context.PropertyOwnerships.Add(new PropertyOwnership
        {
            Id = Guid.NewGuid(),
            PropertyId = propertyId,
            ContactId = contactId,
            EffectiveFrom = DateTime.UtcNow,
            AcquisitionPrice = acquisitionPrice,
            Currency = currency
        });

        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdatePriceAsync(Guid propertyId, decimal newPrice, string currency, CancellationToken cancellationToken = default)
    {
        var property = await Context.Properties.FindAsync([propertyId], cancellationToken)
            ?? throw new KeyNotFoundException($"Property with ID {propertyId} was not found.");

        property.CurrentPrice = newPrice;
        Context.PropertyPriceHistories.Add(new PropertyPriceHistory
        {
            Id = Guid.NewGuid(),
            PropertyId = propertyId,
            Price = newPrice,
            Currency = currency,
            ChangedOn = DateTime.UtcNow
        });

        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<DashboardVM>> GetDashboardDataAsync(CancellationToken cancellationToken = default)
    {
        return await Context.PropertyOwnerships
            .Include(o => o.Property)
            .Include(o => o.Contact)
            .OrderByDescending(o => o.EffectiveFrom)
            .Select(o => new DashboardVM
            {
                PropertyId = o.PropertyId,
                PropertyName = o.Property.Name,
                AskingPrice = o.Property.CurrentPrice,
                Owner = o.Contact.FirstName + " " + o.Contact.LastName,
                DateOfPurchase = o.EffectiveFrom,
                SoldAtEUR = o.AcquisitionPrice,
                SoldAtUSD = Math.Round(o.AcquisitionPrice * _eurToUsdRate, 2)
            })
            .ToListAsync(cancellationToken);
    }
}
