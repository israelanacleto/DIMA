using Dima.Api.Data;
using Dima.Api.Models;
using Dima.Core.Enums;
using Dima.Core.Models;
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
            // 1. Automatically Apply Migrations (including Views)
            context.Database.Migrate();
            
            // 2. Seed Demo Data if empty
            SeedInitialData(context);
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

    private static void SeedInitialData(AppDbContext context)
    {
        if (context.Categories.Any()) return;

        // Note: For a true "Senior" portfolio, you might want to link this 
        // to the first registered user. For now, we seed global demo categories.
        var demoUser = "demo@dima.com";
        
        var categories = new List<Category>
        {
            new() { Title = "Salário", Description = "Renda principal", UserId = demoUser },
            new() { Title = "Investimentos", Description = "Dividendos e Juros", UserId = demoUser },
            new() { Title = "Aluguel", Description = "Moradia", UserId = demoUser },
            new() { Title = "Alimentação", Description = "Supermercado e Restaurantes", UserId = demoUser },
            new() { Title = "Lazer", Description = "Cinema, Viagens, etc", UserId = demoUser },
            new() { Title = "Saúde", Description = "Farmácia e Convênio", UserId = demoUser }
        };

        context.Categories.AddRange(categories);
        context.SaveChanges();

        // Let's add some dummy transactions to make the dashboard alive immediately
        if (!context.Transactions.Any())
        {
            var salaryCat = categories[0];
            var rentCat = categories[2];
            var foodCat = categories[3];

            var transactions = new List<Transaction>
            {
                new() { Title = "Salário Mensal", Amount = 5000, Type = ETransactionType.Deposit, CategoryId = salaryCat.Id, PaidOrReceivedAt = DateTime.Now.AddDays(-5), UserId = demoUser },
                new() { Title = "Pagamento Aluguel", Amount = 1200, Type = ETransactionType.Withdrawal, CategoryId = rentCat.Id, PaidOrReceivedAt = DateTime.Now.AddDays(-3), UserId = demoUser },
                new() { Title = "Jantar", Amount = 150, Type = ETransactionType.Withdrawal, CategoryId = foodCat.Id, PaidOrReceivedAt = DateTime.Now.AddDays(-1), UserId = demoUser }
            };

            context.Transactions.AddRange(transactions);
            context.SaveChanges();
        }
    }
}