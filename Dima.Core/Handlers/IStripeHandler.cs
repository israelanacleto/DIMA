using Dima.Core.Requests.Stripe;
using Dima.Core.Responses;
using Dima.Core.Responses.Stripe;

namespace Dima.Core.Handlers;

public interface IStripeHandler
{
    Task<Response<CreateSessionResponse?>> CreateSessionAsync(CreateSessionRequest request);
    Task<Response<List<StripeTransactionResponse>>> GetTransactionsByOrderNumberAsync(GetTransactionsByOrderNumberRequest request);
}