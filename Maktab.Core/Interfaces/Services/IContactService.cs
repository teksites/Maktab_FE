using MaktabDataContracts.Requests.OtherContacts;
using MaktabDataContracts.Responses.OtherContacts;

namespace Maktab.Core.Interfaces.Services
{
     /// <summary>
     /// Service interface for contact operations.
     /// All methods have proper error handling, validation, and logging.
     /// </summary>
     public interface IContactService : IDomainService
     {
          /// <summary>
          /// Delete contact by family ID (backend uses POST)
          /// </summary>
          Task<bool> DeleteContactByFamilyId(Guid familyId);

          /// <summary>
          /// Delete contact by ID (backend uses POST)
          /// </summary>
          Task<bool> DeleteContactById(Guid contactId);

          /// <summary>
          /// Get contacts by family ID with error handling
          /// </summary>
          Task<IList<OtherContactResponse>> GetContactsByFamilyId(Guid familyId);

          /// <summary>
          /// Get contact by ID with error handling
          /// </summary>
          Task<OtherContactResponse> GetContactById(Guid contactId);

          /// <summary>
          /// Check if contact has been added for family (backend uses POST)
          /// </summary>
          Task<bool> HasContactAddedForFamily(Guid familyId);

          /// <summary>
          /// Save new contact with validation
          /// </summary>
          Task<OtherContactResponse> SaveContactAsync(Guid familyId, AddOtherContact contact);

          /// <summary>
          /// Update existing contact (backend uses POST)
          /// </summary>
          Task<OtherContactResponse> UpdateContactAsync(OtherContactResponse contact);
     }
}
