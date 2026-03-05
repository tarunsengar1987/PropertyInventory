namespace PropertyInventory.Models.Entities;

/// <summary>
/// Marker interface for entities with a Guid primary key.
/// </summary>
public interface IEntity
{
    Guid Id { get; set; }
}
