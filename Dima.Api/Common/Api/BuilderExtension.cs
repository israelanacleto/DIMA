using Dima.Core;
using Microsoft.AspNetCore.Identity;
using Scalar.AspNetCore;
using Stripe;

namespace Dima.Api.Common.Api;

public static class BuilderExtension
{
    extension(WebApplicationBuilder builder)
    {
        public void AddConfiguration()
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

        public void AddDocs()
        {
            builder
                .Services
                .AddOpenApi(options => options.AddScalarTransformers());
        }

        public void AddSecurity()
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

        public void AddInfrastructure()
        {
            builder
                .Services
                .AddInfrastructure();
        }

        public void AddCrossOrigin()
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
}