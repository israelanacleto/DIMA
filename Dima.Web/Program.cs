using Dima.Core.Handlers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Dima.Web;
using Dima.Web.Handlers;
using Dima.Web.Security;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

Configuration.BackendUrl = builder.Configuration.GetValue<string>("BackendUrl") ?? string.Empty;
Configuration.StripePublicKey = builder.Configuration.GetValue<string>("StripePublicKey") ?? string.Empty;

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<CookieHandler>();

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();
builder.Services.AddScoped(x =>
    (ICookieAuthenticationStateProvider)x.GetRequiredService<AuthenticationStateProvider>());

builder.Services.AddMudServices();

builder.Services.AddHttpClient(Configuration.HttpClientName, options =>
{
    options.BaseAddress = new Uri(Configuration.BackendUrl);
}).AddHttpMessageHandler<CookieHandler>();

builder.Services.AddScoped<AccountHandler>();
builder.Services.AddScoped<IAccountHandler>(x => x.GetRequiredService<AccountHandler>());
builder.Services.AddScoped<IProfileHandler>(x => x.GetRequiredService<AccountHandler>());
builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();
builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
builder.Services.AddTransient<IDashboardHandler, DashboardHandler>();
builder.Services.AddTransient<IVoucherHandler, VoucherHandler>();
builder.Services.AddTransient<IProductHandler, ProductHandler>();
builder.Services.AddTransient<IOrderHandler, OrderHandler>();
builder.Services.AddTransient<IStripeHandler, StripeHandler>();

builder.Services.AddLocalization();
System.Globalization.CultureInfo.DefaultThreadCurrentCulture = new System.Globalization.CultureInfo("pt-BR");
System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = new System.Globalization.CultureInfo("pt-BR");

await builder.Build().RunAsync();
