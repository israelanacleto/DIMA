namespace Dima.Core.Models.Dashboard;

public record ExpensesByCategory(
    string UserId,
    string Category,
    int Year,
    decimal Expenses);