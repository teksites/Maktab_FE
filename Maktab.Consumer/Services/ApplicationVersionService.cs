using Maktab.Core.Interfaces.Services;
using System.Reflection;

namespace Maktab.Consumer.Services;
public class ApplicationVersionService : IApplicationVersionService
{
    public string GetApplicationVersion()
    {
        var version = Assembly.GetExecutingAssembly()
            .GetName()
            .Version;

        return version?.ToString() ?? "Unknown";
    }
}