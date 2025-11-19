using Maktab.Core.Interfaces.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Maktab.Consumer.Helpers
{
     public class AuthStateProvider : AuthenticationStateProvider
     {
          private readonly ISessionService _sessionService;
          private ClaimsPrincipal _currentUser = new(new ClaimsIdentity());

          public AuthStateProvider(ISessionService sessionService)
          {
               _sessionService = sessionService;
          }

          public string Username
          {
               get; private set;
          }

          public async override Task<AuthenticationState> GetAuthenticationStateAsync()
          {
               var token = await _sessionService.GetAuthTokenAsync();
               if (string.IsNullOrEmpty(token))
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

               var identity = ParseClaimsFromJwt(token);
               _currentUser = new ClaimsPrincipal(identity);

               Username = await _sessionService.GetUserNameAsync();
               return new AuthenticationState(_currentUser);
          }

          public async void NotifyUserAuthentication()
          {
               var token = await _sessionService.GetAuthTokenAsync();

               if (!string.IsNullOrEmpty(token))
               {
                    var identity = ParseClaimsFromJwt(token);
                    _currentUser = new ClaimsPrincipal(identity);

                    Username = await _sessionService.GetUserNameAsync();
               }
               else
               {
                    _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
               }

               NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
          }

          public void NotifyUserLogout()
          {
               _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
               NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
          }

          private static ClaimsIdentity ParseClaimsFromJwt(string jwt)
          {
               var handler = new JwtSecurityTokenHandler();
               var token = handler.ReadJwtToken(jwt);
               return new ClaimsIdentity(token.Claims, "jwt");
          }
     }

}
