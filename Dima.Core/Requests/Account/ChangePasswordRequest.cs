using System.ComponentModel.DataAnnotations;

namespace Dima.Core.Requests.Account;

public class ChangePasswordRequest : Request
{
    [Required(ErrorMessage = "Senha atual inválida")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nova senha inválida")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirmação de senha inválida")]
    [Compare(nameof(NewPassword), ErrorMessage = "As senhas não coincidem")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
