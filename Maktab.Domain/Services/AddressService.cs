using Maktab.Core.Interfaces.Services;

namespace Maktab.Domain.Services
{
     public class AddressService : BaseService, IAddressService
     {
          public AddressService(IHttpService httpService, ILocalStorageService localStorageService) 
          : base(httpService, localStorageService)
          {
          }
     }
}
