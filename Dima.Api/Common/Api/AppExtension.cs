using Dima.Api.Data;
using Dima.Api.Models;
using Dima.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Common.Api;

public static class AppExtension
{
    public static void ConfigureDevEnvironment(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        try
        {
            // 1. Ensure Database and Views
            context.Database.EnsureCreated();
            CreateViews(context);
            
            // 2. Seed Data if empty
            SeedInitialData(context);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during initialization: {ex.Message}");
        }
    }

    private static void CreateViews(AppDbContext context)
    {
        var views = new[]
        {
            @"CREATE OR ALTER VIEW [vwGetExpensesByCategory] AS
                SELECT [Transaction].[UserId], [Category].[Title] AS [Category], YEAR([Transaction].[PaidOrReceivedAt]) AS [Year], SUM([Transaction].[Amount]) AS [Expenses]
                FROM [Transaction] INNER JOIN [Category] ON [Transaction].[CategoryId] = [Category].[Id]
                WHERE [Transaction].PaidOrReceivedAt >= DATEADD(MONTH, -12, CAST(GETDATE() AS DATE)) AND [Transaction].[Type] = 2
                GROUP BY [Transaction].[UserId], [Category].[Title], YEAR([Transaction].[PaidOrReceivedAt])",

            @"CREATE OR ALTER VIEW [vwGetIncomesAndExpenses] AS
                SELECT [Transaction].[UserId], MONTH([Transaction].[PaidOrReceivedAt]) AS [Month], YEAR([Transaction].[PaidOrReceivedAt]) AS [Year],
                SUM(CASE WHEN [Transaction].[Type] = 1 THEN [Transaction].[Amount] ELSE 0 END) AS [Incomes],
                SUM(CASE WHEN [Transaction].[Type] = 2 THEN [Transaction].[Amount] ELSE 0 END) AS [Expenses]
                FROM [Transaction]
                WHERE [Transaction].[PaidOrReceivedAt] >= DATEADD(MONTH, -11, CAST(GETDATE() AS DATE))
                GROUP BY [Transaction].[UserId], MONTH([Transaction].[PaidOrReceivedAt]), YEAR([Transaction].[PaidOrReceivedAt])",

            @"CREATE OR ALTER VIEW [vwGetIncomesByCategory] AS
                SELECT [Transaction].[UserId], [Category].[Title] AS [Category], YEAR([Transaction].[PaidOrReceivedAt]) AS [Year], SUM([Transaction].[Amount]) AS [Incomes]
                FROM [Transaction] INNER JOIN [Category] ON [Transaction].[CategoryId] = [Category].[Id]
                WHERE [Transaction].[PaidOrReceivedAt] >= DATEADD(MONTH, -11, CAST(GETDATE() AS DATE)) AND [Transaction].[Type] = 1
                GROUP BY [Transaction].[UserId], [Category].[Title], YEAR([Transaction].[PaidOrReceivedAt])"
        };

        foreach (var view in views)
        {
            context.Database.ExecuteSqlRaw(view);
        }
    }

    private static void SeedInitialData(AppDbContext context)
    {
        // We look for any user to seed some demo data if the DB is empty
        if (context.Categories.Any()) return;

        // Note: Seed usually needs a UserId. For demo, we can skip or wait for first login.
        // But let's create global categories at least.
        var categories = new List<Category>
        {
            new() { Title = "Salário", Description = "Renda principal", UserId = "demo@dima.com" },
            new() { Title = "Aluguel", Description = "Moradia", UserId = "demo@dima.com" },
            new() { Title = "Alimentação", Description = "Supermercado e Restaurantes", UserId = "demo@dima.com" },
            new() { Title = "Lazer", Description = "Cinema, Viagens, etc", UserId = "demo@dima.com" }
        };

        context.Categories.AddRange(categories);
        context.SaveChanges();
    }
}