using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Requests.Account;

namespace Dima.Api.Endpoints.Identity;

public class UpdateProfileEndpoint : IEndpoint
{
    public static RouteHandlerBuilder Map(IEndpointRouteBuilder app)
        => app.MapPut("/me", HandleAsync)
            .WithName("/v1/identity/me")
            .WithDescription("Atualizar o perfil do usuário")
            .Produces(StatusCodes.Status200OK);

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        IProfileHandler handler,
        UpdateProfileRequest request)
    {
        if (user.Identity is null || !user.Identity.IsAuthenticated)
            return Results.Unauthorized();
        
        request.UserId = user.Identity?.Name ?? string.Empty;
        var result = await handler.UpdateProfileAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}