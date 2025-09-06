using MaktabDataContracts.Requests.Users;
using MaktabDataContracts.Responses.Users;

namespace Maktab.Core.Interfaces.Services
{
     public interface IUserService : IDomainService
     {
          Task<UserInformationResponse> GetUserByIdAsync(Guid id);
          Task<UserInformationResponse> RegisterUserAsync(AddUserInformation userInfo);
          Task<bool> SendUserActivationCodeAsync(Guid userId);
          Task<bool> ActivateUserByCodeAsync(Guid userId, UserVerificationRequest request);
          Task<bool> ValidateUsernameAsync(string username);
          Task<bool> ChangeUserPasswordAsync(Guid userId, UpdateUserPasswordRequest changeRequest);
          Task<bool> ForgotUserPasswordAsync(string username);
          Task<UserInformationResponse> LinkUserToFamilyByIdAsync(Guid userId, Guid familyId);
          Task<Guid> GetFamilyIdByUserInfoAsync(string userEmail, string userPhone);
          Task<ExtendedUserInformationResponse> GetExtendedInfoByUserIdAsync(Guid userId);
          Task<ExtendedUserInformationResponse> SaveExtendedInfoAsync(Guid userId, AddExtendedUserInformationRequest request);
     }
}
