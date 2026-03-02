using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Account;
using Dima.Core.Requests.Orders;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Products;

public partial class ListProductsPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; } = false;
    public List<Product> Products { get; set; } = [];

    #endregion

    #region Services

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    public IProductHandler Handler { get; set; } = null!;

    [Inject]
    public IProfileHandler ProfileHandler { get; set; } = null!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

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
                    NavigationManager.NavigateTo("/settings");
                    return;
                }
            }

            var request = new GetAllProductsRequest();
            var result = await Handler.GetAllAsync(request);
            if (result.IsSuccess)
                Products = result.Data ?? [];
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
