using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models.Dashboard;
using Dima.Core.Requests.Dashboard;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Dashboard;

public class GetMostUsedCategoriesEndpoint : IEndpoint
{
    public static RouteHandlerBuilder Map(IEndpointRouteBuilder app)
        => app.MapGet("/most-used", HandleAsync)
            .WithName("Dashboard: Buscar Categorias Mais Utilizadas")
            .WithDescription("Busca as categorias mais utilizadas no mês")
            .WithSummary("Busca as categorias mais utilizadas no mês")
            .WithOrder(5)
            .Produces<Response<List<MostUsedCategory>?>>();
    
    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        IDashboardHandler handler)
    {
        var request = new GetMostUsedCategoriesRequest
        {
            UserId = user.Identity?.Name ?? string.Empty
        };
        var result = await handler.GetMostUsedCategoriesReportAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}
