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

        var categories = endpoints.MapGroup("/v1/categories")
            .WithTags("Categories")
            .RequireAuthorization()
            .WithBadge("v1", BadgePosition.After);

        categories.MapEndpoint<GetAllCombosCategoryEndpoint>();
        categories.MapEndpoint<CreateCategoryEndpoint>();
        categories.MapEndpoint<UpdateCategoryEndpoint>();
        categories.MapEndpoint<DeleteCategoryEndpoint>();
        categories.MapEndpoint<GetCategoryByIdEndpoint>();
        categories.MapEndpoint<GetAllCategoriesEndpoint>();
        
        var transactions = endpoints.MapGroup("/v1/transactions")
            .WithTags("Transactions")
            .RequireAuthorization()
            .WithBadge("v1", BadgePosition.After);

        transactions.MapEndpoint<CreateTransactionEndpoint>();
        transactions.MapEndpoint<UpdateTransactionEndpoint>();
        transactions.MapEndpoint<DeleteTransactionEndpoint>();
        transactions.MapEndpoint<GetTransactionByIdEndpoint>();
        transactions.MapEndpoint<GetTransactionsByPeriodEndpoint>();
        
        var dashboard = endpoints.MapGroup("/v1/dashboard")
            .WithTags("Dashboard")
            .RequireAuthorization()
            .WithBadge("v1", BadgePosition.After);

        dashboard.MapEndpoint<GetExpensesByCategoryEndpoint>();
        dashboard.MapEndpoint<GetFinancialSummaryEndpoint>();
        dashboard.MapEndpoint<GetIncomesAndExpensesEndpoint>();
        dashboard.MapEndpoint<GetIncomesByCategoryEndpoint>();
        dashboard.MapEndpoint<GetMostUsedCategoriesEndpoint>();
        
        var products = endpoints.MapGroup("/v1/products")
            .WithTags("Products")
            .RequireAuthorization()
            .WithBadge("v1", BadgePosition.After);

        products.MapEndpoint<GetAllProductsEndpoint>();
        products.MapEndpoint<GetProductBySlugEndpoint>();

        var vouchers = endpoints.MapGroup("/v1/vouchers")
            .WithTags("Vouchers")
            .RequireAuthorization()
            .WithBadge("v1", BadgePosition.After);

        vouchers.MapEndpoint<GetVoucherByNumberEndpoint>();
        
        var orders = endpoints.MapGroup("/v1/orders")
            .WithTags("Orders")
            .RequireAuthorization()
            .WithBadge("v1", BadgePosition.After);

        orders.MapEndpoint<GetAllOrdersEndpoint>();
        orders.MapEndpoint<GetOrderByNumberEndpoint>();
        orders.MapEndpoint<CreateOrderEndpoint>();
        orders.MapEndpoint<CancelOrderEndpoint>();
        orders.MapEndpoint<PayOrderEndpoint>();
        orders.MapEndpoint<RefundOrderEndpoint>();

        var payments = endpoints.MapGroup("/v1/payments/stripe")
            .WithTags("Payments - Stripe")
            .RequireAuthorization()
            .WithBadge("v1", BadgePosition.After);

        payments.MapEndpoint<CreateSessionEndpoint>();

    }

    private static RouteHandlerBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app)
        where TEndpoint : IEndpoint
    {
        return TEndpoint.Map(app);
    }
}