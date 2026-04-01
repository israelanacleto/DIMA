using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models.Dashboard;
using Dima.Core.Requests.Dashboard;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Dashboard;

public class GetExpensesByCategoryEndpoint : IEndpoint
{
    public static RouteHandlerBuilder Map(IEndpointRouteBuilder app)
        => app.MapGet("/expenses", HandleAsync)
            .WithName("Dashboard: Get Expenses By Category")
            .WithDescription("Busca os gastos por categoria")
            .WithSummary("Busca os gastos por categoria")
            .WithOrder(1)
            .Produces<Response<List<ExpensesByCategory>?>>();
    
    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        IDashboardHandler handler)
    {
        var request = new GetExpensesByCategoryRequest
        {
            UserId = user.Identity?.Name ?? string.Empty
        };
        var result = await handler.GetExpensesByCategoryReportAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}