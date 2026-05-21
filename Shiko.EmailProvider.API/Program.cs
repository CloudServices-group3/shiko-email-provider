var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();


var app = builder.Build();



app.Run();
