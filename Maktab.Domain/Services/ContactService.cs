using Maktab.Core.Interfaces.Services;
using MaktabDataContracts.Requests.OtherContacts;
using MaktabDataContracts.Responses.OtherContacts;

namespace Maktab.Domain.Services
{
     public class ContactService : BaseService, IContactService
     {

          private const string getContactById = @"/api/otherContacts/{0}";
          private const string getContactByFamilyId = @"/api/families/{0}/othercontacts";
          private const string saveContact = @"/api/families/{0}/othercontacts/add";
          private const string updateAddress = @"/api/othercontacts/update";
          private const string deleteContactById = @"/api/othercontacts/{0}/delete";
          private const string deleteContactByFamilId = @"/api/families/{0}/othercontacts/delete";
          private const string checkContactByFamilId = @"/api/families/{0}/othercontacts/check";

          public ContactService(IHttpService httpService, ILocalStorageService localStorageService) 
          : base(httpService, localStorageService)
          {
          }

          public async Task<OtherContactResponse> GetContactById(Guid contactId)
          {
               var formatedUrl = string.Format(getContactById, contactId);
               var result = await _httpService.Get<OtherContactResponse>(formatedUrl);
               return result;
          }

          public async Task<IList<OtherContactResponse>> GetContactsByFamilyId(Guid familyId)
          {
               var formatedUrl = string.Format(getContactByFamilyId, familyId);
               var result = await _httpService.Get<IList<OtherContactResponse>>(formatedUrl);
               return result;
          }

          public async Task<OtherContactResponse> SaveContactAsync(Guid familyId, AddOtherContact contact)
          {
               var formatedUrl = string.Format(saveContact, familyId);
               var result = await _httpService.Post<OtherContactResponse>(formatedUrl, contact);
               return result;
          }

          public async Task<OtherContactResponse> UpdateContactAsync(OtherContactResponse contact)
          {
               var result = contact;//await _httpService.Post<AddressResponse>(updateAddress, addAddress);
               return result;
          }

          public async Task<bool> DeleteContactById(Guid contactId)
          {
               var formatedUrl = string.Format(deleteContactById, contactId);
               var result = await _httpService.Post<bool>(formatedUrl, null);
               return result;
          }

          public async Task<bool> DeleteContactByFamilyId(Guid familyId)
          {
               var formatedUrl = string.Format(deleteContactByFamilId, familyId);
               var result = await _httpService.Post<bool>(formatedUrl, null);
               return result;
          }

          public async Task<bool> HasContactAddedForFamily(Guid familyId)
          {
               var formatedUrl = string.Format(checkContactByFamilId, familyId);
               var result = await _httpService.Post<bool>(formatedUrl, null);
               return result;
          }
     }
}
