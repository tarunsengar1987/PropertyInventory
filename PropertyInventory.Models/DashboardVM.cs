namespace PropertyInventory.Models;

public class DashboardVM
{
    public Guid PropertyId { get; set; }
    public string PropertyName { get; set; } = string.Empty;
    public decimal AskingPrice { get; set; }
    public string Owner { get; set; } = string.Empty;
    public DateTime DateOfPurchase { get; set; }
    public decimal SoldAtEUR { get; set; }
    public decimal SoldAtUSD { get; set; }
}
