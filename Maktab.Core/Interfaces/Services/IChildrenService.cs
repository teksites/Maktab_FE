using MaktabDataContracts.Requests.Children;
using MaktabDataContracts.Requests.Policies;
using MaktabDataContracts.Responses.Children;

namespace Maktab.Core.Interfaces.Services
{
     /// <summary>
     /// Service interface for children operations.
     /// All methods have proper error handling, validation, and logging.
     /// </summary>
     public interface IChildrenService : IDomainService
     {
          /// <summary>
          /// Get child by ID with error handling
          /// </summary>
          Task<ChildResponse> GetChildByIdAsync(Guid childId);

          /// <summary>
          /// Get all children for a family with error handling
          /// </summary>
          Task<IEnumerable<ChildResponse>> GetChildrenByFamilyIdAsync(Guid familyId, bool includeAdults = false);

          /// <summary>
          /// Add child to family with validation
          /// </summary>
          Task<ChildResponse> AddChildToFamilyAsync(Guid familyId, AddChildRequest addChildRequest, IReadOnlyCollection<ChildConsent> childConsents);

          /// <summary>
          /// Check if child exists with RAMQ number (backend uses POST)
          /// </summary>
          Task<bool> IsChildExistWithRamQNumberAsync(Guid familyId, string ramqNumber);

          /// <summary>
          /// Remove all children from family (backend uses POST)
          /// </summary>
          Task<bool> RemoveChildFromFamilyAsync(Guid familyId);

          /// <summary>
          /// Remove child by ID (backend uses POST)
          /// </summary>
          Task<bool> RemoveChildByIdAsync(Guid childId);
          Task<ChildResponse> UpdateChildAsync(UpdateChildRequest updateChildResponse);
          Task<ChildEducationalProfileResponse> GetEducationalProfileByChildIdAsync(Guid childId);
          Task<ChildEducationalProfileResponse> PostEducationalProfileByChildIdAsync(Guid childId, Guid familyId, UpsertChildEducationalProfileRequest childEducationalProfileRequest, CancellationToken cancellationToken = default);
     }
}
