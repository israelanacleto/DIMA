using Dima.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Extensions;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContext(services, configuration);
        
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
}