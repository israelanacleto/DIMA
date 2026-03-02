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
                FontFamily = ["Raleway", "sans-serif"]
            }
        },
        PaletteLight = new PaletteLight
        {
            Primary = "#1E293B", // Navy - Sobriedade e Confiança
            Secondary = "#64748B", // Slate - Equilíbrio
            Tertiary = "#10B981", // Emerald - Finanças/Crescimento
            Success = "#10B981", 
            Error = "#EF4444",
            Warning = "#F59E0B",
            Info = "#3B82F6",
            Background = "#F8FAFC",
            AppbarBackground = "#334155",
            AppbarText = Colors.Shades.White,
            PrimaryContrastText = Colors.Shades.White,
            DrawerBackground = Colors.Shades.White,
            DrawerText = "#1E293B"
        },
        PaletteDark = new PaletteDark
        {
            Primary = "#10B981", // Destaque em Emerald no modo dark
            Secondary = "#94A3B8",
            Tertiary = "#34D399",
            Success = "#34D399",
            Error = "#F87171",
            Warning = "#FBBF24",
            Info = "#60A5FA",
            Background = "#0F172A", // Navy mais profundo
            Surface = "#1E293B",
            AppbarBackground = "#1E293B",
            AppbarText = Colors.Shades.White,
            PrimaryContrastText = Colors.Shades.White,
            DrawerBackground = "#162033",
            DrawerText = Colors.Shades.White
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