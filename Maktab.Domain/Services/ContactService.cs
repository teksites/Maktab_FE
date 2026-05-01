using Maktab.Core.Interfaces.Services;
using MaktabDataContracts.Requests.OtherContacts;
using MaktabDataContracts.Responses.OtherContacts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maktab.Domain.Services
{
     /// <summary>
     /// Contact service with comprehensive error handling, validation, and logging.
     /// Production-ready implementation for backend API integration.
     /// </summary>
     public class ContactService : BaseService, IContactService
     {
          // API Endpoints (properly formatted, not "formated")
          private const string getContactById = @"/api/othercontacts/{0}";
          private const string getContactByFamilyId = @"/api/families/{0}/othercontacts";
          private const string saveContact = @"/api/families/{0}/othercontacts/add";
          private const string updateContact = @"/api/othercontacts/update";
          private const string deleteContactById = @"/api/othercontacts/{0}/delete";
          private const string deleteContactByFamilId = @"/api/families/{0}/othercontacts/delete";
          private const string checkContactByFamilId = @"/api/families/{0}/othercontacts/check";

          private readonly ILogger<ContactService> _logger;

          public ContactService(
               IHttpService httpService,
               ILocalStorageService localStorageService,
               ILogger<ContactService> logger)
               : base(httpService, localStorageService)
          {
               _logger = logger ?? throw new ArgumentNullException(nameof(logger));
          }

          /// <summary>
          /// Get contact by ID with error handling and validation
          /// </summary>
          public async Task<OtherContactResponse> GetContactById(Guid contactId)
          {
               try
               {
                    // Validate input
                    if (contactId == Guid.Empty)
                    {
                         _logger.LogWarning("GetContactById called with empty GUID");
                         throw new ArgumentException("Contact ID cannot be empty", nameof(contactId));
                    }

                    _logger.LogInformation("Fetching contact {ContactId}", contactId);

                    // Make request
                    var formattedUrl = string.Format(getContactById, contactId);
                    var result = await _httpService.Get<OtherContactResponse>(formattedUrl);

                    // Validate response
                    if (result == null)
                    {
                         _logger.LogWarning("Contact {ContactId} not found", contactId);
                         return null;
                    }

                    _logger.LogInformation("Successfully fetched contact {ContactId}", contactId);
                    return result;
               }
               catch (HttpRequestException ex)
               {
                    _logger.LogError(ex, "Network error fetching contact {ContactId}", contactId);
                    throw;
               }
               catch (TaskCanceledException ex)
               {
                    _logger.LogError(ex, "Request timeout fetching contact {ContactId}", contactId);
                    throw;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Unexpected error fetching contact {ContactId}", contactId);
                    throw;
               }
          }

          /// <summary>
          /// Get all contacts for a family with error handling
          /// </summary>
          public async Task<IList<OtherContactResponse>> GetContactsByFamilyId(Guid familyId)
          {
               try
               {
                    // Validate input
                    if (familyId == Guid.Empty)
                    {
                         _logger.LogWarning("GetContactsByFamilyId called with empty GUID");
                         throw new ArgumentException("Family ID cannot be empty", nameof(familyId));
                    }

                    _logger.LogInformation("Fetching contacts for family {FamilyId}", familyId);

                    // Make request
                    var formattedUrl = string.Format(getContactByFamilyId, familyId);
                    var result = await _httpService.Get<IList<OtherContactResponse>>(formattedUrl);

                    // Validate response
                    if (result == null)
                    {
                         _logger.LogWarning("No contacts found for family {FamilyId}", familyId);
                         return new List<OtherContactResponse>();
                    }

                    _logger.LogInformation("Successfully fetched {ContactCount} contacts for family {FamilyId}", result.Count, familyId);
                    return result;
               }
               catch (HttpRequestException ex)
               {
                    _logger.LogError(ex, "Network error fetching contacts for family {FamilyId}", familyId);
                    throw;
               }
               catch (TaskCanceledException ex)
               {
                    _logger.LogError(ex, "Request timeout fetching contacts for family {FamilyId}", familyId);
                    throw;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Unexpected error fetching contacts for family {FamilyId}", familyId);
                    throw;
               }
          }

          /// <summary>
          /// Save new contact with validation and error handling
          /// </summary>
          public async Task<OtherContactResponse> SaveContactAsync(Guid familyId, AddOtherContact contact)
          {
               try
               {
                    // Validate input
                    if (familyId == Guid.Empty)
                    {
                         _logger.LogWarning("SaveContactAsync called with empty family GUID");
                         throw new ArgumentException("Family ID cannot be empty", nameof(familyId));
                    }

                    if (contact == null)
                    {
                         _logger.LogWarning("SaveContactAsync called with null contact");
                         throw new ArgumentNullException(nameof(contact), "Contact cannot be null");
                    }

                    _logger.LogInformation("Saving new contact for family {FamilyId}", familyId);

                    // Make request
                    var formattedUrl = string.Format(saveContact, familyId);
                    var result = await _httpService.Post<OtherContactResponse>(formattedUrl, contact);

                    // Validate response
                    if (result == null)
                    {
                         _logger.LogError("Server returned null response when saving contact");
                         throw new InvalidOperationException("Server did not return contact confirmation");
                    }

                    _logger.LogInformation("Successfully saved contact for family {FamilyId}", familyId);
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error saving contact for family {FamilyId}", familyId);
                    throw;
               }
          }

          /// <summary>
          /// Update existing contact (backend uses POST, not PUT)
          /// </summary>
          public async Task<OtherContactResponse> UpdateContactAsync(OtherContactResponse contact)
          {
               try
               {
                    // Validate input
                    if (contact == null)
                    {
                         _logger.LogWarning("UpdateContactAsync called with null contact");
                         throw new ArgumentNullException(nameof(contact), "Contact cannot be null");
                    }

                    _logger.LogInformation("Updating contact");

                    // Use POST (backend uses POST for update, not PUT)
                    var result = await _httpService.Post<OtherContactResponse>(updateContact, contact);

                    // Validate response
                    if (result == null)
                    {
                         _logger.LogError("Server returned null response when updating contact");
                         throw new InvalidOperationException("Server did not return updated contact");
                    }

                    _logger.LogInformation("Successfully updated contact");
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error updating contact");
                    throw;
               }
          }

          /// <summary>
          /// Delete contact by ID (backend uses POST, not DELETE)
          /// </summary>
          public async Task<bool> DeleteContactById(Guid contactId)
          {
               try
               {
                    // Validate input
                    if (contactId == Guid.Empty)
                    {
                         _logger.LogWarning("DeleteContactById called with empty GUID");
                         throw new ArgumentException("Contact ID cannot be empty", nameof(contactId));
                    }

                    _logger.LogInformation("Deleting contact {ContactId}", contactId);

                    // Use POST (backend uses POST for delete, not DELETE)
                    var formattedUrl = string.Format(deleteContactById, contactId);
                    var result = await _httpService.Post<bool>(formattedUrl, null);

                    _logger.LogInformation("Successfully deleted contact {ContactId}", contactId);
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error deleting contact {ContactId}", contactId);
                    throw;
               }
          }

          /// <summary>
          /// Delete contact by family ID (backend uses POST, not DELETE)
          /// </summary>
          public async Task<bool> DeleteContactByFamilyId(Guid familyId)
          {
               try
               {
                    // Validate input
                    if (familyId == Guid.Empty)
                    {
                         _logger.LogWarning("DeleteContactByFamilyId called with empty GUID");
                         throw new ArgumentException("Family ID cannot be empty", nameof(familyId));
                    }

                    _logger.LogInformation("Deleting contact for family {FamilyId}", familyId);

                    // Use POST (backend uses POST for delete, not DELETE)
                    var formattedUrl = string.Format(deleteContactByFamilId, familyId);
                    var result = await _httpService.Post<bool>(formattedUrl, null);

                    _logger.LogInformation("Successfully deleted contact for family {FamilyId}", familyId);
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error deleting contact for family {FamilyId}", familyId);
                    throw;
               }
          }

          /// <summary>
          /// Check if contact has been added for family (backend uses POST)
          /// </summary>
          public async Task<bool> HasContactAddedForFamily(Guid familyId)
          {
               try
               {
                    // Validate input
                    if (familyId == Guid.Empty)
                    {
                         _logger.LogWarning("HasContactAddedForFamily called with empty GUID");
                         throw new ArgumentException("Family ID cannot be empty", nameof(familyId));
                    }

                    _logger.LogInformation("Checking if contact added for family {FamilyId}", familyId);

                    // Use POST (backend uses POST for this check)
                    var formattedUrl = string.Format(checkContactByFamilId, familyId);
                    var result = await _httpService.Post<bool>(formattedUrl, null);

                    _logger.LogInformation("Contact check result for family {FamilyId}: {HasContact}", familyId, result);
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error checking if contact added for family {FamilyId}", familyId);
                    throw;
               }
          }
     }
}
