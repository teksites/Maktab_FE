using Maktab.Core.Interfaces.Services;
using MaktabDataContracts.Requests.Helcim;
using MaktabDataContracts.Responses.Helcim;
using Microsoft.Extensions.Logging;

namespace Maktab.Domain.Services
{
     /// <summary>
     /// Helcim Payment service with comprehensive error handling, validation, and logging.
     /// Production-ready implementation for backend API integration.
     /// </summary>
     public class HelcimPaymentMethodService : BaseService, IHelcimPaymentMethodService
     {
          // API Endpoints (properly formatted, not "formated")
          private const string initializePaymentUrl  = @"/api/helcim/initialize-payment";

          private readonly ILogger<HelcimPaymentMethodService> _logger;

          public HelcimPaymentMethodService(
               IHttpService httpService,
               ILocalStorageService localStorageService,
               ILogger<HelcimPaymentMethodService> logger)
               : base(httpService, localStorageService)
          {
               _logger = logger ?? throw new ArgumentNullException(nameof(logger));
          }

          /// <summary>
          /// Initializes a payment with validation and error handling.
          /// </summary>
          public async Task<HelcimPayInitializeResponse> InitializePaymentAsync(InitiatePaymentRequest paymentRequest)
          {
               try
               {
                    if (paymentRequest == null)
                    {
                         _logger.LogWarning("InitializePaymentAsync called with null payment request");
                         throw new ArgumentNullException(nameof(paymentRequest), "Payment request cannot be null");
                    }

                    _logger.LogInformation("Initializing new payment");

                    var result = await _httpService.Post<HelcimPayInitializeResponse>(initializePaymentUrl, paymentRequest);

                    if (result == null)
                    {
                         _logger.LogError("Server returned null response when initializing payment");
                         throw new InvalidOperationException("Server did not return payment confirmation");
                    }

                    _logger.LogInformation("Successfully initialized payment");
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error initializing payment");
                    throw;
               }
          }
     }
}
