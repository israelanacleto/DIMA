using Dima.Api.Data;
using Dima.Api.Handlers;
using Dima.Core.Handlers;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Extensions;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContext(services, configuration);
        AddServices(services);
    }
    
    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration
            .GetConnectionString("DefaultConnection") ?? string.Empty;
        
        services.AddDbContext<AppDbContext>(config => 
            config
                .UseSqlServer(connectionString: connectionString)
        );
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddTransient<ICategoryHandler, CategoryHandler>();
    }
}