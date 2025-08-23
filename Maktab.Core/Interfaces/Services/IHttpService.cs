namespace Maktab.Core.Interfaces.Services
{
     public interface IHttpService : IApplicationService
     {
          Task<T> Get<T>(string uri);
          Task Post(string uri, object value);
          Task<T> Post<T>(string uri, object? value = null, bool autoLogout = true);
          Task Put(string uri, object? value = null, bool autoLogout = true);
          Task<T> Put<T>(string uri, object value);
          Task Delete(string uri);
          Task<T> Delete<T>(string uri);
     }
}
