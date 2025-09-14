using Maktab.Core.Interfaces.Services;
using MaktabDataContracts.Models;
using MaktabDataContracts.Requests.Children;
using MaktabDataContracts.Responses.Children;

namespace Maktab.Domain.Services
{
     public class ChildrenService : BaseService, IChildrenService
     {
          private const string getChildById = @"/api/children/{0}";
          private const string removeChildById = @"/api/children/{0}/delete?ifHardDelete=false";
          private const string getChildrenByFamilyId = @"/api/families/{0}/children";
          private const string addChildByFamilyId = @"/api/families/{0}/children/add";
          private const string removeChildByFamilyId = @"/api/families/{0}/children/delete?ifHardDelete=false";
          private const string isChildrenExistByRamQNumber = @"/api/children/check";



          public ChildrenService(IHttpService httpService, ILocalStorageService localStorageService) 
          : base(httpService, localStorageService)
          {
          }

          public async Task<ChildResponse> GetChildByIdAsync(Guid childId)
          {
               var formatedUrl = string.Format(getChildById, childId);
               var result = await _httpService.Get<MaktabApiResult<ChildResponse>>(formatedUrl);
               return result.Result;
          }

          public async Task<IEnumerable<ChildResponse>> GetChildrenByFamilyIdAsync(Guid familyId)
          {
               var formatedUrl = string.Format(getChildrenByFamilyId, familyId);
               var result = await _httpService.Get<List<MaktabApiResult<ChildResponse>>>(formatedUrl);

               var child = result.Select(x => x.Result);
               return child;
          }

          public async Task<ChildResponse> AddChildToFamilyAsync(Guid familyId, AddChildRequest addChildRequest)
          {
               var formatedUrl = string.Format(addChildByFamilyId, familyId);
               var result = await _httpService.Post<ChildResponse>(formatedUrl, addChildRequest);
               return result;
          }

          public async Task<bool> IsChildExistWithRamQNumberAsync(Guid familyId, string ramqNumber)
          {
               var verificationRequest = new
               {
                    FamilyId = familyId,
                    RamqNumber = ramqNumber
               };
               var result = await _httpService.Post<bool>(isChildrenExistByRamQNumber, verificationRequest);
               return result;
          }

          public async Task<bool> RemoveChild(Guid childId)
          {
               var formatedUrl = string.Format(removeChildById, childId);
               var result = await _httpService.Post<bool>(formatedUrl);
               return result;
          }

          public async Task<bool> RemoveChildFromFamily(Guid familyId)
          {
               var formatedUrl = string.Format(removeChildByFamilyId, familyId);
               var result = await _httpService.Post<bool>(formatedUrl);
               return result;
          }
     }
}
