using Dima.Api.Data;
using Dima.Api.Models;
using Dima.Core.Enums;
using Dima.Core.Handlers;
using Dima.Core.Requests.Account;
using Dima.Core.Responses;
using Dima.Core.Responses.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class ProfileHandler(AppDbContext context, UserManager<User> userManager) : IProfileHandler
{
    public event Action? OnChange;
    public void NotifyChange() => OnChange?.Invoke();

    public async Task<Response<GetProfileResponse?>> GetProfileAsync(GetProfileRequest request)
    {
        try
        {
            var user = await context.Users
                .AsNoTracking()
                .Select(u => new GetProfileResponse
                {
                    Name = u.Name,
                    Email = u.Email ?? string.Empty,
                    PhoneNumber = u.PhoneNumber ?? string.Empty,
                    UserName = u.UserName ?? string.Empty
                })
                .FirstOrDefaultAsync(u => u.Email == request.UserId);

            if (user is null)
                return new Response<GetProfileResponse?>(null, 401, "Erro ao obter o perfil do usuário");

            user.IsPremium = await context.Orders
                .AsNoTracking()
                .AnyAsync(x => x.UserId == request.UserId &&
                               x.Status == EOrderStatus.Paid &&
                               x.SubscriptionStartDate <= DateTime.UtcNow &&
                               x.SubscriptionEndDate >= DateTime.UtcNow);

            return new Response<GetProfileResponse?>(user);
        }
        catch
        {
            return new Response<GetProfileResponse?>(null, 500, "Não foi possível recuperar a conta");
        }
    }

    public async Task<Response<GetProfileResponse?>> UpdateProfileAsync(UpdateProfileRequest request)
    {
        try
        {
            var user = await context
                .Users
                .FirstOrDefaultAsync(u => u.Email == request.UserId);
        
            if (user is null)
                return new Response<GetProfileResponse?>(null, 401, "Erro ao obter o perfil do usuário");
        
            user.Name = request.Name;
            user.PhoneNumber = request.PhoneNumber;
        
            await context.SaveChangesAsync();

            var isPremium = await context.Orders
                .AsNoTracking()
                .AnyAsync(x => x.UserId == request.UserId &&
                               x.Status == EOrderStatus.Paid &&
                               x.SubscriptionStartDate <= DateTime.UtcNow &&
                               x.SubscriptionEndDate >= DateTime.UtcNow);

            return new Response<GetProfileResponse?>(new GetProfileResponse
                {
                    Name = user.Name,
                    Email = user.Email ?? string.Empty,
                    PhoneNumber = user.PhoneNumber ?? string.Empty,
                    UserName = user.UserName ?? string.Empty,
                    IsPremium = isPremium
                }
            );
        }
        catch
        {
            return new Response<GetProfileResponse?>(null, 500, "Erro ao atualizar o perfil do usuário");
        }
    }

    public async Task<Response<string?>> ChangePasswordAsync(ChangePasswordRequest request)
    {
        try
        {
            var user = await userManager.FindByEmailAsync(request.UserId);

            if (user is null)
                return new Response<string?>(null, 404, "Usuário não encontrado");

            var result = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            return result.Succeeded
                ? new Response<string?>("Senha alterada com sucesso")
                : new Response<string?>(null, 400, "Erro ao alterar a senha");
        }
        catch
        {
            return new Response<string?>(null, 500, "Erro ao alterar a senha");
        }
    }
}