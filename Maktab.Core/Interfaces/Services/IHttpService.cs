namespace Maktab.Core.Interfaces.Services
{
     public interface IHttpService : IApplicationService
     {
          Task<T> Get<T>(string uri, CancellationToken ct = default);
          Task Post(string uri, object value, CancellationToken cancellationToken = default);
          Task<T> Post<T>(string uri, object? value = null, bool autoLogout = true, CancellationToken cancellationToken = default);
          Task Put(string uri, object? value = null, bool autoLogout = true, CancellationToken cancellationToken = default);
          Task<T> Put<T>(string uri, object value = null, CancellationToken cancellationToken = default);
          Task Delete(string uri, CancellationToken cancellationToken = default);
          Task<T> Delete<T>(string uri, CancellationToken cancellationToken = default);
     }
}
