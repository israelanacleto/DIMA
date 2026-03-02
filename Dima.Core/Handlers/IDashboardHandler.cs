using Dima.Core.Models.Dashboard;
using Dima.Core.Requests.Dashboard;
using Dima.Core.Responses;

namespace Dima.Core.Handlers;

public interface IDashboardHandler
{
    Task<Response<List<IncomesAndExpenses>?>> GetIncomesAndExpensesReportAsync(GetIncomesAndExpensesRequest request);
    Task<Response<List<IncomesByCategory>?>> GetIncomesByCategoryReportAsync(GetIncomesByCategoryRequest request);
    Task<Response<List<ExpensesByCategory>?>> GetExpensesByCategoryReportAsync(GetExpensesByCategoryRequest request);
    Task<Response<List<MostUsedCategory>?>> GetMostUsedCategoriesReportAsync(GetMostUsedCategoriesRequest request);
    Task<Response<FinancialSummary?>> GetFinancialSummaryReportAsync(GetFinancialSummaryRequest request);
}