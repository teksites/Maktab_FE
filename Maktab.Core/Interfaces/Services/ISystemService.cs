namespace Maktab.Core.Interfaces.Services
{
    public interface ISystemService : IApplicationService
    {
        public string GetFormFactor();
        public string GetPlatform();
        public DateTime DateTimeUtcNow();
          DateTime DateTimeNow();
     }
}
