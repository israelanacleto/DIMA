using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Account;
using Dima.Core.Requests.Orders;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Orders;

public partial class ListOrdersPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; } = false;
    public List<Order> Orders { get; set; } = [];
    public bool IsPremium { get; set; } = false;

    #endregion

    #region Services

    [Inject]
    public IOrderHandler Handler { get; set; } = null!;

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
            var profileResult = await ProfileHandler.GetProfileAsync(new GetProfileRequest());
            if (profileResult.IsSuccess)
                IsPremium = profileResult.Data?.IsPremium ?? false;

            var request = new GetAllOrdersRequest();
            var result = await Handler.GetAllAsync(request);
            if (result.IsSuccess)
                Orders = result.Data ?? [];
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
