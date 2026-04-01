using Dima.Api.Data;
using Dima.Api.Models;

namespace Dima.Api.Handlers;

public interface IAuditHandler
{
    Task LogAsync(string userId, string action, string entityName, string entityId, string? details = null);
}

public class AuditHandler(AppDbContext context) : IAuditHandler
{
    public async Task LogAsync(string userId, string action, string entityName, string entityId, string? details = null)
    {
        var log = new AuditLog
        {
            UserId = userId,
            Action = action,
            EntityName = entityName,
            EntityId = entityId,
            Details = details,
            CreatedAt = DateTime.Now
        };

        try
        {
            await context.AuditLogs.AddAsync(log);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Fail silently or log to console - audit shouldn't break the main flow
            Console.WriteLine($"Audit logging failed: {ex.Message}");
        }
    }
}