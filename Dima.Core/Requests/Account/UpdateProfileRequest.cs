namespace Dima.Core.Requests.Account;

public class UpdateProfileRequest : Request
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}