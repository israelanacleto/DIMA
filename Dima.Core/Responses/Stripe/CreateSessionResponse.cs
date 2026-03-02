namespace Dima.Core.Responses.Stripe;

public class CreateSessionResponse
{
    public string SessionId { get; set; } = string.Empty;
    public string RedirectUrl { get; set; } = string.Empty;
}