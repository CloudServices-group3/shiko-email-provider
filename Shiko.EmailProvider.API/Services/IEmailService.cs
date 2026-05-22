using Shiko.EmailProvider.API.Models;

namespace Shiko.EmailProvider.API.Services;

public interface IEmailService
{
    Task<bool> SendEmailAsync(EmailVerificationMessage message);
}