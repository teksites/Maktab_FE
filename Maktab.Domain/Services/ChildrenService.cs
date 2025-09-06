using Maktab.Core.Interfaces.Services;
using MaktabDataContracts.Models;
using MaktabDataContracts.Requests.Children;
using MaktabDataContracts.Responses.Children;

namespace Maktab.Domain.Services
{
     public class ChildrenService : BaseService, IChildrenService
     {
          private const string getChildById = @"/api/children/{0}";
          private const string removeChildById = @"/api/children/{0}/delete";
          private const string getChildrenByFamilyId = @"/api/families/{0}/children";
          private const string addChildByFamilyId = @"/api/families/{0}/children/add";
          private const string removeChildByFamilyId = @"/api/families/{0}/children/delete";
          private const string hasChildrenByFamilyId = @"/api/children/check";



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
               var result = await _httpService.Get< IEnumerable<MaktabApiResult < ChildResponse>>>(formatedUrl);

               var child = result.Select(x => x.Result);
               return child;
          }

          public async Task<ChildResponse> SendUserActivationCodeAsync(Guid familyId, AddChildRequest addChildRequest)
          {
               var formatedUrl = string.Format(addChildByFamilyId, familyId);
               var result = await _httpService.Put<ChildResponse>(formatedUrl, addChildRequest);
               return result;
          }
     }
}
