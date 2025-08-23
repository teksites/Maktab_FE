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
          private const string checkUserExist = @"/api/users/checkuser?userName={0}";

          






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

          public async Task<bool> ValidateUserByActivationCodeAsync(Guid userId, UserVerificationRequest request)
          {
               var formatedUrl = string.Format(validateUserActivationCode, userId);
               var result = await _httpService.Get<bool>(formatedUrl);
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
               var formatedUrl = string.Format(changeUserPassword, checkUserExist);
               var result = await _httpService.Get<bool>(formatedUrl);
               return result;
          }
     }
}
