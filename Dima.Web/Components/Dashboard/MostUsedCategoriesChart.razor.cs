using Dima.Core.Handlers;
using Dima.Core.Models.Dashboard;
using Dima.Core.Requests.Dashboard;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Components.Dashboard;

public partial class MostUsedCategoriesChartPage : ComponentBase
{
    #region Properties

    protected bool IsLoading { get; set; } = true;
    protected double[] Data { get; set; } = [];
    protected string[] Labels { get; set; } = [];
    protected ChartOptions Options { get; set; } = new();

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
        ConfigureChart();
        await GetMostUsedCategoriesAsync();
    }

    #endregion

    #region Private Methods

    private void ConfigureChart()
    {
        Options.ChartPalette = Configuration.ChartPalette;
    }

    private async Task GetMostUsedCategoriesAsync()
    {
        IsLoading = true;
        try
        {
            var result = await Handler.GetMostUsedCategoriesReportAsync(new GetMostUsedCategoriesRequest());
            if (!result.IsSuccess || result.Data is null) return;

            var rawData = result.Data;
            var top = rawData.Take(10).ToList();
            var others = rawData.Skip(10).ToList();

            if (others.Any())
            {
                top.Add(new MostUsedCategory("Outros", others.Sum(x => x.Count)));
            }

            Labels = top.Select(x => x.Category).ToArray();
            Data = top.Select(x => (double)x.Count).ToArray();
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }

    #endregion
}
