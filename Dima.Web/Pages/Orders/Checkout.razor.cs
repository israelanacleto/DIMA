using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Account;
using Dima.Core.Requests.Orders;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Orders;

public partial class CheckoutPage : ComponentBase
{
    #region Parameters

    [Parameter]
    public string ProductSlug { get; set; } = string.Empty;

    [SupplyParameterFromQuery]
    public string? VoucherNumber { get; set; }

    #endregion

    #region Properties

    public bool IsBusy { get; set; } = false;
    public bool IsValid { get; set; } = false;
    private CreateOrderRequest InputModel { get; set; } = new();
    protected Product? Product { get; set; }
    protected Voucher? Voucher { get; set; }
    protected decimal Total { get; set; }
    
    protected readonly IMask VoucherMask = new BlockMask(
        delimiters:"-", 
        new Block('a', 4,4), 
        new Block('0', 4,4));

    #endregion

    #region Services

    [Inject]
    public IProductHandler ProductHandler { get; set; } = null!;

    [Inject]
    public IVoucherHandler VoucherHandler { get; set; } = null!;

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
        IsBusy = true;
        try
        {
            var profileResult = await ProfileHandler.GetProfileAsync(new GetProfileRequest());
            if (profileResult.IsSuccess && profileResult.Data is not null)
            {
                if (profileResult.Data.IsPremium)
                {
                    Snackbar.Add("Você já possui uma assinatura ativa.", Severity.Info);
                    NavigationManager.NavigateTo("/settings");
                    return;
                }
            }

            var result = await ProductHandler.GetBySlugAsync(new GetProductBySlugRequest { Slug = ProductSlug });
            if (result.IsSuccess && result.Data is not null)
            {
                Product = result.Data;
                InputModel.ProductId = Product.Id;
                IsValid = true;
                Total = Product.Price;
            }
            else
            {
                Snackbar.Add(result.Message ?? "Não foi possível obter o produto", Severity.Error);
                NavigationManager.NavigateTo("/plans");
            }

            if (!string.IsNullOrEmpty(VoucherNumber))
            {
                await OnApplyVoucherAsync();
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

    public async Task OnApplyVoucherAsync()
    {
        if (string.IsNullOrEmpty(VoucherNumber))
            return;

        IsBusy = true;
        try
        {
            var result = await VoucherHandler.GetByNumberAsync(new GetVoucherByNumberRequest
            {
                Number = VoucherNumber.Replace("-", string.Empty)
            });
            if (result.IsSuccess && result.Data is not null)
            {
                Voucher = result.Data;
                InputModel.VoucherId = Voucher.Id;
                Total = Product!.Price - Voucher.Amount;
                Snackbar.Add("Voucher aplicado com sucesso", Severity.Success);
            }
            else
            {
                Voucher = null;
                InputModel.VoucherId = null;
                VoucherNumber = string.Empty;
                Total = Product!.Price;
                Snackbar.Add(result.Message ?? "Voucher inválido", Severity.Error);
            }
        }
        catch
        {
            Snackbar.Add("Voucher inválido", Severity.Error);
            Voucher = null;
            InputModel.VoucherId = null;
            VoucherNumber = string.Empty;
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;
        try
        {
            var result = await OrderHandler.CreateAsync(InputModel);
            if (result.IsSuccess && result.Data is not null)
            {
                Snackbar.Add("Pedido criado com sucesso", Severity.Success);
                NavigationManager.NavigateTo($"/orders/{result.Data.Number}");
            }
            else
            {
                Snackbar.Add(result.Message ?? "Não foi possível criar o pedido", Severity.Error);
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
