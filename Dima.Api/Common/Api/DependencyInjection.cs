using Dima.Api.Data;
using Dima.Api.Handlers;
using Dima.Api.Models;
using Dima.Core;
using Dima.Core.Handlers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Common.Api;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        AddDbContext(services);
        AddIdentity(services);
        AddServices(services);
    }
    
    private static void AddDbContext(IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(config => 
            config
                .UseSqlServer(connectionString: Configuration.ConnectionString)
        );
    }

    private static void AddIdentity(IServiceCollection services)
    {
        services
            .AddIdentityCore<User>()
            .AddRoles<IdentityRole<long>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddApiEndpoints();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddTransient<ICategoryHandler, CategoryHandler>();
        services.AddTransient<ITransactionHandler, TransactionalHandler>();
        services.AddTransient<IProfileHandler, ProfileHandler>();
    }
}