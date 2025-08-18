
using MaktabDataContracts.Requests.Authentication;
using MaktabDataContracts.Responses.Authentication;

namespace Maktab.Core.Interfaces.Services
{
     public interface ISessionService : IDomainService
     {
          Task<bool> Login(UserLoginInformation loginInformation);
          Task<bool> LogoutAsync(bool removeLogInDetails = false);

          Task<bool> IsAuthenticatedAsync();

          Task<string> GetAuthTokenAsync();
          Task<Guid> GetLoggedInUserIdAsync();
          Task<Guid> GetLoggedInUserSessionIdAsync();

          Task<string> GetUserNameAsync();
          //Task<DateTime> GetSessionEndTime();
     }
}
