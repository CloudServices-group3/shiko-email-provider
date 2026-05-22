using Azure.Messaging.ServiceBus;
using Shiko.EmailProvider.API.Models;
using Shiko.EmailProvider.API.Services;
using System.Text.Json;

namespace Shiko.EmailProvider.API.Workers;

public class EmailQueueSubscriber (

    ServiceBusClient serviceBusClient,  //SDK client to communicate with the service bus
    IConfiguration configuration,
    IServiceProvider serviceProvider,
    ILogger<EmailQueueSubscriber> logger) : BackgroundService

{
    // create a service bus processor with method from clilent and queue name from configuration
    private readonly ServiceBusProcessor processor = serviceBusClient.CreateProcessor(
         configuration["AzureServiceBus:QueueName"],
         new ServiceBusProcessorOptions { AutoCompleteMessages = false }
     );

    // method to listen to Azure Service Bus using Service Bus Processor
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        // register methods to handle messages and errors,
        // message handler will be called every time a new message arrives in the queue (listen to event) and process it
        // error handler will be called if there is an error while processing messages 
        processor.ProcessMessageAsync += MessageHandler;
        processor.ProcessErrorAsync += ErrorHandler;

        // start subscriber
        logger.LogInformation("EmailQueueSubscriber starting...");
        await processor.StartProcessingAsync(ct);

        // keep service running until cancellation is requested
        while (!ct.IsCancellationRequested)
        {
            await Task.Delay(1000, ct); // "listen" to queue every 1 second, to keep the service alive.
        }

        // stop subscriber when cancellation is requested, but keep handle any messages that are still being processed before shutting down
        logger.LogInformation("EmailQueueSubscriber shutting down...");
        await processor.StopProcessingAsync();
    }

    private async Task MessageHandler(ProcessMessageEventArgs args)
    {
        try
        {
            // get message (JSON) from Service Bus
            string body = args.Message.Body.ToString();
            logger.LogInformation($"Message arrived from queue: {body}");

            // deserialize and convert to EmailVerificationMessage object (DTO)
            var emailRequest = JsonSerializer.Deserialize<EmailVerificationMessage>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (emailRequest != null)
            {
                // BackgroundService is "Singleton" -> create a temporary scope
                //to be able to use IEmailService (if registered as Scoped/Transient)
                using var scope = serviceProvider.CreateScope();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                // SEND EMAIL!
                // call method in email service to send email with emailRequest (data from message)
                // await emailService.SendEmailAsync(emailRequest);

                logger.LogInformation($"Email sent to {emailRequest.To}!");
            }

            // complete message in queue (remove it from the queue) if everything went well
            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occured trying to handle message.");
            // If error in try ->  CompleteMessageAsync is never called, 
            //Message returns to queue and tries again (max 10 times, as configured in azure).
        }
    }

    // method to handle errors that occur during message processing, ex network failure or other infrastructure issues,
    // logger gets the exception message from EventArgs and the method returns a task 
    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        logger.LogError($"ServiceBus Infrastructure Error: {args.Exception.Message}");
        return Task.CompletedTask;
    }

    //method to stop/shut down the subscriber, dispose of the processor and call base method to stop the background service
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await processor.DisposeAsync();
        await base.StopAsync(cancellationToken);
    }
}
