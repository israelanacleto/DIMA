using Dima.Api.Common.Api;
using Dima.Api.Models;
using Dima.Core.Requests.Account;
using Microsoft.AspNetCore.Identity;

namespace Dima.Api.Endpoints.Identity;

public class LoginEndpoint : IEndpoint
{
    public static RouteHandlerBuilder Map(IEndpointRouteBuilder app)
        => app.MapPost("/login", HandleAsync)
            .WithName("Identity: Login")
            .WithSummary("Authenticates a user")
            .WithDescription("Authenticates a user and returns a session cookie")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

    private static async Task<IResult> HandleAsync(
        SignInManager<User> signInManager,
        LoginRequest request)
    {
        var result = await signInManager.PasswordSignInAsync(
            request.Email, 
            request.Password, 
            isPersistent: true, 
            lockoutOnFailure: false);

        if (result.Succeeded)
            return Results.Ok(new Dima.Core.Responses.Response<string>("Login realizado com sucesso!", 200, "Login realizado com sucesso!"));

        return Results.Json(new Dima.Core.Responses.Response<string>(null, 401, "E-mail ou senha inválidos"), statusCode: 401);
    }
}