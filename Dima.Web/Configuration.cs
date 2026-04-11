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
                FontFamily = ["Inter", "sans-serif"],
                FontSize = "0.875rem",
                FontWeight = "400",
                LineHeight = "1.6"
            },
            H1 = new H1Typography
            {
                FontFamily = ["Inter", "sans-serif"],
                FontSize = "2.5rem",
                FontWeight = "700",
                LineHeight = "1.2"
            },
            H2 = new H2Typography
            {
                FontFamily = ["Inter", "sans-serif"],
                FontSize = "2rem",
                FontWeight = "700",
                LineHeight = "1.25"
            },
            H3 = new H3Typography
            {
                FontFamily = ["Inter", "sans-serif"],
                FontSize = "1.5rem",
                FontWeight = "600",
                LineHeight = "1.3"
            },
            H4 = new H4Typography
            {
                FontFamily = ["Inter", "sans-serif"],
                FontSize = "1.25rem",
                FontWeight = "600",
                LineHeight = "1.4"
            },
            H5 = new H5Typography
            {
                FontFamily = ["Inter", "sans-serif"],
                FontSize = "1rem",
                FontWeight = "600"
            },
            H6 = new H6Typography
            {
                FontFamily = ["Inter", "sans-serif"],
                FontSize = "0.875rem",
                FontWeight = "600",
                LetterSpacing = "0.05em"
            },
            Subtitle1 = new Subtitle1Typography
            {
                FontFamily = ["Inter", "sans-serif"],
                FontSize = "1rem",
                FontWeight = "500"
            },
            Subtitle2 = new Subtitle2Typography
            {
                FontFamily = ["Inter", "sans-serif"],
                FontSize = "0.875rem",
                FontWeight = "500"
            },
            Body1 = new Body1Typography
            {
                FontFamily = ["Inter", "sans-serif"],
                FontSize = "0.9375rem",
                LineHeight = "1.6"
            },
            Body2 = new Body2Typography
            {
                FontFamily = ["Inter", "sans-serif"],
                FontSize = "0.8125rem",
                LineHeight = "1.5"
            },
            Button = new ButtonTypography
            {
                FontFamily = ["Inter", "sans-serif"],
                FontSize = "0.875rem",
                FontWeight = "600",
                LetterSpacing = "0.02em",
                TextTransform = "none"
            }
        },
        PaletteLight = new PaletteLight
        {
            Primary = "#0F2D5E",
            PrimaryContrastText = new MudColor("#FFFFFF"),
            PrimaryDarken = "#0A1E3F",
            PrimaryLighten = "#1A4A8A",
            Secondary = "#00B4D8",
            SecondaryContrastText = new MudColor("#FFFFFF"),
            Tertiary = "#10B981",
            TertiaryContrastText = new MudColor("#FFFFFF"),
            Background = "#F0F4F8",
            BackgroundGray = "#E2E8F0",
            Surface = "#FFFFFF",
            AppbarBackground = "#0F2D5E",
            AppbarText = Colors.Shades.White,
            TextPrimary = "#0A1628",
            TextSecondary = "#64748B",
            TextDisabled = "#94A3B8",
            DrawerText = "#E2E8F0",
            DrawerBackground = "#0A1628",
            DrawerIcon = "#94A3B8",
            Divider = "rgba(0, 180, 216, 0.15)",
            DividerLight = "rgba(15, 45, 94, 0.08)",
            Success = "#10B981",
            SuccessContrastText = new MudColor("#FFFFFF"),
            Error = "#F43F5E",
            ErrorContrastText = new MudColor("#FFFFFF"),
            Warning = "#F59E0B",
            WarningContrastText = new MudColor("#FFFFFF"),
            Info = "#00B4D8",
            InfoContrastText = new MudColor("#FFFFFF"),
            ActionDefault = "#64748B",
            ActionDisabled = "#CBD5E1",
            LinesDefault = "rgba(0, 180, 216, 0.12)",
            TableLines = "rgba(15, 45, 94, 0.08)",
            OverlayLight = "rgba(15, 45, 94, 0.04)",
            OverlayDark = "rgba(10, 22, 40, 0.6)"
        },
        PaletteDark = new PaletteDark
        {
            Primary = "#4F8CC9",
            PrimaryContrastText = new MudColor("#FFFFFF"),
            PrimaryDarken = "#3A6FA0",
            PrimaryLighten = "#6BAAD8",
            Secondary = "#00B4D8",
            SecondaryContrastText = new MudColor("#FFFFFF"),
            Tertiary = "#10B981",
            TertiaryContrastText = new MudColor("#FFFFFF"),
            Background = "#0A1628",
            BackgroundGray = "#070E1C",
            Surface = "#0F1E35",
            AppbarBackground = "#070E1C",
            AppbarText = Colors.Shades.White,
            TextPrimary = "#F0F4F8",
            TextSecondary = "#94A3B8",
            TextDisabled = "#475569",
            DrawerText = "#CBD5E1",
            DrawerBackground = "#070E1C",
            DrawerIcon = "#64748B",
            Divider = "rgba(0, 180, 216, 0.12)",
            DividerLight = "rgba(240, 244, 248, 0.06)",
            Success = "#10B981",
            SuccessContrastText = new MudColor("#FFFFFF"),
            Error = "#F43F5E",
            ErrorContrastText = new MudColor("#FFFFFF"),
            Warning = "#F59E0B",
            WarningContrastText = new MudColor("#FFFFFF"),
            Info = "#00B4D8",
            InfoContrastText = new MudColor("#FFFFFF"),
            ActionDefault = "#94A3B8",
            ActionDisabled = "#334155",
            LinesDefault = "rgba(0, 180, 216, 0.1)",
            TableLines = "rgba(240, 244, 248, 0.06)",
            OverlayLight = "rgba(240, 244, 248, 0.03)",
            OverlayDark = "rgba(7, 14, 28, 0.8)"
        }
    };

    public static string[] ChartPalette { get; set; } =
    [
        "#00B4D8",
        "#10B981",
        "#F59E0B",
        "#F43F5E",
        "#4F8CC9",
        "#8B5CF6",
        "#06B6D4",
        "#84CC16",
        "#FB923C",
        "#EC4899",
        "#14B8A6",
        "#A78BFA",
        "#34D399",
        "#FCD34D",
        "#F87171"
    ];

    public static string[] IncomesExpensesPalette { get; set; } =
    [
        "#10B981",
        "#EF4444"
    ];
}