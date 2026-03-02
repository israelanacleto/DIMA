using Dima.Core.Handlers;
using Dima.Core.Models.Dashboard;
using Dima.Core.Requests.Dashboard;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Components.Dashboard;

public partial class IncomesAndExpensesChart : ComponentBase
{
    #region Properties

    protected bool IsLoading { get; set; } = true;
    protected List<ChartSeries> Series { get; set; } = [];
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
        await GetIncomesAndExpensesAsync();
    }

    #endregion

    #region Private Methods

    private void ConfigureChart()
    {
        Options.InterpolationOption = InterpolationOption.NaturalSpline;
        Options.YAxisFormat = "C";
        Options.ChartPalette = Configuration.IncomesExpensesPalette;
    }

    private async Task GetIncomesAndExpensesAsync()
    {
        IsLoading = true;
        try
        {
            var result = await Handler.GetIncomesAndExpensesReportAsync(new GetIncomesAndExpensesRequest());
            if (!result.IsSuccess || result.Data is null) return;

            var data = result.Data;
            Labels = data
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .Select(x => GetMonthName(x.Month))
                .ToArray();

            Series =
            [
                new ChartSeries
                {
                    Name = "Receitas",
                    Data = data
                        .OrderBy(x => x.Year)
                        .ThenBy(x => x.Month)
                        .Select(x => (double)x.Incomes)
                        .ToArray()
                },
                new ChartSeries
                {
                    Name = "Despesas",
                    Data = data
                        .OrderBy(x => x.Year)
                        .ThenBy(x => x.Month)
                        .Select(x => (double)x.Expenses)
                        .ToArray()
                }
            ];
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

    private static string GetMonthName(int month)
        => new System.Globalization.DateTimeFormatInfo().GetAbbreviatedMonthName(month);

    #endregion
}
