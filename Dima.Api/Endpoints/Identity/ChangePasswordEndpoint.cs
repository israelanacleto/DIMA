using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Requests.Account;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Identity;

public abstract class ChangePasswordEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/change-password", HandleAsync)
            .WithName("v1/identity/change-password")
            .WithDescription("Alterar a senha do usuário")
            .Produces<Response<string?>>();

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        IProfileHandler handler,
        ChangePasswordRequest request)
    {
        if (user.Identity is null || !user.Identity.IsAuthenticated)
            return Results.Unauthorized();
        
        request.UserId = user.Identity.Name ?? string.Empty;
        var result = await handler.ChangePasswordAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}
