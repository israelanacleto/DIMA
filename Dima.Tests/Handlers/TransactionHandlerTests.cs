using Dima.Api.Data;
using Dima.Api.Handlers;
using Dima.Core.Enums;
using Dima.Core.Requests.Transactions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Dima.Tests.Handlers;

public class TransactionHandlerTests
{
    private AppDbContext GetDatabaseContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        var databaseContext = new AppDbContext(options);
        databaseContext.Database.EnsureCreated();
        return databaseContext;
    }

    [Fact]
    public async Task CreateAsync_ShouldNormalizeAmountToNegative_WhenTypeIsWithdrawal()
    {
        // Arrange
        var context = GetDatabaseContext();
        var handler = new TransactionalHandler(context);
        var request = new CreateTransactionRequest
        {
            UserId = "test@user.com",
            Title = "Café",
            Amount = 15.50m, // Valor positivo
            Type = ETransactionType.Withdrawal, // Mas tipo saída
            CategoryId = 1,
            PaidOrReceivedAt = DateTime.Now
        };

        // Act
        var result = await handler.CreateAsync(request);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data!.Amount.Should().Be(-15.50m); // Verificando normalização
    }

    [Fact]
    public async Task CreateAsync_ShouldKeepAmountPositive_WhenTypeIsDeposit()
    {
        // Arrange
        var context = GetDatabaseContext();
        var handler = new TransactionalHandler(context);
        var request = new CreateTransactionRequest
        {
            UserId = "test@user.com",
            Title = "Salário",
            Amount = 5000m,
            Type = ETransactionType.Deposit,
            CategoryId = 1,
            PaidOrReceivedAt = DateTime.Now
        };

        // Act
        var result = await handler.CreateAsync(request);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data!.Amount.Should().Be(5000m);
    }

    [Fact]
    public async Task GetByPeriodAsync_ShouldReturnTransactionsWithinDates()
    {
        // Arrange
        var context = GetDatabaseContext();
        var handler = new TransactionalHandler(context);
        var userId = "test@user.com";
        var baseDate = new DateTime(2026, 03, 15);
        
        // Seed transactions
        context.Transactions.AddRange(new List<Dima.Core.Models.Transaction>
        {
            new() { Title = "T1", Amount = 10, Type = ETransactionType.Deposit, PaidOrReceivedAt = baseDate.AddDays(-5), UserId = userId, CategoryId = 1 },
            new() { Title = "T2", Amount = 20, Type = ETransactionType.Deposit, PaidOrReceivedAt = baseDate, UserId = userId, CategoryId = 1 },
            new() { Title = "T3", Amount = 30, Type = ETransactionType.Deposit, PaidOrReceivedAt = baseDate.AddDays(20), UserId = userId, CategoryId = 1 }
        });
        await context.SaveChangesAsync();

        var request = new GetTransactionsByPeriodRequest
        {
            UserId = userId,
            StartDate = new DateTime(2026, 03, 01),
            EndDate = new DateTime(2026, 03, 31, 23, 59, 59)
        };

        // Act
        var result = await handler.GetByPeriodAsync(request);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(2); 
        result.Data!.Should().Contain(x => x.Title == "T1");
        result.Data!.Should().Contain(x => x.Title == "T2");
        result.Data!.Should().NotContain(x => x.Title == "T3");
    }
}