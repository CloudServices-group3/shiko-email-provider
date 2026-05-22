using Azure;
using Azure.Communication.Email;
using Shiko.EmailProvider.API.Models;

namespace Shiko.EmailProvider.API.Services;

public class EmailService (
    
    IConfiguration configuration,
    ILogger<EmailService> logger) : IEmailService
{
    private readonly EmailClient emailClient = new(configuration["AzureCommunicationServices:ConnectionString"]
         ?? throw new InvalidOperationException("ConnectionString is not configured."));

    private readonly string senderEmail = configuration["AzureCommunicationServices:SenderEmail"]
         ?? throw new InvalidOperationException("SenderEmail is not configured.");

    public async Task<bool> SendEmailAsync(EmailVerificationMessage message)
    {
        try
        {
            logger.LogInformation($"Sending verification email to {message.To}...");

            var emailSubject = "Welcome to Shiko - Verify Your Account";

            // create content for email
            var emailContent = new EmailContent(emailSubject)
            {
                Html = $@"
              <table width='600' cellpadding='0' cellspacing='0'>
                  <tr>
                      <td style='font-family: Arial, sans-serif; padding: 20px;'>
                          <h2 style='color: #333;'>Welcome to Shiko!</h2>
                          <p>Here is your verification code:</p>
                            <table width='100%'>
                             <tr>
                                <td style='background-color: #f4f4f4; padding: 15px; text-align: center; font-size: 24px;'>
                                   {message.VerificationCode}
                                </td>
                             </tr>
                   
                           </table>
                      </td>
                    </tr>
               </table>"
                  };

            // create email message (default class from azure.communication.email)
            var emailMessage = new EmailMessage(
                senderAddress: senderEmail,
                recipientAddress: message.To,
                content: emailContent
            );

            // send email and log information
            EmailSendOperation emailSendOperation = await emailClient.SendAsync(WaitUntil.Completed, emailMessage);

            logger.LogInformation($"Email sent successfully! MessageId: {emailSendOperation.Id}");
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Could not send mail to {message.To} due to an error.");
            return false;
        }
    }
}