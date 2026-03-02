using Dima.Core.Handlers;
using Dima.Core.Requests.Orders;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Orders;

public partial class SuccessPage : ComponentBase
{
    #region Parameters

    [Parameter]
    public string OrderNumber { get; set; } = string.Empty;

    #endregion

    #region Properties

    public bool IsBusy { get; set; } = true;
    public bool IsConfirmed { get; set; } = false;

    #endregion

    #region Services

    [Inject]
    public IOrderHandler OrderHandler { get; set; } = null!;
    
    [Inject]
    public IProfileHandler ProfileHandler { get; set; } = null!;
    
    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;
    
    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        await ConfirmPaymentAsync();
    }

    #endregion

    #region Methods

    public async Task ConfirmPaymentAsync()
    {
        IsBusy = true;
        await Task.Delay(10000);
        try
        {
            var request = new PayOrderRequest
            {
                OrderNumber = OrderNumber
            };

            for (var i = 0; i < 5; i++)
            {
                var result = await OrderHandler.PayAsync(request);
                if (result.IsSuccess)
                {
                    IsConfirmed = true;
                    IsBusy = false;
                    ProfileHandler.NotifyChange();
                    StateHasChanged();
                    return;
                }

                await Task.Delay(3000);
            }

            IsConfirmed = false;
            Snackbar.Add("Não foi possível confirmar seu pagamento. Por favor, verifique os detalhes do pedido.", Severity.Error);
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
            StateHasChanged();
        }
    }

    #endregion
}
