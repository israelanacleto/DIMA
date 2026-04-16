using Dima.Api.Common.Api;
using Dima.Api.Data;
using Dima.Api.Models;
using Dima.Core.Requests.Account;
using Microsoft.AspNetCore.Identity;

namespace Dima.Api.Endpoints.Identity;

public class RegisterEndpoint : IEndpoint
{
    public static RouteHandlerBuilder Map(IEndpointRouteBuilder app)
        => app.MapPost("/register", HandleAsync)
            .WithName("Identity: Register")
            .WithSummary("Registers a new user")
            .WithDescription("Registers a new user and optionally seeds demo data")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

    private static async Task<IResult> HandleAsync(
        UserManager<User> userManager,
        AppDbContext context,
        RegisterRequest request)
    {
        var user = new User
        {
            UserName = request.Email,
            Email = request.Email
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return Results.BadRequest(result.Errors);

        await DbInitializer.SeedDemoDataAsync(context, user.Email!);

        return Results.Ok();
    }
}