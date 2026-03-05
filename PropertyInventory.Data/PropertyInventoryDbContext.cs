using Microsoft.EntityFrameworkCore;
using PropertyInventory.Models.Entities;

namespace PropertyInventory.Data
{
    public class PropertyInventoryDbContext : DbContext
    {
        public PropertyInventoryDbContext(DbContextOptions<PropertyInventoryDbContext> options) : base(options)
        {
        }

        public DbSet<Property> Properties { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<PropertyOwnership> PropertyOwnerships { get; set; }
        public DbSet<PropertyPriceHistory> PropertyPriceHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Property>()
                .HasMany(p => p.Ownerships)
                .WithOne(o => o.Property)
                .HasForeignKey(o => o.PropertyId);

            modelBuilder.Entity<Contact>()
                .HasMany(c => c.Ownerships)
                .WithOne(o => o.Contact)
                .HasForeignKey(o => o.ContactId);

            modelBuilder.Entity<Property>()
                .HasMany(p => p.PriceHistories)
                .WithOne(ph => ph.Property)
                .HasForeignKey(ph => ph.PropertyId);
        }
    }
}
