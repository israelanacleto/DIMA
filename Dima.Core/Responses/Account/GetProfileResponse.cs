namespace Dima.Core.Responses.Account;

public class GetProfileResponse
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsPremium { get; set; }
}