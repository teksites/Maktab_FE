using Maktab.Core.Interfaces.Services;
using MaktabDataContracts.Responses.Children;
using Microsoft.Extensions.Logging;

namespace Maktab.Domain.Services
{
     public class QuranService : BaseService, IQuranService
     {
          private const string getQuranSurahs = @"api/quran/surahs";

          private readonly ILogger<QuranService> _logger;

          public QuranService(IHttpService httpService, ILocalStorageService localStorageService, ILogger<QuranService> logger) 
          : base(httpService, localStorageService)
          {
               _logger = logger ?? throw new ArgumentNullException(nameof(logger));
          }

          public async Task<IReadOnlyCollection<QuranSurahOptionResponse>> GetSurahAsync()
          {
               try
               {
                    _logger.LogInformation("Fetching Quran surahs");

                    // Make request
                    var result = await _httpService.Get<List<QuranSurahOptionResponse>>(getQuranSurahs);

                    // Validate response
                    if (result?.Any() != true)
                    {
                         _logger.LogWarning("Quran surahs not found");
                         return Array.Empty<QuranSurahOptionResponse>();
                    }

                    _logger.LogInformation("Successfully fetched Quran surahs");
                    return result;
               }
               catch (HttpRequestException ex)
               {
                    _logger.LogError(ex, "Network error fetching Quran surahs");
                    throw;
               }
               catch (TaskCanceledException ex)
               {
                    _logger.LogError(ex, "Request timeout fetching Quran surahs");
                    throw;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Unexpected error fetching Quran surahs");
                    throw;
               }
          }
     }
}
