using MaktabDataContracts.Requests.Helcim;
using MaktabDataContracts.Responses.Helcim;

namespace Maktab.Core.Interfaces.Services
{
     /// <summary>
     /// Defines the contract for Helcim payment method services, providing operations to manage payment methods
     /// </summary>
     public interface IHelcimPaymentMethodService
     {
          /// <summary>
          /// Initializes a payment using the provided payment request.
          /// </summary>
          /// <param name="paymentRequest">The request containing payment initialization details.</param>
          /// <returns>The response containing payment initialization results.</returns>
          Task<HelcimPayInitializeResponse> InitializePaymentAsync(InitiatePaymentRequest paymentRequest);
     }
}