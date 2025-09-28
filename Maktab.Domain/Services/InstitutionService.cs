using Maktab.Core.Interfaces.Services;
using MaktabDataContracts.Responses.Institute;
using MaktabDataContracts.Requests.Institute;

namespace Maktab.Domain.Services
{
     public class InstitutionService : BaseService, IInstitutionService
     {
          private Guid iccSchoolId = new Guid("FDCA4C86-DF0E-4CCC-BCE7-D4AE62F6E337");
          private Guid iccSchool2Id = new Guid("5AE84751-B58B-44D8-AC30-08F28E32BF15");

          private IList<InstituteReponse> _institutes;

          public InstitutionService(
                   IHttpService httpService,
                   ILocalStorageService localStorageService)
               : base(httpService, localStorageService)
          {
               _institutes = GetAllInstitutions();
          }

          public async Task<InstituteReponse> GetInstitutionByIdAsync(Guid institutionId)
          {
               return _institutes.FirstOrDefault(x => x.InstituteId == institutionId);

          }

          public async Task<IEnumerable<InstituteReponse>> GetAllInstitutionsAsync()
          {
               return _institutes;
          }

          private IList<InstituteReponse> GetAllInstitutions()
          { 
               var institutions = new List<InstituteReponse>()
               {
                    new InstituteReponse
                    { 
                         InstituteId = iccSchoolId, Name = "ICC Brossard",
                         IsActive = true,
                         
                         
                    },
                    new InstituteReponse
                    {
                         InstituteId = iccSchool2Id, Name = "Qobaa Arabic",
                         IsActive = true,
                    },
               };

               return institutions;
          }

          public Task<InstituteReponse> AddInstitutionAsync(AddInstitute addInstitute)
          {
               throw new NotImplementedException();
          }

          public Task<bool> IsInstituteExistAsync(string instituteName)
          {
               throw new NotImplementedException();
          }

          public Task<bool> RemoveInstituteAsync(Guid instituteId)
          {
               throw new NotImplementedException();
          }

          public Task<bool> DeactivateInstituteAsync(Guid instituteId)
          {
               throw new NotImplementedException();
          }
     }
}
