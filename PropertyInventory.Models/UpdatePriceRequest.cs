namespace PropertyInventory.Models;

public class UpdatePriceRequest
{
    public decimal NewPrice { get; set; }
    public string Currency { get; set; } = "EUR";
}
