using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Requests.Common;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Categories;

public class GetAllCombosCategoryEndpoint : IEndpoint
{
    public static RouteHandlerBuilder Map(IEndpointRouteBuilder app)
        => app.MapGet("/all", HandleAsync)
            .WithName("Categories: Get All Combos")
        .WithSummary("Recupera todas as categorias para combo select")
        .WithDescription("Recupera todas as categorias para combo select")
        .WithOrder(5)
        .Produces<Response<List<ComboItens>>>(StatusCodes.Status200OK);

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        ICategoryHandler handler)
    {
        var request = new GetCombosRequest()
        {
            UserId = user.Identity?.Name ?? string.Empty
        };

        var result = await handler.GetAllComboSelectAsync(request);
        return result.IsSuccess
            ? Results.Ok(result)
            : Results.BadRequest(result.Data);
    }
}