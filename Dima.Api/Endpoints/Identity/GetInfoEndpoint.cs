using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Models.Account;

namespace Dima.Api.Endpoints.Identity;

public class GetInfoEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/manage/info", Handle)
            .WithName("Identity: Get Info")
            .WithSummary("Gets current user info")
            .WithDescription("Returns information about the currently authenticated user")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    private static IResult Handle(ClaimsPrincipal user)
    {
        if (user.Identity is not { IsAuthenticated: true })
            return Results.Unauthorized();

        return Results.Ok(new User
        {
            Email = user.Identity.Name ?? string.Empty,
            IsEmailConfirmed = true, // Simplified for local dev
            Claims = user.Claims.ToDictionary(c => c.Type, c => c.Value)
        });
    }
}