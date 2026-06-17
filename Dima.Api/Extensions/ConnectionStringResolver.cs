namespace Dima.Api.Extensions;

/// <summary>
/// Resolve a connection string com a precedência correta para deploy:
/// <list type="number">
/// <item>Env <c>ConnectionStrings__DefaultConnection</c> (override explícito).</item>
/// <item>Env <c>DATABASE_URL</c> — padrão do Railway/Heroku ao plugar o Postgres.</item>
/// <item>O valor vindo da configuração (appsettings, usado em dev).</item>
/// </list>
/// Aceita tanto o formato keyword do Npgsql (Host=...;Port=...) quanto o formato
/// URL (postgres://user:senha@host:porta/db), convertendo este último.
/// Sem isso, o default localhost do appsettings mascararia a DATABASE_URL no
/// Railway e o app tentaria conectar em localhost.
/// </summary>
public static class ConnectionStringResolver
{
    public static string Resolve(string? fromConfig)
    {
        // 1. Override explícito por variável de ambiente.
        var envConn = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
        if (!string.IsNullOrWhiteSpace(envConn))
            return Normalize(envConn);

        // 2. DATABASE_URL (Railway expõe ao referenciar o serviço de Postgres).
        var dbUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
        if (!string.IsNullOrWhiteSpace(dbUrl))
            return Normalize(dbUrl);

        // 3. Configuração (appsettings em desenvolvimento).
        return Normalize(fromConfig ?? string.Empty);
    }

    private static string Normalize(string value)
        => IsUrl(value) ? FromUrl(value) : value;

    private static bool IsUrl(string value)
        => value.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase);

    private static string FromUrl(string url)
    {
        var uri = new Uri(url);
        var userInfo = uri.UserInfo.Split(':', 2);
        var user = Uri.UnescapeDataString(userInfo[0]);
        var password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : string.Empty;
        var database = uri.AbsolutePath.Trim('/');
        var port = uri.Port > 0 ? uri.Port : 5432;

        // SSL Mode=Prefer + Trust Server Certificate funciona tanto na rede
        // privada do Railway (sem SSL) quanto em conexões externas (com SSL).
        return $"Host={uri.Host};Port={port};Database={database};" +
               $"Username={user};Password={password};" +
               "SSL Mode=Prefer;Trust Server Certificate=true";
    }
}
