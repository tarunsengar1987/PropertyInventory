using Microsoft.AspNetCore.Mvc;
using PropertyInventory.Interfaces.Services;
using PropertyInventory.Models.Entities;

namespace PropertyInventory.API.Controllers;

[Route("api/[controller]")]
public class ContactController(IContactService contactService) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? filter = null,
        CancellationToken cancellationToken = default)
    {
        var result = await contactService.GetAllAsync(page, pageSize, filter, cancellationToken);
        return ResultOk(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var contact = await contactService.GetByIdAsync(id, cancellationToken);
        if (contact == null)
            return ResultNotFound($"Contact with ID {id} was not found.");
        return ResultOk(contact);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Contact contact, CancellationToken cancellationToken = default)
    {
        var created = await contactService.CreateAsync(contact, cancellationToken);
        return ResultCreated(created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Contact contact, CancellationToken cancellationToken = default)
    {
        var updated = await contactService.UpdateAsync(id, contact, cancellationToken);
        return ResultUpdated(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        await contactService.DeleteAsync(id, cancellationToken);
        return ResultDeleted("Contact deleted successfully.");
    }
}
