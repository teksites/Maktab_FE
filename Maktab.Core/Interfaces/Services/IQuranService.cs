using MaktabDataContracts.Responses.Children;

namespace Maktab.Core.Interfaces.Services
{
     public interface IQuranService : IApplicationService
     {
          Task<IReadOnlyCollection<QuranSurahOptionResponse>> GetSurahAsync();
     }
}
