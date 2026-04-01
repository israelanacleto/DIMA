namespace Dima.Api.Models;

public class AuditLog
{
    public long Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty; // Create, Update, Delete
    public string EntityName { get; set; } = string.Empty; // Order, Category, etc.
    public string EntityId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string? Details { get; set; } // JSON or description
}