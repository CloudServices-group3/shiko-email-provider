using Azure.Communication.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Shiko.EmailProvider.API.Models;
using Shiko.EmailProvider.API.Services;

namespace Shiko.EmailProvider.Tests;

public class EmailServiceIntegrationTests
{
    private readonly EmailService _emailService;

    public EmailServiceIntegrationTests()
    {
        // read configuration from user secrets and environment variables, build configuration object
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<EmailServiceIntegrationTests>()
            .Build();

        // configure connection string for email client from configuration, throw exception if missing
        var emailClient = new EmailClient(
            configuration["AzureCommunicationServices:ConnectionString"]
                ?? throw new InvalidOperationException("ConnectionString missing i User Secrets."));

        // NullLogger used for testing, nothing gets logged during tests - used for dependency requirement of EmailService
        var logger = NullLogger<EmailService>.Instance;

        // create emailService with real email client, configuration and null logger
        _emailService = new EmailService(emailClient, configuration, logger);
    }

   
    [Fact] //(Skip = "Integrationtest - run manually")
    public async Task SendEmailAsync_ShouldReturnTrue_WhenEmailIsSentSuccessfully()
    {
        // ARRANGE 
        // create real message to send

        var message = new EmailVerificationMessage(
                 To: "tiwses@live.com",
                 VerificationCode: "123456"
 );

        // ACT 
        var result = await _emailService.SendEmailAsync(message);

        // ASSERT — verify that email was sent successfully
        Assert.True(result);
    }


}
