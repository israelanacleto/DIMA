namespace Dima.Core.Models.Dashboard;

public record FinancialSummary(
    string UserId,
    decimal Incomes,
    decimal Expenses)
{
    public decimal Total => Incomes - (Expenses < 0 ? -Expenses : Expenses);
}