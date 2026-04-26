using Maktab.Core.Interfaces.Services;
using MaktabDataContracts.Enums;
using MaktabDataContracts.Models;
using MaktabDataContracts.Requests.Institute;
using MaktabDataContracts.Requests.Policies;
using MaktabDataContracts.Responses.Children;
using MaktabDataContracts.Responses.Institute;
using System.Collections.Generic;

namespace Maktab.Domain.Services
{
     public class InstitutionService : BaseService, IInstitutionService
     {
          private const string getInstituteById = @"/api/institutes/{0}";
          private const string getActiveInstitutes = @"/api/institutes?onlyActive=true";
          private const string getInstitutes = @"/api/institutes?onlyActive=false";

          private const string addInstituteUrl = @"/api/institutes";

          private const string removeInstituteById = @"/api/institutes/{0}?hardDelete={1}";
          private const string getInstitutePolicies = @"/api/institutes/{0}/policies";
          private const string getInstitutePolicyById = @"/api/institutes/policies/{0}";

          private const string addChildByFamilyId = @"/api/families/{0}/children/add";
          private const string removeChildByFamilyId = @"/api/families/{0}/children/delete?ifHardDelete=false";
          private const string isChildrenExistByRamQNumber = @"/api/children/check";

          private const string getPolicyByType = @"/policies?policyType={0}";

          public InstitutionService(
                   IHttpService httpService,
                   ILocalStorageService localStorageService)
               : base(httpService, localStorageService)
          {
          }

          public async Task<InstituteResponse> GetInstitutionByIdAsync(Guid institutionId)
          {
               var formatedUrl = string.Format(getInstituteById, institutionId);
               var result = await _httpService.Get<InstituteResponse>(formatedUrl);
               return result;
          }

          public async Task<InstitutePolicyResponse> GetPolicyAsync(PolicyType policyType)   
          {
               var formatedUrl = string.Format(getPolicyByType, policyType);
               var result = await _httpService.Get<InstitutePolicyResponse>(formatedUrl);
               return result;
          }

          public async Task<IReadOnlyCollection<Consent>> GetChildConsentPoliciesAsync()
          {
               var instituteConcentPolicy = await GetPolicyAsync(PolicyType.ConsentPolicy);
               if(!string.IsNullOrEmpty(instituteConcentPolicy?.Details))
               {
                    IReadOnlyCollection<Consent> concentPolicies = System.Text.Json.JsonSerializer.Deserialize<List<Consent>>(instituteConcentPolicy.Details);
                    return concentPolicies ?? Array.Empty<Consent>();
               }

               return Array.Empty<Consent>();
          }

          public async Task<IEnumerable<InstituteResponse>> GetAllInstitutionsAsync()
          {
               var result = await _httpService.Get<IEnumerable<InstituteResponse>>(getInstitutes);
               return result;
          }

          public async Task<IEnumerable<InstituteResponse>> GetAllActiveInstitutionsAsync()
          {
               var result = await _httpService.Get<IEnumerable<InstituteResponse>>(getActiveInstitutes);
               return result;
          }

          public async Task<InstituteResponse> AddInstitutionAsync(AddInstitute addInstitute)
          {
               var result = await _httpService.Post<InstituteResponse>(addInstituteUrl, addInstitute);
               return result;
          }

          public Task<bool> IsInstituteExistAsync(string instituteName)
          {
               throw new NotImplementedException();
          }

          public async Task<bool> RemoveInstituteAsync(Guid instituteId)
          {
               var formatedUrl = string.Format(removeInstituteById, instituteId, false);
               var result = await _httpService.Delete<bool>(formatedUrl);
               return result;
          }

          public Task<bool> DeactivateInstituteAsync(Guid instituteId)
          {
               throw new NotImplementedException();
          }

          public async Task<IEnumerable<InstitutePolicyResponse>> GetInstitutePoliciesByInstitureIdAsync(Guid institutionId)
          {
               var formatedUrl = string.Format(getInstitutePolicies, institutionId);
               var result = await _httpService.Get<IEnumerable<InstitutePolicyResponse>>(formatedUrl);
               return result;
          }

          public async Task<InstitutePolicyResponse> GetInstitutePolicyByIdAsync(Guid policyId)
          {
               var formatedUrl = string.Format(getInstitutePolicyById, policyId);
               var result = await _httpService.Get<InstitutePolicyResponse>(formatedUrl);
               return result;
          }
     }
}
