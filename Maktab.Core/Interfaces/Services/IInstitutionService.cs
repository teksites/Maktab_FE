using MaktabDataContracts.Responses.Institute;
using MaktabDataContracts.Requests.Institute;
using MaktabDataContracts.Requests.Policies;
using MaktabDataContracts.Enums;

namespace Maktab.Core.Interfaces.Services
{
     public interface IInstitutionService : IDomainService
     {
          Task<IEnumerable<InstituteResponse>> GetAllInstitutionsAsync();

          Task<InstituteResponse> GetInstitutionByIdAsync(Guid institutionId);

          Task<InstituteResponse> AddInstitutionAsync(AddInstitute addInstitute);
          Task<bool> IsInstituteExistAsync(string instituteName);
          Task<bool> RemoveInstituteAsync(Guid instituteId);

          Task<bool> DeactivateInstituteAsync(Guid instituteId);
          Task<IEnumerable<InstituteResponse>> GetAllActiveInstitutionsAsync();
          Task<IReadOnlyCollection<Consent>> GetChildConsentPoliciesAsync();
          Task<InstitutePolicyResponse> GetPolicyAsync(PolicyType policyType);
     }
}
