using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PropertyInventory.Models;
using System.Net;
using System.Text.Json;

namespace PropertyInventory.Utils.Middleware;

/// <summary>
/// Global exception handling middleware for consistent API error responses.
/// </summary>
public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IHostEnvironment env)
{
    private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));
    private readonly ILogger<GlobalExceptionMiddleware> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IHostEnvironment _env = env ?? throw new ArgumentNullException(nameof(env));

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);

        HttpStatusCode statusCode;
        string message;

        if (exception is KeyNotFoundException)
        {
            statusCode = HttpStatusCode.NotFound;
            message = exception.Message;
        }
        else if (exception is ArgumentException or ArgumentNullException)
        {
            statusCode = HttpStatusCode.BadRequest;
            message = exception.Message;
        }
        else if (exception is UnauthorizedAccessException)
        {
            statusCode = HttpStatusCode.Unauthorized;
            message = "Unauthorized.";
        }
        else
        {
            statusCode = HttpStatusCode.InternalServerError;
            message = _env.IsDevelopment() ? exception.Message : "An internal server error occurred.";
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new ServerResponse<object>
        {
            Status = statusCode == HttpStatusCode.InternalServerError ? -1 : 0,
            Message = statusCode == HttpStatusCode.InternalServerError ? "Internal Server Error" : message,
            Error = statusCode == HttpStatusCode.InternalServerError && _env.IsDevelopment() ? exception.ToString() : (statusCode == HttpStatusCode.InternalServerError ? message : null),
            Data = null
        };

        JsonSerializerOptions options = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
    }
}
