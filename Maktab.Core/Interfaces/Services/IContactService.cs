using MaktabDataContracts.Requests.OtherContacts;
using MaktabDataContracts.Responses.OtherContacts;

namespace Maktab.Core.Interfaces.Services
{
     public interface IContactService : IDomainService
     {
          Task<bool> DeleteContactByFamilyId(Guid familyId);
          Task<bool> DeleteContactById(Guid contactId);
          Task<IList<OtherContactResponse>> GetContactsByFamilyId(Guid familyId);
          Task<OtherContactResponse> GetContactById(Guid contactId);
          Task<bool> HasContactAddedForFamily(Guid familyId);
          Task<OtherContactResponse> SaveContactAsync(Guid familyId, AddOtherContact contact);
     }
}
