using Dima.Core.Handlers;
using Dima.Core.Models.Dashboard;
using Dima.Core.Requests.Dashboard;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Components.Dashboard;

public partial class IncomesByCategoryChart : ComponentBase
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
        await GetIncomesByCategoryAsync();
    }

    #endregion

    #region Private Methods

    private void ConfigureChart()
    {
        Options.ChartPalette = Configuration.ChartPalette;
    }

    private async Task GetIncomesByCategoryAsync()
    {
        IsLoading = true;
        try
        {
            var result = await Handler.GetIncomesByCategoryReportAsync(new GetIncomesByCategoryRequest());
            if (!result.IsSuccess || result.Data is null) return;

            var rawData = result.Data
                .GroupBy(x => x.Category)
                .Select(x => new { Category = x.Key, Amount = x.Sum(y => Math.Abs(y.Incomes)) })
                .Where(x => x.Amount > 0)
                .OrderByDescending(x => x.Amount)
                .ToList();

            var top = rawData.Take(10).ToList();
            var others = rawData.Skip(10).ToList();

            if (others.Any())
            {
                top.Add(new { Category = "Outros", Amount = others.Sum(x => x.Amount) });
            }

            Labels = top.Select(x => x.Category).ToArray();
            Data = top.Select(x => (double)x.Amount).ToArray();
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
