# Shiko Email Provider API

Microservice responsible for sending verification emails to users registering in the Shiko system. Listens to an Azure Service Bus queue for incoming email verification messages and delivers them via Azure Communication Services (ACS).


## 🚀 Features

- **Service Bus Integration**: Listens to an Azure Service Bus queue for incoming `EmailVerificationMessage` events published by the Email Verification API.
- **Email Delivery**: Sends HTML-formatted verification emails containing a unique verification code via Azure Communication Services.
- **Background Processing**: Runs as a hosted service, continuously listening for new messages without blocking the application.


## 🛠️ Technologies

- C# .NET 10 (ASP.NET Core)
- Azure Communication Services (ACS) – Email
- Azure Service Bus – Message queue
- `Azure.Communication.Email` – Email client
- `Microsoft.Extensions.Azure` – Azure client factory


## 🏁 Getting Started

This service is deployed to Azure and runs as part of the Shiko microservices architecture. For local development, follow the steps below.


## 🔗 Related Services
