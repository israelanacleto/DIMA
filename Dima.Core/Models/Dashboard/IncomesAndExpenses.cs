namespace Dima.Core.Models.Dashboard;

public record IncomesAndExpenses(
    string UserId, 
    int Month, 
    int Year, 
    decimal Incomes, 
    decimal Expenses);