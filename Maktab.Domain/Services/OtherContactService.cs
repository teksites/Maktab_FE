using Maktab.Core.Interfaces.Services;

namespace Maktab.Domain.Services
{
     public class OtherContactService : BaseService, IOtherContactService
     {
          public OtherContactService(IHttpService httpService, ILocalStorageService localStorageService) : base(httpService, localStorageService)
          {
          }
     }
}
