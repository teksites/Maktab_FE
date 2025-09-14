using Maktab.Core.Interfaces.Services;
using MaktabDataContracts.Requests.Addresses;
using MaktabDataContracts.Responses.Addresses;

namespace Maktab.Domain.Services
{
     public class AddressService : BaseService, IAddressService
     {
          private const string getAddressById = @"/api/address/{0}";
          private const string getAddressByConnectedId = @"/api/connectedid/{0}/address";
          private const string saveAddress = @"/api/user/address/add";
          private const string updateAddress = @"/api/address/update";
          private const string deleteAddressById = @"/api/address/{0}/delete";
          private const string deleteAddressByConnectedId = @"/api/connectedids/{0}/address/delete";

          public AddressService(IHttpService httpService, ILocalStorageService localStorageService) 
          : base(httpService, localStorageService)
          {
          }

          public async Task<AddressResponse> GetAddressById(Guid addressId)
          {
               var formatedUrl = string.Format(getAddressById, addressId);
               var result = await _httpService.Get<AddressResponse>(formatedUrl);
               return result;
          }

          public async Task<AddressResponse> GetAddressByConnectedId(Guid connectedId)
          {
               var formatedUrl = string.Format(getAddressByConnectedId, connectedId);
               var result = await _httpService.Get<AddressResponse>(formatedUrl);
               return result;
          }

          public async Task<AddressResponse> SaveAddressAsync(AddAddress addAddress)
          {
               var result = await _httpService.Post<AddressResponse>(saveAddress, addAddress);
               return result;
          }

          public async Task<AddressResponse> UpdateAddress(AddAddress addAddress)
          {
               var result = await _httpService.Post<AddressResponse>(updateAddress, addAddress);
               return result;
          }

          public async Task<bool> DeleteAddressById(Guid addressId)
          {
               var formatedUrl = string.Format(deleteAddressById, addressId);
               var result = await _httpService.Post<bool>(formatedUrl, null);
               return result;
          }

          public async Task<bool> DeleteAddressByConnectedId(Guid connectedId)
          {
               var formatedUrl = string.Format(deleteAddressByConnectedId, connectedId);
               var result = await _httpService.Post<bool>(formatedUrl,null);
               return result;
          }
     }
}
