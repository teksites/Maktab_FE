using Maktab.Core.Interfaces.Services;
using Microsoft.JSInterop;

namespace Maktab.Consumer.Services
{
     public class IpHelperService : IIpHelperService
     {
          private IJSRuntime _jsRuntime;

          public IpHelperService(IJSRuntime jsRuntime)
          {
               _jsRuntime = jsRuntime;
          }

          public async Task<string> GetUserIp()
          {
               string userIp = await _jsRuntime.InvokeAsync<string>("ipHelper.getClientIp");
               if (userIp == null)
               { 
                    return string.Empty;
               }

               return userIp;
          }
     }
}
