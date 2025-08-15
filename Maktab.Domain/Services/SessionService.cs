using Maktab.Core.Interfaces.Services;
using Maktab.Domain.Helpers;

namespace Maktab.Domain.Services
{
     internal class SessionService : ISessionService
     {
          private const string loginUrl = @"/api/users/session/login";
          private const string logoutUrl = @"/api/users/session/{0}/logout";



          private IHttpService _httpService;
          private ILocalStorageService _localStorageService;


          public SessionService(IHttpService httpService, ILocalStorageService localStorageService)
          {
               _httpService = httpService;
               _localStorageService = localStorageService;
          }

          public Task<object> CreateUserSession(string username, string password)
          {
               throw new NotImplementedException();

               _localStorageService.SetItem(Constants.CurrentUserNameKey, username);
               _localStorageService.SetItem(Constants.CurrentUserIdKey, "");
               _localStorageService.SetItem(Constants.SessionIdKey, "");

               _localStorageService.SetItem(Constants.AccessTokenKey, "");
               _localStorageService.SetItem(Constants.RefreshTokenKey, "");

               _localStorageService.SetItem(Constants.SessionStartTimeKey, "");
               _localStorageService.SetItem(Constants.SessionEndTimeKey, "");

               _localStorageService.SetItem(Constants.AuthorizationStateKey, "");


          }

          public async Task<bool> IsAuthenticatedAsync()
          {
               var authState = await _localStorageService.GetItem<bool>(Constants.AuthorizationStateKey);
               if (authState)
               {
                    var expiredTime = await GetSessionEndTime();
                    if (expiredTime <= DateTime.UtcNow)
                    {
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
               var formatedUrl = string.Format(logoutUrl, sessionId);
               await _httpService.Put("formatedUrl");
               {
                    await _localStorageService.RemoveItem(Constants.SessionIdKey);
                    await _localStorageService.RemoveItem(Constants.AccessTokenKey);
                    await _localStorageService.RemoveItem(Constants.RefreshTokenKey);

                    await _localStorageService.RemoveItem(Constants.CurrentUserIdKey);

                    await _localStorageService.RemoveItem(Constants.SessionStartTimeKey);
                    await _localStorageService.RemoveItem(Constants.SessionEndTimeKey);

                    await _localStorageService.SetItem<bool>(Constants.AuthorizationStateKey, false);

                    return true;
               }
          }
     }
}
