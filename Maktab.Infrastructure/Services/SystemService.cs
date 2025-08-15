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
    }
}
