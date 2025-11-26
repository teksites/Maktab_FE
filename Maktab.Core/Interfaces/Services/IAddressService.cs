using MaktabDataContracts.Requests.Addresses;
using MaktabDataContracts.Responses.Addresses;

namespace Maktab.Core.Interfaces.Services
{
     public interface IAddressService : IDomainService
     {
          Task<bool> DeleteAddressByConnectedId(Guid connectedId);
          Task<bool> DeleteAddressById(Guid addressId);
          Task<AddressResponse> GetAddressByConnectedId(Guid connectedId);
          Task<AddressResponse> GetAddressById(Guid addressId);
          Task<AddressResponse> SaveAddressAsync(AddAddress addAddress);
          Task<AddressResponse> UpdateAddressAsync(AddAddress addAddress);
     }
}
