using Dima.Api.Common.Api;
using Dima.Api.Endpoints.Categories;
using Dima.Api.Endpoints.Dashboard;
using Dima.Api.Endpoints.Identity;
using Dima.Api.Endpoints.Orders;
using Dima.Api.Endpoints.Stripe;
using Dima.Api.Endpoints.Transactions;
using Dima.Api.Models;
using Scalar.AspNetCore;

namespace Dima.Api.Endpoints;

public static class Endpoint
{
    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = app
            .MapGroup("")
            .ProducesProblem(StatusCodes.Status500InternalServerError);
        
        endpoints.MapGroup("/")
            .WithTags("Health Check")
            .MapGet("/", () => new { Message = "Ok" });
        
        // Identity Endpoints
        var identity = endpoints.MapGroup("/v1/identity")
            .WithTags("Identity");

        identity.MapEndpoint<RegisterEndpoint>();
        identity.MapEndpoint<LoginEndpoint>();
        identity.MapEndpoint<GetInfoEndpoint>();
        identity.MapEndpoint<LogoutEndpoint>()
            .RequireAuthorization();
        identity.MapEndpoint<GetRolesEndpoint>()
            .RequireAuthorization();
        identity.MapEndpoint<GetProfileEndpoint>()
            .RequireAuthorization();
        identity.MapEndpoint<UpdateProfileEndpoint>()
            .RequireAuthorization();
        identity.MapEndpoint<ChangePasswordEndpoint>()
            .RequireAuthorization();

        endpoints.MapGroup("/v1/categories")
            .WithTags("Categories")
            .RequireAuthorization()
            .WithBadge("v1", BadgePosition.After)
            .MapEndpoint<GetAllCombosCategoryEndpoint>()
            .MapEndpoint<CreateCategoryEndpoint>()
            .MapEndpoint<UpdateCategoryEndpoint>()
            .MapEndpoint<DeleteCategoryEndpoint>()
            .MapEndpoint<GetCategoryByIdEndpoint>()
            .MapEndpoint<GetAllCategoriesEndpoint>();
        
        endpoints.MapGroup("/v1/transactions")
            .WithTags("Transactions")
            .RequireAuthorization()
            .WithBadge("v1", BadgePosition.After)
            .MapEndpoint<CreateTransactionEndpoint>()
            .MapEndpoint<UpdateTransactionEndpoint>()
            .MapEndpoint<DeleteTransactionEndpoint>()
            .MapEndpoint<GetTransactionByIdEndpoint>()
            .MapEndpoint<GetTransactionsByPeriodEndpoint>();
        
        endpoints.MapGroup("/v1/dashboard")
            .WithTags("Dashboard")
            .RequireAuthorization()
            .WithBadge("v1", BadgePosition.After)
            .MapEndpoint<GetExpensesByCategoryEndpoint>()
            .MapEndpoint<GetFinancialSummaryEndpoint>()
            .MapEndpoint<GetIncomesAndExpensesEndpoint>()
            .MapEndpoint<GetIncomesByCategoryEndpoint>()
            .MapEndpoint<GetMostUsedCategoriesEndpoint>();
        
        endpoints.MapGroup("/v1/products")
            .WithTags("Products")
            .RequireAuthorization()
            .WithBadge("v1", BadgePosition.After)
            .MapEndpoint<GetAllProductsEndpoint>()
            .MapEndpoint<GetProductBySlugEndpoint>();

        endpoints.MapGroup("/v1/vouchers")
            .WithTags("Vouchers")
            .RequireAuthorization()
            .WithBadge("v1", BadgePosition.After)
            .MapEndpoint<GetVoucherByNumberEndpoint>();
        
        endpoints.MapGroup("/v1/orders")
            .WithTags("Orders")
            .RequireAuthorization()
            .WithBadge("v1", BadgePosition.After)
            .MapEndpoint<GetAllOrdersEndpoint>()
            .MapEndpoint<GetOrderByNumberEndpoint>()
            .MapEndpoint<CreateOrderEndpoint>()
            .MapEndpoint<CancelOrderEndpoint>()
            .MapEndpoint<PayOrderEndpoint>()
            .MapEndpoint<RefundOrderEndpoint>();

        endpoints.MapGroup("/v1/payments/stripe")
            .WithTags("Payments - Stripe")
            .RequireAuthorization()
            .WithBadge("v1", BadgePosition.After)
            .MapEndpoint<CreateSessionEndpoint>();

    }

    private static RouteHandlerBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app)
        where TEndpoint : IEndpoint
    {
        return TEndpoint.Map(app);
    }
}