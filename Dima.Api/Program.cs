using System.Security.Claims;
using Dima.Api.Endpoints;
using Dima.Api.Extensions;
using Dima.Api.Models;
using Microsoft.AspNetCore.Identity;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi(options => options.AddScalarTransformers());

builder.Services
    .AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies();
builder.Services.AddAuthorization();

// Dependency Injection
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.AddScalarConfig();
}

app.MapGet("/", () => new { Message = "Ok" });
app.MapEndpoints();
app.MapGroup("/v1/identity")
    .WithTags("Identity")
    .MapIdentityApi<User>();

app.MapGroup("/v1/identity")
    .WithTags("Identity")
    .MapPost("/logout", async (
        SignInManager<User> signInManager) =>
    {
        await signInManager.SignOutAsync();
        return Results.Ok();
    })
    .RequireAuthorization();

app.MapGroup("/v1/identity")
    .WithTags("Identity")
    .MapPost("/roles", (
        ClaimsPrincipal user) =>
    {
        if (user.Identity is null || !user.Identity.IsAuthenticated)
            return Results.Unauthorized();

        var identity = (ClaimsIdentity)user.Identity;
        var roles = identity.FindAll(identity.RoleClaimType)
            .Select(c => new
            {
                c.Issuer,
                c.OriginalIssuer,
                c.Type,
                c.Value,
                c.ValueType
            });
        
        return TypedResults.Json(roles);
    })
    .RequireAuthorization();

app.Run();
