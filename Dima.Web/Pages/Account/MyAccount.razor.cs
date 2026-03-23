using Dima.Core.Handlers;
using Dima.Core.Requests.Account;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using System.Text.RegularExpressions;

namespace Dima.Web.Pages.Account;

public partial class MyAccountPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; } = false;
    public UpdateProfileRequest InputModel { get; set; } = new();
    public ChangePasswordRequest PasswordRequest { get; set; } = new();
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;

    public int StrengthValue { get; set; } = 0;
    public string StrengthLabel { get; set; } = "Muito Fraca";
    public Color StrengthColor { get; set; } = Color.Error;

    #endregion

    #region Services

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    public IProfileHandler ProfileHandler { get; set; } = null!;

    [Inject]
    public IAccountHandler AccountHandler { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;
        try
        {
            var request = new GetProfileRequest();
            var response = await ProfileHandler.GetProfileAsync(request);

            if (response is { IsSuccess: true, Data: not null })
            {
                InputModel.Name = response.Data.Name;
                InputModel.PhoneNumber = response.Data.PhoneNumber;
                Email = response.Data.Email;
                UserName = response.Data.UserName;
            }
            else
            {
                Snackbar.Add(response.Message ?? "Não foi possível recuperar o perfil", Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    #endregion

    #region Methods

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;

        try
        {
            // Limpa a máscara do telefone antes de salvar
            var phone = InputModel.PhoneNumber;
            InputModel.PhoneNumber = Regex.Replace(InputModel.PhoneNumber, @"[^\d]", "");

            var result = await ProfileHandler.UpdateProfileAsync(InputModel);
            if (result.IsSuccess)
            {
                Snackbar.Add("Perfil atualizado com sucesso!", Severity.Success);
            }
            else
            {
                InputModel.PhoneNumber = phone; // Restaura a máscara em caso de erro
                Snackbar.Add(result.Message ?? "Não foi possível atualizar o perfil", Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async Task OnChangePasswordAsync()
    {
        IsBusy = true;

        try
        {
            var result = await AccountHandler.ChangePasswordAsync(PasswordRequest);
            if (result.IsSuccess)
            {
                Snackbar.Add("Senha alterada com sucesso!", Severity.Success);
                PasswordRequest = new();
                StrengthValue = 0;
                StrengthLabel = "Muito Fraca";
                StrengthColor = Color.Error;
            }
            else
            {
                Snackbar.Add(result.Message ?? "Não foi possível alterar a sua senha", Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    public void CheckPasswordStrength(KeyboardEventArgs e)
    {
        var password = PasswordRequest.NewPassword;
        if (string.IsNullOrEmpty(password))
        {
            StrengthValue = 0;
            StrengthLabel = "Muito Fraca";
            StrengthColor = Color.Error;
            return;
        }

        var score = 0;
        if (password.Length >= 8) score++;
        if (Regex.IsMatch(password, @"[a-z]")) score++;
        if (Regex.IsMatch(password, @"[A-Z]")) score++;
        if (Regex.IsMatch(password, @"[0-9]")) score++;
        if (Regex.IsMatch(password, @"[^a-zA-Z0-9]")) score++;

        switch (score)
        {
            case 0:
            case 1:
                StrengthValue = 20;
                StrengthLabel = "Fraca";
                StrengthColor = Color.Error;
                break;
            case 2:
                StrengthValue = 40;
                StrengthLabel = "Razoável";
                StrengthColor = Color.Warning;
                break;
            case 3:
                StrengthValue = 60;
                StrengthLabel = "Boa";
                StrengthColor = Color.Info;
                break;
            case 4:
                StrengthValue = 80;
                StrengthLabel = "Forte";
                StrengthColor = Color.Success;
                break;
            case 5:
                StrengthValue = 100;
                StrengthLabel = "Muito Forte";
                StrengthColor = Color.Success;
                break;
        }
    }

    #endregion
}