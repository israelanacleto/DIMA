using Dima.Api;
using Dima.Api.Common.Api;
using Dima.Api.Endpoints;

// PostgreSQL: mantém timestamps sem fuso (como o datetime2 do SQL Server) e
// evita erros ao gravar DateTime com Kind=Unspecified. Deve vir antes de qualquer
// uso do Npgsql.
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// No Railway/Docker a porta vem da variável PORT; localmente (sem PORT) mantém
// o comportamento do launchSettings.
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(port))
    builder.WebHost.UseUrls($"http://+:{port}");

builder.AddConfiguration();

// Security
builder.AddSecurity();
// Dependency Injection
builder.AddInfrastructure();
// Cors
builder.AddCrossOrigin();
// Docs
builder.AddDocs();

var app = builder.Build();

app.UseExceptionHandler();

// Serve o Blazor WASM publicado no wwwroot (serviço único: front + back juntos)
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseCors(ApiConfiguration.CorsPolicyName);
app.UseSecurity();

await app.ConfigureDevEnvironmentAsync();

app.MapEndpoints();

// Qualquer rota não-API cai no index.html do WASM (SPA fallback)
app.MapFallbackToFile("index.html");

app.Run();
