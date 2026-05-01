using Maktab.Core.Interfaces.Services;
using Microsoft.JSInterop;

namespace Maktab.Consumer.Services
{
     public class ClipboardService : IClipboardService
     {
          private IJSRuntime _jsRuntime;

          public ClipboardService(IJSRuntime jsRuntime)
          {
               _jsRuntime = jsRuntime;
          }

          public async Task CopyTextToClipboard(string textToCopied)
          {
               if (!string.IsNullOrWhiteSpace(textToCopied))
               {
                    await _jsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", textToCopied);
               }
          }
     }
}
