using Dima.Core.Requests.Account;
using Dima.Core.Responses;
using Dima.Core.Responses.Account;

namespace Dima.Core.Handlers;

public interface IProfileHandler
{
    Task<Response<GetProfileResponse?>> GetProfileAsync(GetProfileRequest request);
    
    Task<Response<GetProfileResponse?>> UpdateProfileAsync(UpdateProfileRequest request);
}