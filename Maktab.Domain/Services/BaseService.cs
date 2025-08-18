using Maktab.Core.Interfaces.Services;

namespace Maktab.Domain.Services
{
     public abstract class BaseService
     {
          protected IHttpService _httpService;
          protected ILocalStorageService _localStorageService;


          public BaseService(IHttpService httpService, ILocalStorageService localStorageService)
          {
               _httpService = httpService;
               _localStorageService = localStorageService;
          }
     }
}
