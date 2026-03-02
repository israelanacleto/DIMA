using Scalar.AspNetCore;

namespace Dima.Api.Common.Api;

public static class ScalarApiDocsConfig
{
    public static void AddScalarConfig (this WebApplication app)
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options.Title = "Dima API";
            options.ForceDarkMode();
            options.Theme = ScalarTheme.BluePlanet;
            options.PreserveSchemaPropertyOrder();
            options.SortOperationsByMethod();
            options.ShowDeveloperTools = DeveloperToolsVisibility.Localhost;
        });
    } 
}