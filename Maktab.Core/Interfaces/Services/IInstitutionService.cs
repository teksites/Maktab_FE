using Maktab.Models.Models;
using MaktabDataContracts.Responses.Institute;
using MaktabDataContracts.Requests.Institute;

namespace Maktab.Core.Interfaces.Services
{
     public interface IInstitutionService : IDomainService
     {
          Task<IEnumerable<InstituteReponse>> GetAllInstitutionsAsync();

          Task<InstituteReponse> GetInstitutionByIdAsync(Guid institutionId);

          Task<InstituteReponse> AddInstitutionAsync(AddInstitute addInstitute);
          Task<bool> IsInstituteExistAsync(string instituteName);
          Task<bool> RemoveInstituteAsync(Guid instituteId);

          Task<bool> DeactivateInstituteAsync(Guid instituteId);
     }
}
