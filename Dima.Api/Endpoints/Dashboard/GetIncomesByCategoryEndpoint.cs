using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models.Dashboard;
using Dima.Core.Requests.Dashboard;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Dashboard;

public class GetIncomesByCategoryEndpoint : IEndpoint
{
    public static RouteHandlerBuilder Map(IEndpointRouteBuilder app)
        => app.MapGet("/incomes", HandleAsync)
            .WithName("Dashboard: Get Incomes By Category")
            .WithDescription("Busca as entradas por categoria")
            .WithSummary("Busca as entradas por categoria")
            .WithOrder(4)
            .Produces<Response<List<IncomesByCategory>?>>();
    
    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        IDashboardHandler handler)
    {
        var request = new GetIncomesByCategoryRequest
        {
            UserId = user.Identity?.Name ?? string.Empty
        };
        var result = await handler.GetIncomesByCategoryReportAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}