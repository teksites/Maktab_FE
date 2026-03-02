using MaktabDataContracts.Requests.Addresses;
using MaktabDataContracts.Responses.Addresses;

namespace Maktab.Core.Interfaces.Services
{
     /// <summary>
     /// Service interface for address operations.
     /// All methods have proper error handling, validation, and logging.
     /// </summary>
     public interface IAddressService : IDomainService
     {
          /// <summary>
          /// Get address by ID with full error handling
          /// </summary>
          Task<AddressResponse> GetAddressById(Guid addressId);

          /// <summary>
          /// Get address by connected ID (family/parent connection)
          /// </summary>
          Task<AddressResponse> GetAddressByConnectedId(Guid connectedId);

          /// <summary>
          /// Save new address with validation
          /// </summary>
          Task<AddressResponse> SaveAddressAsync(AddAddress addAddress);

          /// <summary>
          /// Update existing address (backend uses POST)
          /// </summary>
          Task<AddressResponse> UpdateAddressAsync(AddressResponse addAddress);

          /// <summary>
          /// Delete address by ID (backend uses POST)
          /// </summary>
          Task<bool> DeleteAddressById(Guid addressId);

          /// <summary>
          /// Delete address by connected ID
          /// </summary>
          Task<bool> DeleteAddressByConnectedId(Guid connectedId);
     }
}
