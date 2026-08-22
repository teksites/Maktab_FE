using Maktab.Core.Interfaces.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Globalization;

namespace Maktab.Consumer.Services
{
     public class GlobalizationService : IGlobalizationService
     {
          private const string CultureKey = "AppCulture";
          private const string defaultCulture = "fr";
          private readonly ILocalStorageService _localStorageService;
          private readonly NavigationManager _navigationManager;
          private readonly IJSRuntime _jsInterop;
          private readonly IReadOnlyCollection<string> _supportedCultures;


          public GlobalizationService(IJSRuntime jsInterop, ILocalStorageService localStorage, NavigationManager navigationManager)
          {
               _jsInterop = jsInterop;
               _localStorageService = localStorage;
               _navigationManager = navigationManager;
               _supportedCultures = new List<string>() { "en", defaultCulture };
          }

          public CultureInfo CurrentCulture
          {
               get
               {
                    return CultureInfo.CurrentUICulture;
               }
          }

          public async Task SaveCultureAsync(string culture)
          {
               await _localStorageService.SetItem(CultureKey, culture);
              
               ApplyCultureOnUI(culture);

               // force a full page reload to ensure all components pick new culture
               _navigationManager.NavigateTo(_navigationManager.Uri, forceLoad: true);
          }

          public async Task<string?> GetBrowserLocale()
          {
               string? browserLocale;
               try
               {
                    browserLocale = await _jsInterop.InvokeAsync<string>("getBrowserLocale");
               }
               catch (JSException)
               {
                    // JS interop failed (e.g. navigator.language unavailable) — nothing to fall back on here
                    return null;
               }

               if (string.IsNullOrEmpty(browserLocale))
                    return null;

               try
               {
               var browserCulture = new CultureInfo(browserLocale);
               // Extract the two-letter ISO language name
               var twoLetterCode = browserCulture.TwoLetterISOLanguageName.ToLower();
               return twoLetterCode;
          }
               catch (CultureNotFoundException)
               {
                    // navigator.language returned something .NET's ICU data doesn't recognize
                    return null;
               }
          }

          public async Task<string> GetPersistedCultureName()
          {
               var cultureName = await _localStorageService.GetItem<string>(CultureKey);
               return cultureName ?? string.Empty;
          }

          public bool ApplyCultureOnUI(string culture)
          {
               if (!string.IsNullOrEmpty(culture))
               {
                    // update .NET cultures
                    var ci = new CultureInfo(culture);

                    //CultureInfo.CurrentCulture =
                   // CultureInfo.CurrentUICulture =

                    CultureInfo.DefaultThreadCurrentCulture =
                    CultureInfo.DefaultThreadCurrentUICulture = ci;
               }
               else
               {
                    //CultureInfo.CurrentCulture =
                    //CultureInfo.CurrentUICulture =
                    // Default culture (can be configured via appsettings)
                    CultureInfo.DefaultThreadCurrentCulture =
                    CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(defaultCulture);
               }

               return true;
          }

          public string MapToSupportedCulture(string? rawCulture)
          {
               if (string.IsNullOrEmpty(rawCulture))
                    return defaultCulture;

               var match = _supportedCultures.FirstOrDefault(s =>
                   rawCulture.StartsWith(s, StringComparison.OrdinalIgnoreCase));

               return match ?? defaultCulture;
          }
     }
}
