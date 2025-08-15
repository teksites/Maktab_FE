namespace Maktab.Infrastructure.Localization
{
     public static class LocalizationConstants
     {
          public static readonly LanguageCode[] SupportedLanguages = {
            new LanguageCode
            {
                Code = "en-US",
                DisplayName= "English"
            },
            new LanguageCode
            {
                Code = "fr-FR",
                DisplayName = "French"
            },
            new LanguageCode
            {
                Code = "ar",
                DisplayName = "عربي"
            }
        };
     }
}
