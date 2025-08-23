using Maktab.Core.Interfaces.Services;
using Maktab.Domain.Helpers;
using MaktabDataContracts.Requests.Authentication;
using MaktabDataContracts.Responses.Authentication;

namespace Maktab.Domain.Services
{
     public class SessionService : BaseService, ISessionService
     {
          private const string loginUrl = @"/api/users/session/login";
          private const string logoutUrl = @"/api/users/session/{0}/logout";

          public SessionService(IHttpService httpService, ILocalStorageService localStorageService)
           : base(httpService, localStorageService)
          {
               
          }

          public async Task<bool> Login(UserLoginInformation loginInformation)
          {
               var isloggedIn = false;

               var authenticationReponse = await _httpService.Post<AuthenticationResponse>(loginUrl, loginInformation, false);
               if (authenticationReponse != null)
               {
                    //await _localStorageService.SetItem(_userKey, User);

                    await _localStorageService.SetItem(Constants.CurrentUserNameKey, loginInformation.UserName);
                    await _localStorageService.SetItem(Constants.CurrentUserIdKey, authenticationReponse.UserId);
                    await _localStorageService.SetItem(Constants.SessionIdKey, authenticationReponse.SessionId);

                    await _localStorageService.SetItem(Constants.AccessTokenKey, authenticationReponse.AccessToken);
                    await _localStorageService.SetItem(Constants.RefreshTokenKey, authenticationReponse.RefreshToken);

                    await _localStorageService.SetItem(Constants.SessionStartTimeKey, authenticationReponse.LoginTime);
                    await _localStorageService.SetItem(Constants.SessionEndTimeKey, authenticationReponse.ExpiresIn);

                    await _localStorageService.SetItem<bool>(Constants.AuthorizationStateKey, true);

                    isloggedIn = true;
               }
               else
               {
                    throw new UnauthorizedAccessException();
               }

               return isloggedIn;
          }

          public async Task<bool> IsAuthenticatedAsync()
          {
               var authState = await _localStorageService.GetItem<bool>(Constants.AuthorizationStateKey);
               if (authState)
               {
                    var expiredTime = await GetSessionEndTime();
                    if (expiredTime <= DateTime.UtcNow)
                    {
                         authState = false;
                         await this.LogoutAsync();
                    }
               }
               return authState;
          }

          public async Task<string> GetAuthTokenAsync()
          {
               var accessToken = await _localStorageService.GetItem<String>(Constants.AccessTokenKey, string.Empty);
               return accessToken;
          }

         

          public async Task<Guid> GetLoggedInUserSessionIdAsync()
          {
               var sessionId = await _localStorageService.GetItem<Guid>(Constants.SessionIdKey, Guid.Empty);
               return sessionId;
          }

          public async Task<Guid> GetLoggedInUserIdAsync()
          {
               var userId = await _localStorageService.GetItem<Guid>(Constants.CurrentUserIdKey, Guid.Empty);
               return userId;
          }

          public async Task<string> GetUserNameAsync()
          {
               var username = await _localStorageService.GetItem<String>(Constants.CurrentUserNameKey, string.Empty);
               return username;
          }

          public async Task<bool> LogoutAsync(bool removeLogInDetails = false)
          {
               var sessionId = await this.GetLoggedInUserSessionIdAsync();
               return await LogoutSession(sessionId);
          }

          public async Task<DateTime> GetSessionEndTime()
          {
               var sessionEndTime = await _localStorageService.GetItem<DateTime>(Constants.SessionEndTimeKey, DateTime.UtcNow);
               return sessionEndTime;
          }

          protected async Task<bool> LogoutSession(Guid sessionId)
          {
               try
               {
                    var formatedUrl = string.Format(logoutUrl, sessionId);
                    await _httpService.Put("formatedUrl", false);
                    {
                         await CleanLocalStorage();
                         return true;
                    }
               }
               catch (Exception ex)
               {
                    await CleanLocalStorage();
               }

               return false;
          }

          private async Task CleanLocalStorage()
          {
               await _localStorageService.RemoveItem(Constants.SessionIdKey);
               await _localStorageService.RemoveItem(Constants.AccessTokenKey);
               await _localStorageService.RemoveItem(Constants.RefreshTokenKey);

               await _localStorageService.RemoveItem(Constants.CurrentUserIdKey);

               await _localStorageService.RemoveItem(Constants.SessionStartTimeKey);
               await _localStorageService.RemoveItem(Constants.SessionEndTimeKey);

               await _localStorageService.RemoveItem(Constants.AuthorizationStateKey);
          }
     }
}
