namespace PropertyInventory.Models;

/// <summary>
/// Standard API response wrapper for consistent response structure.
/// </summary>
public class ServerResponse<T>
{
    public int Status { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Error { get; set; }
    public T? Data { get; set; }
}
