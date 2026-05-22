

var builder = WebApplication.CreateBuilder(args);

var serviceBusConnectionString = builder.Configuration["AzureServiceBus:ConnectionString"];
var serviceBusQueueName = builder.Configuration["AzureServiceBus:QueueName"];
var acsConnectionString = builder.Configuration["AzureCommunicationServices:ConnectionString"];

builder.Configuration.AddEnvironmentVariables();


var app = builder.Build();



app.Run();
