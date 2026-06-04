# Shiko Email Provider 

Microservice responsible for sending verification emails to users registering in the Shiko system. Listens to an Azure Service Bus queue for incoming email verification messages and delivers them via Azure Communication Services (ACS).
This service is deployed to Azure and runs as part of the Shiko microservices architecture.


## 🚀 Features

- **Service Bus Integration**: Listens to an Azure Service Bus queue for incoming `EmailVerificationMessage` events published by the Email Verification API.
- **Email Delivery**: Sends HTML-formatted verification emails containing a unique verification code via Azure Communication Services.
- **Background Processing**: Runs as a hosted service, continuously listening for new messages without blocking the application.


## 🔗 Related Services

**Shiko Verification API:** In charge of creating the verification code and publish message to Azure Service Bus.

**Shiko Frontend:** [https://github.com/CloudServices-group3/shiko-frontend](https://github.com/CloudServices-group3/shiko-frontend) 
