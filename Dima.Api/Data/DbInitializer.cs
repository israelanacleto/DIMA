using Dima.Api.Data;
using Dima.Api.Models;
using Dima.Core.Enums;
using Dima.Core.Models;
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

    public static async Task SeedDemoDataAsync(AppDbContext context, string userId)
    {
        if (await context.Categories.AnyAsync(x => x.UserId == userId))
            return;

        var categories = new List<Category>
        {
            new() { Title = "Salário", Description = "Renda principal", UserId = userId },
            new() { Title = "Investimentos", Description = "Dividendos e Juros", UserId = userId },
            new() { Title = "Aluguel", Description = "Moradia", UserId = userId },
            new() { Title = "Alimentação", Description = "Supermercado e Restaurantes", UserId = userId },
            new() { Title = "Lazer", Description = "Cinema, Viagens, etc", UserId = userId },
            new() { Title = "Saúde", Description = "Farmácia e Convênio", UserId = userId }
        };

        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();

        var salaryCat = categories[0];
        var rentCat = categories[2];
        var foodCat = categories[3];

        var transactions = new List<Transaction>
        {
            new() { Title = "Salário Mensal", Amount = 5000, Type = ETransactionType.Deposit, CategoryId = salaryCat.Id, PaidOrReceivedAt = DateTime.Now.AddDays(-5), UserId = userId },
            new() { Title = "Pagamento Aluguel", Amount = 1200, Type = ETransactionType.Withdrawal, CategoryId = rentCat.Id, PaidOrReceivedAt = DateTime.Now.AddDays(-3), UserId = userId },
            new() { Title = "Jantar", Amount = 150, Type = ETransactionType.Withdrawal, CategoryId = foodCat.Id, PaidOrReceivedAt = DateTime.Now.AddDays(-1), UserId = userId }
        };

        await context.Transactions.AddRangeAsync(transactions);
        await context.SaveChangesAsync();
    }
}