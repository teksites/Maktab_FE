using Maktab.Core.Interfaces.Services;

namespace Maktab.Infrastructure.Services
{
     public class SystemService : ISystemService
     {
          public string GetFormFactor()
          {
               return "WebAssembly";
          }

          public string GetPlatform()
          {
               return Environment.OSVersion.ToString();
          }

          public DateTime DateTimeUtcNow()
          {
               return DateTime.UtcNow;
          }

          public DateTime DateTimeNow()
          {
               return DateTime.UtcNow;
          }
     }
}
