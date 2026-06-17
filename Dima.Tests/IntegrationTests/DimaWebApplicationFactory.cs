using System.Linq;
using Dima.Api.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dima.Tests.IntegrationTests;

public class DimaWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // Substitui o AppDbContext (Npgsql) por um banco em memória,
            // independente de configuração. Antes os testes dependiam do
            // override "TestDb" via ConfigureAppConfiguration, que NÃO vence
            // o appsettings.json no minimal hosting — então o app tentava
            // conectar no Postgres real (localhost) e os testes falhavam.
            //
            // Remove tudo relacionado às opções do DbContext (incluindo o
            // IDbContextOptionsConfiguration<AppDbContext>, onde o AddDbContext
            // do app registra o provider Npgsql). Sem isso, Npgsql e InMemory
            // coexistiriam ("Only a single database provider can be registered").
            var toRemove = services
                .Where(d => d.ServiceType == typeof(AppDbContext)
                         || (d.ServiceType.FullName?.Contains("DbContextOptions") ?? false))
                .ToList();

            foreach (var descriptor in toRemove)
                services.Remove(descriptor);

            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("IntegrationTestDb"));
        });
    }
}
