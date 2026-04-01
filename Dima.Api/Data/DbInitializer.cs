using Dima.Api.Data;
using Dima.Api.Models;
using Dima.Core.Enums;
using Dima.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Data;

public static class DbInitializer
{
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