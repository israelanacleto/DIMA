using Dima.Core.Handlers;
using Dima.Core.Models.Dashboard;
using Dima.Core.Requests.Dashboard;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages;

public partial class HomePage : ComponentBase
{
    #region Properties

    public bool SummaryLoading { get; set; } = true;
    public bool ShowValues { get; set; } = true;
    public FinancialSummary? Summary { get; set; }

    #endregion

    #region Services

    [Inject]
    public IDashboardHandler Handler { get; set; } = null!;

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        await GetFinancialSummaryAsync();
    }

    #endregion

    #region Private Methods

    private async Task GetFinancialSummaryAsync()
    {
        SummaryLoading = true;
        try
        {
            var result = await Handler.GetFinancialSummaryReportAsync(new GetFinancialSummaryRequest());
            if (result.IsSuccess)
                Summary = result.Data;
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            SummaryLoading = false;
            StateHasChanged();
        }
    }

    #endregion

    #region Public Methods

    public void ToggleShowValues()
        => ShowValues = !ShowValues;

    #endregion
}

