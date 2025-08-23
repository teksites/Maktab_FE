using MaktabDataContracts.Requests.Users;
using MaktabDataContracts.Responses.Users;

namespace Maktab.Core.Interfaces.Services
{
     public interface IUserService : IDomainService
     {
          Task<UserInformationResponse> GetUserByIdAsync(Guid id);
          Task<UserInformationResponse> RegisterUserAsync(AddUserInformation userInfo);
          Task<bool> SendUserActivationCodeAsync(Guid userId);
          Task<bool> ValidateUserByActivationCodeAsync(Guid userId, UserVerificationRequest request);
          Task<bool> ValidateUsernameAsync(string username);
          Task<bool> ChangeUserPasswordAsync(Guid userId, UpdateUserPasswordRequest changeRequest);
          Task<bool> ForgotUserPasswordAsync(string username);
     }
}
