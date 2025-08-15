
namespace Maktab.Core.Interfaces.Services
{
     public interface ISessionService
     {
          Task<object> CreateUserSession(string username, string password);
          Task<bool> LogoutAsync(bool removeLogInDetails = false);

          Task<bool> IsAuthenticatedAsync();

          Task<string> GetAuthTokenAsync();
          Task<Guid> GetLoggedInUserIdAsync();
          Task<Guid> GetLoggedInUserSessionIdAsync();

          Task<string> GetUserNameAsync();
          //Task<DateTime> GetSessionEndTime();
     }
}
