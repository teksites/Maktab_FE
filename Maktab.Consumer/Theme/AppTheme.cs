namespace Maktab.Consumer.Theme
{
     using MudBlazor;

     public static class AppTheme
     {
          public static MudTheme IccBrossardTheme => new MudTheme
          {
               PaletteLight = new PaletteLight()
               {
                    Primary = "#2A8C7A",
                    PrimaryDarken = "#1F6B5D",
                    PrimaryLighten = "#B3DDD5",
                    PrimaryContrastText = "#FFFFFF",

                    Secondary = "#D4871A",
                    SecondaryDarken = "#A86510",
                    SecondaryLighten = "#FAC775",
                    SecondaryContrastText = "#FFFFFF",

                    Background = "#FAF7F2",
                    BackgroundGray = "#F0EBE0",
                    Surface = "#FFFFFF",
                    DrawerBackground = "#FAF7F2",
                    AppbarBackground = "#1F6B5D",
                    AppbarText = "#FFFFFF",

                    TextPrimary = "#2C2416",
                    TextSecondary = "#7A6E60",
                    TextDisabled = "rgba(44,36,22,0.38)",

                    Divider = "#E0D5C3",
                    TableLines = "#E0D5C3",
                    LinesDefault = "#E0D5C3",
                    LinesInputs = "#B8A98E",

                    ActionDefault = "#7A6E60",
                    ActionDisabled = "rgba(44,36,22,0.26)",
                    ActionDisabledBackground = "rgba(44,36,22,0.12)",

                    Success = "#2A8C7A",
                    Warning = "#D4871A",
                    Error = "#C0392B",
                    Info = "#2980B9",
               },

               PaletteDark = new PaletteDark()
               {
                    Primary = "#3AADA0",
                    PrimaryDarken = "#2A8C7A",
                    PrimaryLighten = "#6ED4C8",
                    PrimaryContrastText = "#0D2420",

                    Secondary = "#F0A030",
                    SecondaryDarken = "#D4871A",
                    SecondaryLighten = "#FAB84A",
                    SecondaryContrastText = "#1A1209",

                    Background = "#1A1510",
                    BackgroundGray = "#221D16",
                    Surface = "#2A2318",
                    DrawerBackground = "#1E1A14",
                    AppbarBackground = "#164E45",
                    AppbarText = "#F0EBE0",

                    TextPrimary = "#F0EBE0",
                    TextSecondary = "#A89E90",
                    TextDisabled = "rgba(240,235,224,0.38)",

                    Divider = "#362E20",
                    TableLines = "#362E20",
                    LinesDefault = "#362E20",
                    LinesInputs = "#5C503E",

                    ActionDefault = "#A89E90",
                    ActionDisabled = "rgba(240,235,224,0.26)",
                    ActionDisabledBackground = "rgba(240,235,224,0.12)",

                    Success = "#3AADA0",
                    Warning = "#F0A030",
                    Error = "#E74C3C",
                    Info = "#3498DB",
               },

               Typography = new Typography()
               {
                    Default = new DefaultTypography()
                    {
                         FontFamily = new[] { "Inter", "Segoe UI", "sans-serif" },
                         FontSize = "0.875rem",
                         FontWeight = "400",
                         LineHeight = "1.6",
                    },
                    H5 = new H5Typography() { FontWeight = "600", FontSize = "1.15rem" },
                    H6 = new H6Typography() { FontWeight = "600", FontSize = "1rem" },
                    Body1 = new Body1Typography() { FontSize = "0.9rem", LineHeight = "1.65" },
                    Body2 = new Body2Typography() { FontSize = "0.8rem", LineHeight = "1.55" },
                    Button = new ButtonTypography()
                    {
                         FontSize = "0.875rem",
                         FontWeight = "500",
                         TextTransform = "none",
                    },
                    Caption = new CaptionTypography() { FontSize = "0.75rem" },
               },

               LayoutProperties = new LayoutProperties()
               {
                    DefaultBorderRadius = "10px",
                    DrawerWidthLeft = "260px",
                    DrawerWidthRight = "260px",
                    AppbarHeight = "60px",
               },
          };
     }
}
