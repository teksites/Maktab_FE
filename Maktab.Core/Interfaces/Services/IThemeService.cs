
namespace Maktab.Core.Interfaces.Services
{
     public interface IThemeService : IApplicationService
     {
          bool IsDarkMode { get; }

          Task ToggleThemeAsync();
     }
}
