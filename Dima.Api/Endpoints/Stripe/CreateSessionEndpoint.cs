using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Requests.Stripe;

namespace Dima.Api.Endpoints.Stripe;

public abstract class CreateSessionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/session", HandleAsync)
            .WithName("Stripe: Criar Sessão")
            .WithSummary("Stripe: Criar sessão para pagamento via stripe")
            .WithDescription("Cria uma sessão de pagamento com Stripe para um usuário autenticado")
            .Produces<string?>(StatusCodes.Status200OK)
            .Produces<string?>(StatusCodes.Status400BadRequest);
    

    private static async Task<IResult> HandleAsync(
        HttpContext context,
        ClaimsPrincipal user,
        IStripeHandler handler,
        CreateSessionRequest request)
    {
        request.UserId = user.Identity?.Name ?? string.Empty;
        
        var result = await handler.CreateSessionAsync(request);

        if (result is { IsSuccess: true, Data: not null })
            context.Response.Headers.Append("Location", result.Data.RedirectUrl);

        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}