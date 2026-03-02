using System.Net.Http.Json;
using Dima.Core.Handlers;
using Dima.Core.Models.Dashboard;
using Dima.Core.Requests.Dashboard;
using Dima.Core.Responses;

namespace Dima.Web.Handlers;

public class DashboardHandler(IHttpClientFactory httpClientFactory) : IDashboardHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);
    
    public async Task<Response<List<IncomesAndExpenses>?>> GetIncomesAndExpensesReportAsync(GetIncomesAndExpensesRequest request)
    {
        return await _client.GetFromJsonAsync<Response<List<IncomesAndExpenses>?>>("v1/dashboard/incomes-expenses")
               ?? new Response<List<IncomesAndExpenses>?>(null, 400, "Não foi possível obter os dados");
    }

    public async Task<Response<List<IncomesByCategory>?>> GetIncomesByCategoryReportAsync(
        GetIncomesByCategoryRequest request)
    {
        return await _client.GetFromJsonAsync<Response<List<IncomesByCategory>?>>($"v1/dashboard/incomes")
               ?? new Response<List<IncomesByCategory>?>(null, 400, "Não foi possível obter os dados");
    }

    public async Task<Response<List<ExpensesByCategory>?>> GetExpensesByCategoryReportAsync(
        GetExpensesByCategoryRequest request)
    {
        return await _client.GetFromJsonAsync<Response<List<ExpensesByCategory>?>>($"v1/dashboard/expenses")
               ?? new Response<List<ExpensesByCategory>?>(null, 400, "Não foi possível obter os dados");
    }

    public async Task<Response<List<MostUsedCategory>?>> GetMostUsedCategoriesReportAsync(
        GetMostUsedCategoriesRequest request)
    {
        return await _client.GetFromJsonAsync<Response<List<MostUsedCategory>?>>($"v1/dashboard/most-used")
               ?? new Response<List<MostUsedCategory>?>(null, 400, "Não foi possível obter os dados");
    }

    public async Task<Response<FinancialSummary?>> GetFinancialSummaryReportAsync(GetFinancialSummaryRequest request)
    {
        return await _client.GetFromJsonAsync<Response<FinancialSummary?>>($"v1/dashboard/summary")
               ?? new Response<FinancialSummary?>(null, 400, "Não foi possível obter os dados");
    }
}