
using Microsoft.Extensions.Azure;
using Shiko.EmailProvider.API.Services;
using Shiko.EmailProvider.API.Workers;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddEnvironmentVariables();

//transient - creates a new instance of the service every time it is requested
builder.Services.AddTransient<IEmailService, EmailService>();

// hostedservice - runs in the background and listens to Azure Service Bus queue for incoming messages
builder.Services.AddHostedService<EmailQueueSubscriber>();

// AddAzureClients from Microsoft.Extensions.Azure    -> azure cLientFactory to create and manage Azure SDK clients
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddServiceBusClient(builder.Configuration["AzureServiceBus:ConnectionString"])
                 .WithName("ServiceBusClient");
});

var app = builder.Build();



app.Run();
