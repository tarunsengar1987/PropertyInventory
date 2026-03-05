using Microsoft.AspNetCore.Mvc;
using PropertyInventory.Interfaces.Services;
using PropertyInventory.Models;
using PropertyInventory.Models.Entities;

namespace PropertyInventory.API.Controllers;

[Route("api/[controller]")]
public class PropertyController(IPropertyService propertyService) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? filter = null,
        CancellationToken cancellationToken = default)
    {
        var result = await propertyService.GetAllAsync(page, pageSize, filter, cancellationToken);
        return ResultOk(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var property = await propertyService.GetByIdAsync(id, cancellationToken);
        if (property == null)
            return ResultNotFound($"Property with ID {id} was not found.");
        return ResultOk(property);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Property property, CancellationToken cancellationToken = default)
    {
        var created = await propertyService.CreateAsync(property, cancellationToken);
        return ResultCreated(created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Property property, CancellationToken cancellationToken = default)
    {
        var updated = await propertyService.UpdateAsync(id, property, cancellationToken);
        return ResultUpdated(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        await propertyService.DeleteAsync(id, cancellationToken);
        return ResultDeleted("Property deleted successfully.");
    }

    [HttpPost("{id:guid}/transfer")]
    public async Task<IActionResult> Transfer(Guid id, [FromBody] TransferOwnershipRequest request, CancellationToken cancellationToken = default)
    {
        await propertyService.TransferOwnershipAsync(id, request.ContactId, request.AcquisitionPrice, request.Currency, cancellationToken);
        return ResultUpdated(null, "Ownership transferred successfully.");
    }

    [HttpPost("{id:guid}/update-price")]
    public async Task<IActionResult> UpdatePrice(Guid id, [FromBody] UpdatePriceRequest request, CancellationToken cancellationToken = default)
    {
        await propertyService.UpdatePriceAsync(id, request.NewPrice, request.Currency, cancellationToken);
        return ResultUpdated(null, "Price updated successfully.");
    }
}
