using Maktab.Core.Interfaces.Models;
using System.Security.Claims;

namespace Maktab.Core.Interfaces.Services
{
     public interface IRoleMenuService : IApplicationService
     {
          IEnumerable<MenuItem> GetMenuForUser(ClaimsPrincipal user);
     }
}
