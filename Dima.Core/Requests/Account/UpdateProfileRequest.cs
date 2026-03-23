using System.ComponentModel.DataAnnotations;

namespace Dima.Core.Requests.Account;

public class UpdateProfileRequest : Request
{
    public string PhoneNumber { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Nome inválido")]
    [MaxLength(250, ErrorMessage = "Nome deve conter até 250 caracteres")]
    public string Name { get; set; } = string.Empty;
}