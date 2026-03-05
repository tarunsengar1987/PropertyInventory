using PropertyInventory.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace PropertyInventory.API.Controllers;

[Route("api/[controller]")]
public class DashboardController(IPropertyService propertyService) : BaseController
{
    private readonly IPropertyService _propertyService = propertyService;

    [HttpGet("property-sales")]
    public async Task<IActionResult> GetPropertySales(CancellationToken cancellationToken = default)
    {
        var data = await _propertyService.GetDashboardDataAsync(cancellationToken);
        return ResultOk(data);
    }
}
