using Dima.Core.Handlers;
using Dima.Core.Requests.Orders;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Orders;

public partial class CancelPage : ComponentBase
{
    #region Parameters

    [Parameter]
    public string OrderNumber { get; set; } = string.Empty;

    #endregion

    #region Services

    [Inject]
    public IOrderHandler OrderHandler { get; set; } = null!;
    
    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        var getOrderRequest = new GetOrderByNumberRequest
        {
            Number = OrderNumber
        };
        
        var orderResponse = await OrderHandler.GetByNumberAsync(getOrderRequest);

        if (!orderResponse.IsSuccess || orderResponse.Data == null)
        {
            Snackbar.Add("Não foi possível cancelar o pedido", Severity.Error);
            return;
        }

        var request = new CancelOrderRequest()
        {
            Id = orderResponse.Data.Id
        };
        var result = await OrderHandler.CancelAsync(request);
        if (result.IsSuccess)
        {
            Snackbar.Add("Pedido cancelado com sucesso", Severity.Success);
        }
        else
        {
            Snackbar.Add(result.Message ?? "Não foi possível cancelar o pedido", Severity.Error);
        }
    }

    #endregion
}
