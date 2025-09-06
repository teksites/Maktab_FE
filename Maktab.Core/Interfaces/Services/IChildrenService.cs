using MaktabDataContracts.Requests.Children;
using MaktabDataContracts.Responses.Children;

namespace Maktab.Core.Interfaces.Services
{
     public interface IChildrenService : IDomainService
     {
          Task<ChildResponse> GetChildByIdAsync(Guid childId);
          Task<IEnumerable<ChildResponse>> GetChildrenByFamilyIdAsync(Guid familyId);
          Task<ChildResponse> SendUserActivationCodeAsync(Guid familyId, AddChildRequest addChildRequest);
     }
}
