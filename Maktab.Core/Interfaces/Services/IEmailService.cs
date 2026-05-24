using Maktab.Core.Interfaces.Services;
using MaktabDataContracts.Requests.Email;

public interface IEmailService : IDomainService
{
     Task<bool> SendEmailAsync(SendEmailRequest request);
}