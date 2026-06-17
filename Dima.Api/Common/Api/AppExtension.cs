using Dima.Api.Data;
using Dima.Api.Models;
using Dima.Core.Enums;
using Dima.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Common.Api;

public static class AppExtension
{
    public static async Task ConfigureDevEnvironmentAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        try
        {
            // 1. Aplica migrations automaticamente (pula no banco In-Memory dos testes)
            if (context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                // Espera o banco ficar disponível (container do Postgres ainda subindo, etc.)
                await WaitForDatabaseAsync(context);

                context.Database.Migrate();

                // 2. (Re)cria as views do dashboard em PostgreSQL (idempotente)
                await CreateDashboardViewsAsync(context);
            }

            // 3. Seed dos planos/produtos
            await DbInitializer.SeedAsync(context);

            // 4. Conta de demonstração com 6 meses de finanças (idempotente)
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            await DbInitializer.SeedDemoAsync(context, userManager);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during initialization: {ex.Message}");
        }

        if (app.Environment.IsDevelopment())
        {
            app.AddScalarConfig();
        }
    }

    public static void UseSecurity(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }

    /// <summary>
    /// Aguarda o banco aceitar conexões antes de migrar. Evita a corrida em que o
    /// app sobe antes do Postgres terminar de inicializar (Docker, primeiro deploy).
    /// </summary>
    private static async Task WaitForDatabaseAsync(AppDbContext context, int maxAttempts = 15, int delaySeconds = 3)
    {
        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                if (await context.Database.CanConnectAsync())
                    return;
            }
            catch
            {
                // banco ainda subindo
            }

            Console.WriteLine($"Aguardando o banco de dados... tentativa {attempt}/{maxAttempts}");
            await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
        }
    }

    /// <summary>
    /// Views do dashboard reescritas em PostgreSQL. Antes viviam em uma migration
    /// com SQL de SQL Server; agora são (re)criadas no boot de forma idempotente
    /// (CREATE OR REPLACE), logo após as migrations, garantindo que existam mesmo
    /// num banco recém-criado no Railway.
    /// </summary>
    private static async Task CreateDashboardViewsAsync(AppDbContext context)
    {
        const string expensesByCategory = """
            CREATE OR REPLACE VIEW "vwGetExpensesByCategory" AS
            SELECT
                t."UserId" AS "UserId",
                c."Title"  AS "Category",
                CAST(EXTRACT(YEAR FROM t."PaidOrReceivedAt") AS integer) AS "Year",
                SUM(t."Amount") AS "Expenses"
            FROM "Transaction" t
            INNER JOIN "Category" c ON t."CategoryId" = c."Id"
            WHERE t."PaidOrReceivedAt" >= (CURRENT_DATE - INTERVAL '12 months')
              AND t."PaidOrReceivedAt" <  (CURRENT_DATE + INTERVAL '1 month')
              AND t."Type" = 2
            GROUP BY t."UserId", c."Title", CAST(EXTRACT(YEAR FROM t."PaidOrReceivedAt") AS integer);
            """;

        const string incomesAndExpenses = """
            CREATE OR REPLACE VIEW "vwGetIncomesAndExpenses" AS
            SELECT
                t."UserId" AS "UserId",
                CAST(EXTRACT(MONTH FROM t."PaidOrReceivedAt") AS integer) AS "Month",
                CAST(EXTRACT(YEAR  FROM t."PaidOrReceivedAt") AS integer) AS "Year",
                SUM(CASE WHEN t."Type" = 1 THEN t."Amount" ELSE 0 END) AS "Incomes",
                SUM(CASE WHEN t."Type" = 2 THEN t."Amount" ELSE 0 END) AS "Expenses"
            FROM "Transaction" t
            WHERE t."PaidOrReceivedAt" >= (CURRENT_DATE - INTERVAL '11 months')
              AND t."PaidOrReceivedAt" <  (CURRENT_DATE + INTERVAL '1 month')
            GROUP BY t."UserId",
                     CAST(EXTRACT(MONTH FROM t."PaidOrReceivedAt") AS integer),
                     CAST(EXTRACT(YEAR  FROM t."PaidOrReceivedAt") AS integer);
            """;

        const string incomesByCategory = """
            CREATE OR REPLACE VIEW "vwGetIncomesByCategory" AS
            SELECT
                t."UserId" AS "UserId",
                c."Title"  AS "Category",
                CAST(EXTRACT(YEAR FROM t."PaidOrReceivedAt") AS integer) AS "Year",
                SUM(t."Amount") AS "Incomes"
            FROM "Transaction" t
            INNER JOIN "Category" c ON t."CategoryId" = c."Id"
            WHERE t."PaidOrReceivedAt" >= (CURRENT_DATE - INTERVAL '11 months')
              AND t."PaidOrReceivedAt" <  (CURRENT_DATE + INTERVAL '1 month')
              AND t."Type" = 1
            GROUP BY t."UserId", c."Title", CAST(EXTRACT(YEAR FROM t."PaidOrReceivedAt") AS integer);
            """;

        await context.Database.ExecuteSqlRawAsync(expensesByCategory);
        await context.Database.ExecuteSqlRawAsync(incomesAndExpenses);
        await context.Database.ExecuteSqlRawAsync(incomesByCategory);
    }
}