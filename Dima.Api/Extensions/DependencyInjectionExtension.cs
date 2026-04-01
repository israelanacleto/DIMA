using Asp.Versioning;
using Dima.Api.Common.Api;
using Dima.Api.Data;
using Dima.Api.Handlers;
using Dima.Api.Models;
using Dima.Core.Handlers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Extensions;

public static class DependencyInjectionExtension
{
    public static void AddApiInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContext(services, configuration);
        AddIdentity(services);
        AddHandlers(services);
        AddVersioning(services);

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
    }

    private static void AddVersioning(IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("api-version")
            );
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
    }
    
    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration
            .GetConnectionString("DefaultConnection") ?? string.Empty;

        services.AddDbContext<AppDbContext>(config =>
        {
            if (connectionString == "TestDb")
            {
                config.UseInMemoryDatabase("IntegrationTestDb");
            }
            else
            {
                config.UseSqlServer(connectionString);
            }
        });
    }

    private static void AddIdentity(IServiceCollection services)
    {
        services
            .AddIdentityCore<User>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;
            })
            .AddRoles<IdentityRole<long>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddApiEndpoints();
    }

    private static void AddHandlers(IServiceCollection services)
    {
        services.AddTransient<IAuditHandler, AuditHandler>();
        services.AddTransient<ICategoryHandler, CategoryHandler>();
        services.AddTransient<ITransactionHandler, TransactionalHandler>();
        services.AddTransient<IVoucherHandler, VoucherHandler>();
        services.AddTransient<IProductHandler, ProductHandler>();
        services.AddTransient<IOrderHandler, OrderHandler>();
        services.AddTransient<IProfileHandler, ProfileHandler>();
        services.AddTransient<IStripeHandler, StripeHandler>();
        services.AddTransient<IDashboardHandler, DashboardHandler>();
    }
}