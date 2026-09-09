using System.Threading.Tasks;

namespace Maktab.Core.Interfaces.Services
{
     public interface IIpHelperService : IService
     {
          Task<string> GetUserIp();
     }
}
