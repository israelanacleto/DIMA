using Dima.Core;
using Dima.Core.Handlers;
using Dima.Core.Requests.Stripe;
using Dima.Core.Responses;
using Dima.Core.Responses.Stripe;
using Stripe;
using Stripe.Checkout;

namespace Dima.Api.Handlers;

public class StripeHandler : IStripeHandler
{
    public async Task<Response<CreateSessionResponse?>> CreateSessionAsync(CreateSessionRequest request)
    {
        var options = new SessionCreateOptions
        {
            CustomerEmail = request.UserId,
            PaymentIntentData = new SessionPaymentIntentDataOptions
            {
                Metadata = new Dictionary<string, string> {
                {
                    "order", request.OrderNumber
                }}
            },
            PaymentMethodTypes = [
                "card"
            ],
            LineItems = [
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "brl",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = request.ProductTitle,
                            Description = request.ProductDescription
                        },
                        UnitAmount = request.OrderTotal
                    },
                    Quantity = 1
                }
            ],
            Mode = "payment",
            SuccessUrl = $"{Configuration.FrontendUrl}/orders/{request.OrderNumber}/success",
            CancelUrl = $"{Configuration.FrontendUrl}/orders/{request.OrderNumber}/cancel"
        };
        
        var service = new SessionService();
        var session = await service.CreateAsync(options);

        var response = new CreateSessionResponse
        {
            SessionId = session.Id,
            RedirectUrl = session.Url
        };
        
        return new Response<CreateSessionResponse?>(response);
    }

    public async Task<Response<List<StripeTransactionResponse>>> GetTransactionsByOrderNumberAsync(GetTransactionsByOrderNumberRequest request)
    {
        var options = new ChargeSearchOptions
        {
            Query = $"metadata['order']:'{request.Number}'"
        };

        var service = new ChargeService();
        var result = await service.SearchAsync(options);

        if (result.Data.Count == 0)
        {
            return new Response<List<StripeTransactionResponse>>(null, 404, "Nenhuma transação foi encontrada");
        }

        var data = result.Data.Select(item => new StripeTransactionResponse
            {
                Amount = item.Amount,
                AmountCaptured = item.AmountCaptured,
                Email = item.BillingDetails.Email,
                Paid = item.Paid,
                Refunded = item.Refunded,
                Id = item.Id
            })
            .ToList();

        return new Response<List<StripeTransactionResponse>>(data);
    }
}