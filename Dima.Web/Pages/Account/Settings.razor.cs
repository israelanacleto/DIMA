using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Account;
using Dima.Core.Requests.Orders;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Account;

public partial class SettingsPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; } = false;
    public Order? Order { get; set; }

    #endregion

    #region Services

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    public IOrderHandler Handler { get; set; } = null!;

    [Inject]
    public IProfileHandler ProfileHandler { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;
        try
        {
            var profileResult = await ProfileHandler.GetProfileAsync(new GetProfileRequest());
            if (profileResult.IsSuccess && profileResult.Data is { IsPremium: true })
            {
                var request = new GetAllOrdersRequest();
                var result = await Handler.GetAllAsync(request);
                if (result.IsSuccess)
                {
                    Order = result.Data?
                        .Where(x => x.Status == Core.Enums.EOrderStatus.Paid &&
                                    x.CreatedAt.AddDays(30) > DateTime.UtcNow)
                        .OrderByDescending(x => x.CreatedAt)
                        .FirstOrDefault();
                }
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