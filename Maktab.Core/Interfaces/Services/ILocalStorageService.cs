namespace Maktab.Core.Interfaces.Services
{
     public interface ILocalStorageService : IApplicationService
     {
          Task<T> GetItem<T>(string key, T defaultVaue = default);
          Task SetItem<T>(string key, T value);
          Task RemoveItem(string key);
     }
}
