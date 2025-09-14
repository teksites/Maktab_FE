using Maktab.Core.Interfaces.Services;

namespace Maktab.Domain.Services
{
     public class ContactService : BaseService, IContactService
     {
          public ContactService(IHttpService httpService, ILocalStorageService localStorageService) : base(httpService, localStorageService)
          {
          }
     }
}
