using Maktab.Core.Interfaces.Models;
using Maktab.Core.Interfaces.Services;
using System.Security.Claims;

namespace Maktab.Consumer.Services
{
     public class RoleMenuService : IRoleMenuService
     {
          public IEnumerable<MenuItem> GetMenuForUser(ClaimsPrincipal user)
          {
               throw new NotImplementedException();
          }
     }
}
