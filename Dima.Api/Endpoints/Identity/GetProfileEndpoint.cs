using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Requests.Account;

namespace Dima.Api.Endpoints.Identity;

public abstract class GetProfileEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/me", HandleAsync)
            .WithName("v1/identity/me")
            .Produces(StatusCodes.Status200OK);

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        IProfileHandler handler)
    {
        if (user.Identity is null || !user.Identity.IsAuthenticated)
            return Results.Unauthorized();

        var request = new GetProfileRequest
        {
            UserId = user.Identity.Name ?? string.Empty
        };
        var result = await handler.GetProfileAsync(request);
        return result.IsSuccess
            ? Results.Ok(result)
            : Results.BadRequest(result.Data);
    }
}