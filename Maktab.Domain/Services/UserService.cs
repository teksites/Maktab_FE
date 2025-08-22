using Maktab.Core.Interfaces.Services;
using MaktabDataContracts.Requests.Users;
using MaktabDataContracts.Responses.Users;

namespace Maktab.Domain.Services
{
     public class UserService : BaseService, IUserService
     {
          private const string getUserById = @"/api/users/{0}";
          private const string validateUserActivationCode = @"/api/users/{0}/verify";
          private const string sendActivationCode = @"/api/users/{0}/activationcode";



          public UserService(IHttpService httpService, ILocalStorageService localStorageService) 
          : base(httpService, localStorageService)
          {
          }

          public async Task<UserInformationResponse> GetUserById(Guid userId)
          {
               var formatedUrl = string.Format(getUserById, userId);
               var result = await _httpService.Get<UserInformationResponse>(formatedUrl);
               return result;
          }

          public async Task<bool> ValidateUserByActivationCode(Guid userId, UserVerificationRequest request)
          {
               var formatedUrl = string.Format(validateUserActivationCode, userId);
               var result = await _httpService.Get<bool>(formatedUrl);
               return result;
          }

          public async Task<bool> SendUserActivationCode(Guid userId)
          {
               var formatedUrl = string.Format(sendActivationCode, userId);
               var result = await _httpService.Put<bool>(formatedUrl, null);
               return result;
          }
     }
}
