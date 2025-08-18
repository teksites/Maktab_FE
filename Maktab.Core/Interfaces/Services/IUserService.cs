using MaktabDataContracts.Requests.Users;
using MaktabDataContracts.Responses.Users;

namespace Maktab.Core.Interfaces.Services
{
     public interface IUserService : IDomainService
     {
          Task<UserInformationResponse> GetUserById(Guid id);
          Task<bool> ValidateUserByActivationCode(Guid userId, UserVerificationRequest request);
     }
}
