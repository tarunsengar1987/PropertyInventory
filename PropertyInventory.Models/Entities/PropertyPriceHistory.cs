using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropertyInventory.Models.Entities
{
    public class PropertyPriceHistory
    {
        [Key]
        public Guid Id { get; set; }

        public Guid PropertyId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(10)]
        public string Currency { get; set; } = "EUR";

        public DateTime ChangedOn { get; set; }
        public Property Property { get; set; } = null!;
    }
}
