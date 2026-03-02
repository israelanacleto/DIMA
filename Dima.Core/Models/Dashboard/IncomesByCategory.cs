namespace Dima.Core.Models.Dashboard;

public record IncomesByCategory(
    string UserId, 
    string Category, 
    int Year, 
    decimal Incomes);