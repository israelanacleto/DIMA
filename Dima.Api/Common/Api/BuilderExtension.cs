using Dima.Api.Extensions;
using Dima.Core;
using Microsoft.AspNetCore.Identity;
using Scalar.AspNetCore;
using Stripe;

namespace Dima.Api.Common.Api;

public static class BuilderExtension
{
    public static void AddConfiguration(this WebApplicationBuilder builder)
    {
        Configuration.ConnectionString = 
            builder
                .Configuration
                .GetConnectionString("DefaultConnection") 
            ?? string.Empty;
        
        Configuration.BackendUrl = 
            builder
                .Configuration
                .GetValue<string>("BackendUrl") 
            ?? string.Empty;
        
        Configuration.FrontendUrl = 
            builder
                .Configuration
                .GetValue<string>("FrontendUrl") 
            ?? string.Empty;
        
        ApiConfiguration.StripeApiKey = 
            builder
                .Configuration
                .GetValue<string>("StripeApiKey") 
            ?? string.Empty;
        
        StripeConfiguration.ApiKey = ApiConfiguration.StripeApiKey;
    }

    public static void AddDocs(this WebApplicationBuilder builder)
    {
        builder
            .Services
            .AddOpenApi(options => options.AddScalarTransformers());
    }

    public static void AddSecurity(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddAuthentication(IdentityConstants.ApplicationScheme)
            .AddIdentityCookies();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        });

        builder.Services.AddAuthorization();
    }

    public static void AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder
            .Services
            .AddApiInfrastructure(builder.Configuration);
    }

    public static void AddCrossOrigin(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options => options.AddPolicy(
            ApiConfiguration.CorsPolicyName,
            policy => policy
                .WithOrigins(Configuration.BackendUrl, Configuration.FrontendUrl)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
        ));
    }
}