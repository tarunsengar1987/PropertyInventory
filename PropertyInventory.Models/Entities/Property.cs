using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropertyInventory.Models.Entities
{
    public class Property : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string Address { get; set; } = string.Empty;

        public DateTime DateOfRegistration { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentPrice { get; set; }

        public virtual ICollection<PropertyOwnership> Ownerships { get; set; } = [];

        public virtual ICollection<PropertyPriceHistory> PriceHistories { get; set; } = [];
    }
}
