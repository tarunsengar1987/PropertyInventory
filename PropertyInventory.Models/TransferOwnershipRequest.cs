namespace PropertyInventory.Models;

public class TransferOwnershipRequest
{
    public Guid ContactId { get; set; }
    public decimal AcquisitionPrice { get; set; }
    public string Currency { get; set; } = "EUR";
}
