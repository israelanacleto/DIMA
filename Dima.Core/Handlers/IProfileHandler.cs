using Dima.Core.Requests.Account;
using Dima.Core.Responses;
using Dima.Core.Responses.Account;

namespace Dima.Core.Handlers;

public interface IProfileHandler
{
    event Action? OnChange;
    void NotifyChange();
    Task<Response<GetProfileResponse?>> GetProfileAsync(GetProfileRequest request);
    
    Task<Response<GetProfileResponse?>> UpdateProfileAsync(UpdateProfileRequest request);
    
    Task<Response<string?>> ChangePasswordAsync(ChangePasswordRequest request);
}