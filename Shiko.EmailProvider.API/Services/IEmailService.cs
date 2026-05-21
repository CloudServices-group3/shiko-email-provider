namespace Shiko.EmailProvider.API.Services;

public interface IEmailService
{
    Task<bool> SendVerificationEmailAsync(string toEmail, string verificationCode);
}
