
using System.Globalization;

namespace Maktab.Core.Interfaces.Services
{
     public interface IGlobalizationService : IApplicationService
     {
          CultureInfo CurrentCulture { get; }

          bool ApplyCultureOnUI(string culture);
          Task<string?> GetBrowserLocale();
          Task<string> GetPersistedCultureName();
          string MapToSupportedCulture(string? rawCulture);
          Task SaveCultureAsync(string culture);
     }
}
