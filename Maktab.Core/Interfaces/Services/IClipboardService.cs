namespace Maktab.Core.Interfaces.Services
{
     public interface IClipboardService : IApplicationService
     {
          Task CopyTextToClipboard(string textToCopied);
     }
}
