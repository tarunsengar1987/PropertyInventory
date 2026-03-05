using PropertyInventory.Models;
using Microsoft.AspNetCore.Mvc;

namespace PropertyInventory.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public ObjectResult ResultOk(object? data = null, string message = "Success")
    {
        return StatusCode(StatusCodes.Status200OK, new ServerResponse<object>
        {
            Status = 1,
            Message = message,
            Data = data
        });
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public ObjectResult ResultCreated(object? data = null, string message = "Created successfully")
    {
        return StatusCode(StatusCodes.Status200OK, new ServerResponse<object>
        {
            Status = 1,
            Message = message,
            Data = data
        });
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public ObjectResult ResultUpdated(object? data = null, string message = "Updated successfully")
    {
        return StatusCode(StatusCodes.Status200OK, new ServerResponse<object>
        {
            Status = 1,
            Message = message,
            Data = data
        });
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public ObjectResult ResultDeleted(string message = "Deleted successfully")
    {
        return StatusCode(StatusCodes.Status200OK, new ServerResponse<object>
        {
            Status = 1,
            Message = message
        });
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public ObjectResult ResultBadRequest(string message = "Bad Request")
    {
        return StatusCode(StatusCodes.Status400BadRequest, new ServerResponse<object>
        {
            Status = 0,
            Message = message
        });
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public ObjectResult ResultNotFound(string message = "Not Found")
    {
        return StatusCode(StatusCodes.Status404NotFound, new ServerResponse<object>
        {
            Status = 0,
            Message = message
        });
    }
}
