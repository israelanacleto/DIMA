using System.Net.Http.Json;
using System.Text;
using Dima.Core.Handlers;
using Dima.Core.Requests.Account;
using Dima.Core.Responses;
using Dima.Core.Responses.Account;

namespace Dima.Web.Handlers;

public class AccountHandler(IHttpClientFactory httpClientFactory) : 
    IAccountHandler, 
    IProfileHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);
    
    public async Task<Response<string>> LoginAsync(LoginRequest request)
    {
        var result = await _client.PostAsJsonAsync("v1/identity/login?useCookies=true", request);
        return result.IsSuccessStatusCode
            ? new Response<string>("Login realizado com sucesso!", 200, "Login realizado com sucesso!")
            : new Response<string>(null, (int)result.StatusCode, "Não foi possível realizar o login");
    }

    public async Task<Response<string>> RegisterAsync(RegisterRequest request)
    {
        var result = await _client.PostAsJsonAsync("v1/identity/register", request);
        return result.IsSuccessStatusCode
            ? new Response<string>("Cadastro realizado com sucesso!", 201, "Cadastro realizado com sucesso!")
            : new Response<string>(null, (int)result.StatusCode, "Não foi possível realizar o seu cadastro");
    }

    public async Task<Response<string>> ChangePasswordAsync(ChangePasswordRequest request)
    {
        var result = await _client.PostAsJsonAsync("v1/identity/change-password", request);
        return result.IsSuccessStatusCode
            ? new Response<string>("Senha alterada com sucesso!", 204, "Senha alterada com sucesso!")
            : new Response<string>(null, (int)result.StatusCode, "Não foi possível alterar a sua senha");
    }

    public async Task LogoutAsync()
    {
        var emptyContent = new StringContent("{}", Encoding.UTF8, "application/json");
        await _client.PostAsJsonAsync("v1/identity/logout", emptyContent);
    }

    public async Task<Response<GetProfileResponse?>> GetProfileAsync(GetProfileRequest request)
    {
        return await _client.GetFromJsonAsync<Response<GetProfileResponse?>>($"v1/identity/me")
               ?? new Response<GetProfileResponse?>(null, 400, "Não foi possível recuperar o perfil do usuário");
    }

    public async Task<Response<GetProfileResponse?>> UpdateProfileAsync(UpdateProfileRequest request)
    {
        var result = await _client.PutAsJsonAsync("v1/identity/me", request);
        return await result
                   .Content
                   .ReadFromJsonAsync<Response<GetProfileResponse?>>()
               ?? new Response<GetProfileResponse?>(null, (int)result.StatusCode,
                   "Falha ao atualizar o perfil do usuário");
    }
}