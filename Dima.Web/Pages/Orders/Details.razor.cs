using Dima.Core.Enums;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Requests.Stripe;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace Dima.Web.Pages.Orders;

public partial class DetailsPage : ComponentBase
{
    #region Parameters

    [Parameter]
    public string OrderNumber { get; set; } = string.Empty;

    #endregion

    #region Properties

    public bool IsBusy { get; set; } = false;
    protected Order? Order { get; set; }
    
    protected bool CanRefund =>
        Order?.SubscriptionStartDate >= DateTime.UtcNow.AddDays(-7);

    #endregion

    #region Services

    [Inject]
    public IJSRuntime JsRuntime { get; set; } = null!;

    [Inject]
    public IOrderHandler Handler { get; set; } = null!;

    [Inject]
    public IStripeHandler StripeHandler { get; set; } = null!;

    [Inject]
    public IProfileHandler ProfileHandler { get; set; } = null!;

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;
        try
        {
            var result = await Handler.GetByNumberAsync(new GetOrderByNumberRequest { Number = OrderNumber });
            if (result.IsSuccess)
                Order = result.Data;
            else
                Snackbar.Add(result.Message ?? "Não foi possível obter o pedido", Severity.Error);
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

    public async Task OnPayAsync()
    {
        IsBusy = true;
        if (Order is null)
        {
            Snackbar.Add("Erro ao tentar processar pagamento.", Severity.Error);
            return;
        }

        var request = new CreateSessionRequest
        {
            OrderNumber = Order.Number,
            ProductTitle = Order.Product.Title,
            ProductDescription = Order.Product.Description,
            OrderTotal = (int)Math.Round(Order.Total * 100, 2)
        };

        try
        {
            var result = await StripeHandler.CreateSessionAsync(request);
            if (!result.IsSuccess)
            {
                Snackbar.Add(result.Message ?? "Erro ao criar a sessão do pagamento.", Severity.Error);
                return;
            }

            if (result.Data is null)
            {
                Snackbar.Add(result.Message ?? "Erro ao obter a sessão do pagamento.", Severity.Error);
                return;
            }
            
            await JsRuntime.InvokeVoidAsync("checkout", Configuration.StripePublicKey, result.Data.SessionId);

        }
        catch (Exception ex)
        {
            Snackbar.Add("Não foi possível iniciar a sessão com o gateway de pagamento", Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
        // try
        // {
        //
        //     // var request = new PayOrderRequest
        //     // {
        //     //     OrderNumber = OrderNumber,
        //     //     ExternalReference = "PAGAMENTO_SIMULADO"
        //     // };
        //     // var result = await Handler.PayAsync(request);
        //     // if (result.IsSuccess)
        //     // {
        //     //     Order = result.Data;
        //     //     Snackbar.Add("Pagamento realizado com sucesso", Severity.Success);
        //     //     ProfileHandler.NotifyChange();
        //     // }
        //     // else
        //     // {
        //     //     Snackbar.Add(result.Message ?? "Não foi possível realizar o pagamento", Severity.Error);
        //     // }
        // }
        
    }

    public async Task OnCancelAsync()
    {
        IsBusy = true;
        try
        {
            var request = new CancelOrderRequest { Id = Order!.Id };
            var result = await Handler.CancelAsync(request);
            if (result.IsSuccess)
            {
                Order = result.Data;
                Snackbar.Add("Pedido cancelado com sucesso", Severity.Success);
            }
            else
            {
                Snackbar.Add(result.Message ?? "Não foi possível cancelar o pedido", Severity.Error);
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
    
    public async Task OnRefundedAsync()
    {
        IsBusy = true;
        try
        {
            var request = new RefundOrderRequest() { Id = Order!.Id };
            var result = await Handler.RefundAsync(request);
            if (result.IsSuccess)
            {
                Order = result.Data;
                Snackbar.Add("Solicitação de reembolso realizada com sucesso", Severity.Success);
                ProfileHandler.NotifyChange();
            }
            else
            {
                Snackbar.Add(result.Message ?? "Não foi possível realizar o reembolso", Severity.Error);
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
}
