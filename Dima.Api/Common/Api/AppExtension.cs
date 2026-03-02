namespace Dima.Api.Common.Api;

public static class AppExtension
{
    extension(WebApplication app)
    {
        public void ConfigureDevEnvironment()
        {
            if (app.Environment.IsDevelopment())
            {
                app.AddScalarConfig();
            }
        }

        public void UseSecurity()
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}