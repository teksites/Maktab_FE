using Maktab.Core.Interfaces.Services;
using MaktabDataContracts.Requests.Addresses;
using MaktabDataContracts.Responses.Addresses;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Maktab.Domain.Services
{
     /// <summary>
     /// Address service with comprehensive error handling, validation, and logging.
     /// Production-ready implementation for backend API integration.
     /// </summary>
     public class AddressService : BaseService, IAddressService
     {
          // API Endpoints (properly formatted, not "formated")
          private const string getAddressById = @"/api/address/{0}";
          private const string getAddressByConnectedId = @"/api/connectedid/{0}/address";
          private const string saveAddress = @"/api/user/address/add";
          private const string updateAddress = @"/api/address/update";
          private const string deleteAddressById = @"/api/address/{0}/delete";
          private const string deleteAddressByConnectedId = @"/api/connectedids/{0}/address/delete";

          private readonly ILogger<AddressService> _logger;

          public AddressService(
               IHttpService httpService,
               ILocalStorageService localStorageService,
               ILogger<AddressService> logger)
               : base(httpService, localStorageService)
          {
               _logger = logger ?? throw new ArgumentNullException(nameof(logger));
          }

          /// <summary>
          /// Get address by ID with error handling and validation
          /// </summary>
          public async Task<AddressResponse> GetAddressById(Guid addressId)
          {
               try
               {
                    // Validate input
                    if (addressId == Guid.Empty)
                    {
                         _logger.LogWarning("GetAddressById called with empty GUID");
                         throw new ArgumentException("Address ID cannot be empty", nameof(addressId));
                    }

                    _logger.LogInformation("Fetching address {AddressId}", addressId);

                    // Make request
                    var formattedUrl = string.Format(getAddressById, addressId);
                    var result = await _httpService.Get<AddressResponse>(formattedUrl);

                    // Validate response
                    if (result == null)
                    {
                         _logger.LogWarning("Address {AddressId} not found", addressId);
                         return null;
                    }

                    _logger.LogInformation("Successfully fetched address {AddressId}", addressId);
                    return result;
               }
               catch (HttpRequestException ex)
               {
                    _logger.LogError(ex, "Network error fetching address {AddressId}", addressId);
                    throw;
               }
               catch (TaskCanceledException ex)
               {
                    _logger.LogError(ex, "Request timeout fetching address {AddressId}", addressId);
                    throw;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Unexpected error fetching address {AddressId}", addressId);
                    throw;
               }
          }

          /// <summary>
          /// Get address by connected ID
          /// </summary>
          public async Task<IEnumerable<AddressResponse>> GetAddressesByConnectedId(Guid connectedId)
          {
               try
               {
                    // Validate input
                    if (connectedId == Guid.Empty)
                    {
                         _logger.LogWarning("GetAddressByConnectedId called with empty GUID");
                         throw new ArgumentException("Connected ID cannot be empty", nameof(connectedId));
                    }

                    _logger.LogInformation("Fetching address by connected ID {ConnectedId}", connectedId);

                    var formattedUrl = string.Format(getAddressByConnectedId, connectedId);
                    var result = await _httpService.Get<IEnumerable<AddressResponse>>(formattedUrl);

                    if (result == null)
                    {
                         _logger.LogWarning("Address for connected ID {ConnectedId} not found", connectedId);
                         return null;
                    }

                    _logger.LogInformation("Successfully fetched address for connected ID {ConnectedId}", connectedId);
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error fetching address by connected ID {ConnectedId}", connectedId);
                    throw;
               }
          }

          /// <summary>
          /// Save new address with validation
          /// </summary>
          public async Task<AddressResponse> SaveAddressAsync(AddAddress addAddress)
          {
               try
               {
                    // Validate input
                    if (addAddress == null)
                    {
                         _logger.LogWarning("SaveAddressAsync called with null address");
                         throw new ArgumentNullException(nameof(addAddress), "Address cannot be null");
                    }

                    _logger.LogInformation("Saving new address");

                    var result = await _httpService.Post<AddressResponse>(saveAddress, addAddress);

                    // Validate response
                    if (result == null)
                    {
                         _logger.LogError("Server returned null response when saving address");
                         throw new InvalidOperationException("Server did not return address confirmation");
                    }

                    _logger.LogInformation("Successfully saved address");
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error saving address");
                    throw;
               }
          }

          /// <summary>
          /// Update existing address (backend uses POST, not PUT)
          /// </summary>
          public async Task<AddressResponse> UpdateAddressAsync(AddressResponse addAddress)
          {
               try
               {
                    // Validate input
                    if (addAddress == null)
                    {
                         _logger.LogWarning("UpdateAddressAsync called with null address");
                         throw new ArgumentNullException(nameof(addAddress), "Address cannot be null");
                    }

                    _logger.LogInformation("Updating address");

                    // Use POST (backend uses POST for update, not RESTful but that's the API)
                    var result = await _httpService.Post<AddressResponse>(updateAddress, addAddress);

                    // Validate response
                    if (result == null)
                    {
                         _logger.LogError("Server returned null response when updating address");
                         throw new InvalidOperationException("Server did not return updated address");
                    }

                    _logger.LogInformation("Successfully updated address");
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error updating address");
                    throw;
               }
          }

          /// <summary>
          /// Delete address by ID (backend uses POST, not DELETE)
          /// </summary>
          public async Task<bool> DeleteAddressById(Guid addressId)
          {
               try
               {
                    // Validate input
                    if (addressId == Guid.Empty)
                    {
                         _logger.LogWarning("DeleteAddressById called with empty GUID");
                         throw new ArgumentException("Address ID cannot be empty", nameof(addressId));
                    }

                    _logger.LogInformation("Deleting address {AddressId}", addressId);

                    var formattedUrl = string.Format(deleteAddressById, addressId);
                    var result = await _httpService.Post<bool>(formattedUrl, null);

                    _logger.LogInformation("Successfully deleted address {AddressId}", addressId);
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error deleting address {AddressId}", addressId);
                    throw;
               }
          }

          /// <summary>
          /// Delete address by connected ID (backend uses POST, not DELETE)
          /// </summary>
          public async Task<bool> DeleteAddressByConnectedId(Guid connectedId)
          {
               try
               {
                    if (connectedId == Guid.Empty)
                    {
                         _logger.LogWarning("DeleteAddressByConnectedId called with empty GUID");
                         throw new ArgumentException("Connected ID cannot be empty", nameof(connectedId));
                    }

                    _logger.LogInformation("Deleting address by connected ID {ConnectedId}", connectedId);

                    var formattedUrl = string.Format(deleteAddressByConnectedId, connectedId);
                    var result = await _httpService.Post<bool>(formattedUrl, null);

                    _logger.LogInformation("Successfully deleted address for connected ID {ConnectedId}", connectedId);
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error deleting address by connected ID {ConnectedId}", connectedId);
                    throw;
               }
          }
     }
}
