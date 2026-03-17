using MudBlazor;
using MudBlazor.Utilities;

namespace Dima.Web;

public static class Configuration
{
    public const string HttpClientName = "dima";

    public static string BackendUrl { get; set; } = "http://localhost:5204";
    
    public static MudTheme Theme = new()
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
            DrawerBackground = "#0F172A",
            DrawerText = Colors.Shades.White
        }
    };
}