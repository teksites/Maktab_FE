using MaktabDataContracts.Requests.Users;
using MaktabDataContracts.Responses.Users;

namespace Maktab.Core.Interfaces.Services
{
     /// <summary>
     /// Service interface for user operations.
     /// All methods have proper error handling, validation, and logging.
     /// </summary>
     public interface IUserService : IDomainService
     {
          /// <summary>
          /// Get user by ID with full error handling
          /// </summary>
          Task<UserInformationResponse> GetUserByIdAsync(Guid id);

          /// <summary>
          /// Register new user with validation
          /// </summary>
          Task<UserInformationResponse> RegisterUserAsync(AddUserInformation userInfo);

          /// <summary>
          /// Send user activation code
          /// </summary>
          Task<bool> SendUserActivationCodeAsync(Guid userId);

          /// <summary>
          /// Activate user by verification code
          /// </summary>
          Task<bool> ActivateUserByCodeAsync(Guid userId, UserVerificationRequest request);

          /// <summary>
          /// Validate username availability
          /// </summary>
          Task<bool> ValidateUsernameAsync(string username);

          /// <summary>
          /// Change user password
          /// </summary>
          Task<bool> ChangeUserPasswordAsync(Guid userId, UpdateUserPasswordRequest changeRequest);

          /// <summary>
          /// Process forgot password request
          /// </summary>
          Task<bool> ForgotUserPasswordAsync(string username);

          /// <summary>
          /// Link user to family by IDs
          /// </summary>
          Task<UserInformationResponse> LinkUserToFamilyByIdAsync(Guid userId, Guid familyId);

          /// <summary>
          /// Get family ID by user information
          /// </summary>
          Task<Guid> GetFamilyIdByUserInfoAsync(string userEmail, string userPhone);

          /// <summary>
          /// Get extended user info by user ID
          /// </summary>
          Task<ExtendedUserInformationResponse> GetExtendedInfoByUserIdAsync(Guid userId);

          /// <summary>
          /// Save extended user info
          /// </summary>
          Task<ExtendedUserInformationResponse> SaveExtendedInfoAsync(Guid userId, AddExtendedUserInformationRequest request);

          /// <summary>
          /// Update extended user info (backend uses PUT)
          /// </summary>
          Task<ExtendedUserInformationResponse> UpdateExtendedInfoAsync(Guid userId, ExtendedUserInformationResponse request);

          /// <summary>
          /// Delete extended user info
          /// </summary>
          Task<bool> DeleteExtendedInfoAsync(Guid userId);

          /// <summary>
          /// Get family members by family ID
          /// </summary>
          Task<IEnumerable<UserInformationResponse>> GetFamilyByFamilyId(Guid familyId);
          Task<FamilyInformationDetailsResponse> GetFamilyDetailInfoByFamilyId(Guid familyId);
     }
}
