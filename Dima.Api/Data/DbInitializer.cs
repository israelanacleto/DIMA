using Dima.Api.Data;
using Dima.Api.Models;
using Dima.Core.Enums;
using Dima.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await SeedProductsAsync(context);
    }

    private static async Task SeedProductsAsync(AppDbContext context)
    {
        try
        {
            if (await context.Products.AnyAsync())
            {
                Console.WriteLine("Products already exist. Skipping seed.");
                return;
            }

            Console.WriteLine("Seeding products...");
            var products = new List<Product>
            {
                new()
                {
                    Title = "Dima Lite",
                    Description = "Ideal para quem está começando a organizar as finanças.",
                    Slug = "dima-lite",
                    IsActive = true,
                    Price = 0,
                    SubscriptionDurationInDays = 365,
                    Benefits = new List<string> { "Até 50 lançamentos mensais", "Gráficos básicos", "Suporte via e-mail" }
                },
                new()
                {
                    Title = "Dima Premium Elite",
                    Description = "A experiência completa para quem busca liberdade financeira.",
                    Slug = "dima-premium-elite",
                    IsActive = true,
                    Price = 29.90m,
                    SubscriptionDurationInDays = 30,
                    Benefits = new List<string> { "Lançamentos ilimitados", "Dashboards avançados", "Suporte prioritário", "Exportação de dados" }
                }
            };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
            Console.WriteLine("Products seeded successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to seed products: {ex.Message}");
        }
    }

    /// <summary>
    /// Cria (se não existir) a conta de demonstração e popula 6 meses de finanças,
    /// para o dashboard e os relatórios nascerem com dados realistas. Idempotente.
    /// Credenciais: demo@dima.app / Demo@123
    /// </summary>
    public static async Task SeedDemoAsync(AppDbContext context, UserManager<User> userManager)
    {
        const string demoEmail = "demo@dima.app";

        try
        {
            if (await userManager.FindByEmailAsync(demoEmail) is null)
            {
                var user = new User
                {
                    UserName = demoEmail,
                    Email = demoEmail,
                    EmailConfirmed = true,
                    Name = "Conta Demo"
                };

                var result = await userManager.CreateAsync(user, "Demo@123");
                if (!result.Succeeded)
                {
                    Console.WriteLine("Failed to create demo user: " +
                        string.Join("; ", result.Errors.Select(e => e.Description)));
                    return;
                }

                Console.WriteLine("Demo user created: demo@dima.app / Demo@123");
            }

            await SeedDemoDataAsync(context, demoEmail);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to seed demo data: {ex.Message}");
        }
    }

    /// <summary>
    /// Popula 6 meses de finanças (categorias + lançamentos) para um usuário.
    /// Chamado no cadastro (todo novo usuário já nasce com dados) e pela conta demo.
    /// Idempotente: não refaz se o usuário já tiver categorias.
    /// </summary>
    public static async Task SeedDemoDataAsync(AppDbContext context, string userId)
    {
        // O UserId gravado nas transações/categorias é o UserName (o que
        // user.Identity.Name devolve), por isso usamos o e-mail aqui.
        if (await context.Categories.AnyAsync(x => x.UserId == userId))
            return;

        Console.WriteLine("Seeding demo finances (6 months)...");

        var names = new[]
        {
            "Salário", "Freelance", "Investimentos", "Moradia", "Alimentação",
            "Transporte", "Saúde", "Lazer", "Educação", "Outros"
        };

        var categories = names.ToDictionary(
            n => n,
            n => new Category { Title = n, Description = n, UserId = userId });

        await context.Categories.AddRangeAsync(categories.Values);
        await context.SaveChangesAsync();

        var rnd = new Random(2026);
        var today = DateTime.Now;
        var transactions = new List<Transaction>();

        DateTime DayIn(int monthsAgo, int day)
        {
            var first = new DateTime(today.Year, today.Month, 1).AddMonths(-monthsAgo);
            var clamped = Math.Min(day, DateTime.DaysInMonth(first.Year, first.Month));
            var date = new DateTime(first.Year, first.Month, clamped, 12, 0, 0);
            return monthsAgo == 0 && date > today ? today : date;
        }

        decimal Vary(decimal value, double pct)
            => Math.Round(value * (decimal)(1 + (rnd.NextDouble() * 2 - 1) * pct), 2);

        void Add(int monthsAgo, int day, string title, ETransactionType type, decimal amount, string category)
        {
            // Despesa (Withdrawal) é gravada com valor negativo, como o app faz.
            var signed = type == ETransactionType.Withdrawal ? -Math.Abs(amount) : Math.Abs(amount);
            var when = DayIn(monthsAgo, day);
            transactions.Add(new Transaction
            {
                Title = title,
                Type = type,
                Amount = signed,
                CategoryId = categories[category].Id,
                UserId = userId,
                CreatedAt = when,
                PaidOrReceivedAt = when
            });
        }

        for (var m = 5; m >= 0; m--)
        {
            // Entradas
            Add(m, 5, "Salário", ETransactionType.Deposit, Vary(6500m, 0.04), "Salário");
            if (rnd.NextDouble() < 0.5)
                Add(m, 18, "Projeto freelance", ETransactionType.Deposit, Vary(1200m, 0.4), "Freelance");
            if (rnd.NextDouble() < 0.6)
                Add(m, 12, "Dividendos", ETransactionType.Deposit, Vary(280m, 0.5), "Investimentos");

            // Saídas
            Add(m, 8, "Aluguel", ETransactionType.Withdrawal, 1850m, "Moradia");
            Add(m, 3, "Supermercado", ETransactionType.Withdrawal, Vary(620m, 0.2), "Alimentação");
            Add(m, 20, "Restaurantes e delivery", ETransactionType.Withdrawal, Vary(310m, 0.3), "Alimentação");
            Add(m, 6, "Transporte e combustível", ETransactionType.Withdrawal, Vary(380m, 0.25), "Transporte");
            Add(m, 14, "Plano de saúde", ETransactionType.Withdrawal, 240m, "Saúde");
            Add(m, 22, "Streaming e lazer", ETransactionType.Withdrawal, Vary(260m, 0.4), "Lazer");
            Add(m, 10, "Curso online", ETransactionType.Withdrawal, Vary(220m, 0.5), "Educação");
            if (rnd.NextDouble() < 0.7)
                Add(m, 25, "Despesas diversas", ETransactionType.Withdrawal, Vary(190m, 0.5), "Outros");
        }

        await context.Transactions.AddRangeAsync(transactions);
        await context.SaveChangesAsync();

        Console.WriteLine($"Demo finances seeded: {categories.Count} categories, {transactions.Count} transactions.");
    }
}