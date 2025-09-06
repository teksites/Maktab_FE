using Maktab.Core.Interfaces.Services;
using Maktab.Models.Models;
using MaktabDataContracts.Requests.Users;
using MaktabDataContracts.Responses.Users;

namespace Maktab.Domain.Services
{
     public class UserService : BaseService, IUserService
     {
          private const string getUserById = @"/api/users/{0}";
          private const string validateUserActivationCode = @"/api/users/{0}/verify";
          private const string sendActivationCode = @"/api/users/{0}/activationcode";
          private const string addUser = @"/api/users/add";
          private const string changeUserPassword = @"/api/users/{0}/resetpassword";
          private const string forgotUserPassword = @"/api/users/forgotpassword?userName={0}";
          private const string checkUsernameExist = @"/api/users/checkuser?userName={0}";
          private const string checkUserRegistered = @"/api/users/checkuser?userName={0}";
          private const string linkUserToFamilId = @"/api/users/{0}/link/{familyId}";
          private const string getFamilIdByUserInfo = @"/api/users/familyinfo";
          private const string getExtendedInfoByUserId = @"/api/users/{0}/extendedinfo";
          private const string saveExtendedInfo = @"/api/users/{0}/extendedinfo";






          public UserService(IHttpService httpService, ILocalStorageService localStorageService) 
          : base(httpService, localStorageService)
          {
          }

          public async Task<UserInformationResponse> GetUserByIdAsync(Guid userId)
          {
               var formatedUrl = string.Format(getUserById, userId);
               var result = await _httpService.Get<UserInformationResponse>(formatedUrl);
               return result;
          }

          public async Task<bool> ActivateUserByCodeAsync(Guid userId, UserVerificationRequest request)
          {
               var formatedUrl = string.Format(validateUserActivationCode, userId);
               var result = await _httpService.Post<bool>(formatedUrl, request);
               return result;
          }

          public async Task<bool> SendUserActivationCodeAsync(Guid userId)
          {
               var formatedUrl = string.Format(sendActivationCode, userId);
               var result = await _httpService.Put<bool>(formatedUrl, null);
               return result;
          }

          public async Task<UserInformationResponse> RegisterUserAsync(AddUserInformation userInfo)
          {
               var result = await _httpService.Post<UserInformationResponse>(addUser, userInfo);
               return result;
          }

          public async Task<bool> ChangeUserPasswordAsync(Guid userId, UpdateUserPasswordRequest changeRequest)
          {
               var formatedUrl = string.Format(changeUserPassword, userId);
               var result = await _httpService.Post<bool>(formatedUrl, changeRequest);
               return result;
          }

          public async Task<bool> ForgotUserPasswordAsync(string username)
          {
               var formatedUrl = string.Format(forgotUserPassword, username);
               var result = await _httpService.Get<bool>(formatedUrl);
               return result;
          }

          public async Task<bool> ValidateUsernameAsync(string username)
          {
               var formatedUrl = string.Format(checkUsernameExist, username);
               var result = await _httpService.Get<bool>(formatedUrl);
               return result;
          }

          public async Task<UserInformationResponse> LinkUserToFamilyByIdAsync(Guid userId, Guid familyId)
          {
               var formatedUrl = string.Format(linkUserToFamilId, userId, familyId);
               var result = await _httpService.Put<UserInformationResponse>(formatedUrl);
               return result;
          }

          public async Task<Guid> GetFamilyIdByUserInfoAsync(string userEmail, string userPhone)
          {
               var formatedUrl = new UserFamilyInformationRequest
               {
                    Email = userEmail,
               };

               if(string.IsNullOrEmpty(userPhone))
               {
                    formatedUrl.Phone = userPhone;
               }

               var result = await _httpService.Post<string>(getFamilIdByUserInfo, formatedUrl);

               if (Guid.TryParse(result, out var familyId))
               {
                    return familyId;
               }

               return Guid.Empty;
          }

          public async Task<ExtendedUserInformationResponse> GetExtendedInfoByUserIdAsync(Guid userId)
          {
               var formatedUrl = string.Format(getExtendedInfoByUserId, userId);
               var result = await _httpService.Get<ExtendedUserInformationResponse>(formatedUrl);
               return result;
          }

          public async Task<ExtendedUserInformationResponse> SaveExtendedInfoAsync(Guid userId, MaktabDataContracts.Requests.Users.AddExtendedUserInformationRequest request)
          {
               var formatedUrl = string.Format(saveExtendedInfo, userId);
               var result = await _httpService.Post<ExtendedUserInformationResponse>(formatedUrl, request);
               return result;
          }
     }
}
