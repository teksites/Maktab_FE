using Maktab.Core.Interfaces.Models;
using Maktab.Core.Interfaces.Settings;

namespace Maktab.Core.Interfaces.Managers
{
     public interface IPreferenceManager
     {
          Task SetPreference(IPreference preference);

          Task<IPreference> GetPreference();

          Task<IResult> ChangeLanguageAsync(string languageCode);
     }
}
