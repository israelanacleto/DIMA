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
            Metadata = new Dictionary<string, string> {
            {
                "order", request.OrderNumber
            }},
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
        try
        {
            // 1. Tenta buscar no Checkout Session (Lugar mais seguro onde o fluxo começa)
            var sessionOptions = new SessionListOptions
            {
                Limit = 5 // Pega as últimas 5 para garantir
            };
            var sessionService = new SessionService();
            var sessions = await sessionService.ListAsync(sessionOptions);
            
            // Filtra manualmente as sessões que batem com o número do pedido no metadata
            var session = sessions.FirstOrDefault(x => 
                x.Metadata.ContainsKey("order") && 
                x.Metadata["order"] == request.Number &&
                x.PaymentStatus == "paid");

            if (session != null)
            {
                return new Response<List<StripeTransactionResponse>>([new StripeTransactionResponse
                {
                    Id = session.PaymentIntentId ?? session.Id,
                    Amount = session.AmountTotal ?? 0,
                    AmountCaptured = session.AmountTotal ?? 0,
                    Email = session.CustomerEmail,
                    Paid = true,
                    Refunded = false
                }]);
            }

            // 2. Fallback: Busca no PaymentIntent (O que já tínhamos)
            var piOptions = new PaymentIntentSearchOptions { Query = $"metadata['order']:'{request.Number}'" };
            var piService = new PaymentIntentService();
            var piResult = await piService.SearchAsync(piOptions);

            if (piResult.Data.Any())
            {
                var data = piResult.Data.Select(pi => new StripeTransactionResponse
                {
                    Id = pi.Id,
                    Amount = pi.Amount,
                    AmountCaptured = pi.AmountReceived,
                    Email = pi.ReceiptEmail,
                    Paid = pi.Status == "succeeded",
                    Refunded = false
                }).ToList();
                return new Response<List<StripeTransactionResponse>>(data);
            }

            return new Response<List<StripeTransactionResponse>>(null, 404, "Nenhuma transação foi encontrada no Stripe para este pedido.");
        }
        catch (Exception ex)
        {
            return new Response<List<StripeTransactionResponse>>(null, 500, $"Erro ao consultar Stripe: {ex.Message}");
        }
    }
}