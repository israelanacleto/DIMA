using Dima.Api.Data;
using Dima.Api.Handlers;
using Dima.Core.Requests.Categories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Dima.Tests.Handlers;

public class CategoryHandlerTests
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
    public async Task CreateAsync_ShouldReturnSuccess_WhenRequestIsValid()
    {
        // Arrange
        var context = GetDatabaseContext();
        var handler = new CategoryHandler(context);
        var request = new CreateCategoryRequest
        {
            UserId = "test@user.com",
            Title = "Lazer",
            Description = "Gastos com cinema e viagens"
        };

        // Act
        var result = await handler.CreateAsync(request);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Code.Should().Be(201);
        result.Data.Should().NotBeNull();
        result.Data!.Title.Should().Be("Lazer");
        
        // Verify database persistence
        var categoryInDb = await context.Categories.FirstOrDefaultAsync(x => x.Id == result.Data.Id);
        categoryInDb.Should().NotBeNull();
        categoryInDb!.Title.Should().Be("Lazer");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        var context = GetDatabaseContext();
        var handler = new CategoryHandler(context);
        var request = new GetCategoryByIdRequest
        {
            Id = 999,
            UserId = "test@user.com"
        };

        // Act
        var result = await handler.GetByIdAsync(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Code.Should().Be(404);
        result.Message.Should().Be("Categoria não encontrada");
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnSuccess_WhenCategoryExists()
    {
        // Arrange
        var context = GetDatabaseContext();
        var handler = new CategoryHandler(context);
        
        // Seed a category
        var requestCreate = new CreateCategoryRequest { UserId = "test@user.com", Title = "Comida", Description = "Teste" };
        var created = await handler.CreateAsync(requestCreate);
        
        var requestDelete = new DeleteCategoryRequest
        {
            Id = created.Data!.Id,
            UserId = "test@user.com"
        };

        // Act
        var result = await handler.DeleteAsync(requestDelete);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be("Categoria excluída com sucesso!");
        
        // Verify database removal
        var exists = await context.Categories.AnyAsync(x => x.Id == created.Data.Id);
        exists.Should().BeFalse();
    }
}