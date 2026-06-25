using Maktab.Consumer.Theme;
using Maktab.Core.Interfaces.Services;
using MudBlazor;

namespace Maktab.Consumer.Services
{
     public class ThemeService : IThemeService
     {
          private readonly ILocalStorageService _storageService;
          private const string StorageKey = "AppTheme";

          public bool IsDarkMode { get; private set; }

          public MudTheme CurrentTheme
          {
               get; private set;
          }

          public ThemeService(ILocalStorageService storageService)
          {
               _storageService = storageService;
               CurrentTheme = AppTheme.IccBrossardTheme;
          }

          public async Task InitializeAsync()
          {
               var stored = await _storageService.GetItem<string>(StorageKey, string.Empty);
               IsDarkMode = stored == "dark";
          }


          public async Task ToggleThemeAsync()
          {
               IsDarkMode = !IsDarkMode;
               await _storageService.SetItem( StorageKey, IsDarkMode ? "dark" : "light");
          }
     }
}
