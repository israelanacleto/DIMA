using Dima.Api.Data;
using Dima.Core.Handlers;
using Dima.Core.Requests.Account;
using Dima.Core.Responses;
using Dima.Core.Responses.Account;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class ProfileHandler(AppDbContext context) : IProfileHandler
{
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

            return user is null
                ? new Response<GetProfileResponse?>(null, 401, "Erro ao obter o perfil do usuário")
                : new Response<GetProfileResponse?>(user);
        }
        catch
        {
            return new Response<GetProfileResponse?>(null, 500, "Não foi possível recuperar a conta");
        }
    }
}