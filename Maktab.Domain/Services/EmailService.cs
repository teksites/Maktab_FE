using Maktab.Core.Interfaces.Services;
using MaktabDataContracts.Requests.Email;
using Microsoft.Extensions.Logging;

namespace Maktab.Domain.Services
{
     public class EmailService : BaseService, IEmailService
     {
          // API Endpoints (properly formatted, not "formated")
          private const string sendEmail = @"/api/email/sendemail";

          private readonly ILogger<EmailService> _logger;

          public EmailService(IHttpService httpService,
                                  ILocalStorageService localStorageService,
                                  ILogger<EmailService> logger) 
                                  : base(httpService, localStorageService)
          {
               _logger = logger;
          }

          public async Task<bool> SendEmailAsync(SendEmailRequest emailRequest)
          {
               try
               {
                    // Validate input
                    if (emailRequest == null)
                    {
                         _logger.LogWarning("SendContactEmailAsync called with null email request");
                         throw new ArgumentNullException(nameof(emailRequest), "Email request cannot be null");
                    }

                    _logger.LogInformation("Sending email");

                    var result = await _httpService.Post<bool?>(sendEmail, emailRequest);

                    // Validate response
                    if (!result.HasValue)
                    {
                         _logger.LogError("Server returned null response when sending email");
                         throw new InvalidOperationException("Server did not return address confirmation");
                    }

                    if (result.Value)
                    {
                         _logger.LogInformation("Successfully sent email");
                    }
                    else
                    {
                         _logger.LogWarning("Failed to send email");
                    }

                    return result.Value;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error sending email");
                    throw;
               }
          }
     }
}
