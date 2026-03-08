using Scalar.AspNetCore;

namespace Dima.Api.Extensions;

public static class ScalarApiDocsConfiguration
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