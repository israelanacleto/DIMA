using Dima.Api.Common.Api;
using Dima.Api.Endpoints.Categories;
using Scalar.AspNetCore;

namespace Dima.Api.Endpoints;

public static class Endpoint
{
    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = app
            .MapGroup("")
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        endpoints.MapGroup("v1/categories")
            .WithTags("Categories")
            // .RequireAuthorization()
            .WithBadge("v1", BadgePosition.After)
            .MapEndpoint<CreateCategoryEndpoint>()
            .MapEndpoint<UpdateCategoryEndpoint>()
            .MapEndpoint<DeleteCategoryEndpoint>()
            .MapEndpoint<GetCategoryByIdEndpoint>()
            .MapEndpoint<GetAllCategoriesEndpoint>();
    }

    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app)
        where TEndpoint : IEndpoint
    {
        TEndpoint.Map(app);
        return app;
    }
}