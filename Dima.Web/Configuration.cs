using MudBlazor;
using MudBlazor.Utilities;

namespace Dima.Web;

public static class Configuration
{
    public const string HttpClientName = "dima";

    public static string BackendUrl { get; set; } = "http://localhost:5204";
    public static string StripePublicKey { get; set; } = "";
    
    public static readonly MudTheme Theme = new()
    {
        Typography = new Typography
        {
            Default = new DefaultTypography
            {
                FontFamily = ["Inter", "Raleway", "sans-serif"],
                FontSize = "0.875rem",
                FontWeight = 400
            },
            H4 = new H4
            {
                FontFamily = ["Inter", "Raleway", "sans-serif"],
                FontSize = "2rem",
                FontWeight = 600
            }
        },
        PaletteLight = new PaletteLight
        {
            Primary = "#6366F1",
            PrimaryContrastText = new MudColor("#FFFFFF"),
            Secondary = "#8B5CF6",
            Background = "#F5F5F5",
            AppbarBackground = "#6366F1",
            AppbarText = Colors.Shades.White,
            TextPrimary = Colors.Shades.Black,
            DrawerText = Colors.Shades.White,
            DrawerBackground = "#0F172A"
        },
        PaletteDark = new PaletteDark
        {
            Primary = "#818CF8",
            Secondary = "#8B5CF6",
            // Background = Colors.LightGreen.Darken4,
            AppbarBackground = "#818CF8",
            AppbarText = Colors.Shades.Black,
            PrimaryContrastText = new MudColor("#000000")
        }
    };

    public static string[] ChartPalette { get; set; } =
    [
        "#594AE2",
        "#00C853",
        "#FFAB00",
        "#FF4081",
        "#00B0FF",
        "#FF3D00",
        "#8E24AA",
        "#00E5FF",
        "#D4E157",
        "#FF6E40",
        "#B388FF",
        "#1DE9B6",
        "#EF6C00",
        "#00E676",
        "#E91E63"
    ];

    public static string[] IncomesExpensesPalette { get; set; } =
    [
        "#10B981",
        "#EF4444"
    ];
}