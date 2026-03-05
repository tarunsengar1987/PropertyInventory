using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropertyInventory.Models.Entities
{
    public class PropertyOwnership
    {
        [Key]
        public Guid Id { get; set; }

        public Guid PropertyId { get; set; }

        public Guid ContactId { get; set; }

        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTill { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AcquisitionPrice { get; set; }

        [Required]
        [MaxLength(10)]
        public string Currency { get; set; } = "EUR";

        public Property Property { get; set; } = null!;

        public Contact Contact { get; set; } = null!;
    }
}
