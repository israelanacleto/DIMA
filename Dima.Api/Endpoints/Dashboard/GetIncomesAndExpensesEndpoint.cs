using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models.Dashboard;
using Dima.Core.Requests.Dashboard;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Dashboard;

public class GetIncomesAndExpensesEndpoint : IEndpoint
{
    public static RouteHandlerBuilder Map(IEndpointRouteBuilder app)
        => app.MapGet("/incomes-expenses", HandleAsync)
            .WithName("Dashboard: Get Incomes and Expenses")
            .WithDescription("Busca as entradas e saídas")
            .WithSummary("Busca as entradas e saídas")
            .WithOrder(3)
            .Produces<Response<List<IncomesAndExpenses>?>>();
    
    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        IDashboardHandler handler)
    {
        var request = new GetIncomesAndExpensesRequest
        {
            UserId = user.Identity?.Name ?? string.Empty
        };
        var result = await handler.GetIncomesAndExpensesReportAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}