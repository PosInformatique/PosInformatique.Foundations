# PosInformatique.Foundations.Emailing.Azure

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.Emailing.Azure)](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing.Azure/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.Emailing.Azure)](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing.Azure/)

## Introduction

[PosInformatique.Foundations.Emailing.Azure](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing.Azure/) provides an `IEmailProvider`
implementation for [PosInformatique.Foundations.Emailing](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing/)
based on the `EmailClient` from [Azure.Communication.Email](https://www.nuget.org/packages/Azure.Communication.Email).

It allows you to send templated emails (created via `IEmailManager`) using **Azure Communication Service**.

## Install

You can install the package from [NuGet](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing.Azure/):

```powershell
dotnet add package PosInformatique.Foundations.Emailing.Azure
```

## Features

- `IEmailProvider` implementation using [Azure.Communication.Email.EmailClient](https://learn.microsoft.com/en-us/dotnet/api/azure.communication.email.emailclient?view=azure-dotnet).
- Simple configuration through `AddEmailing().UseAzureCommunicationService(...)`.
- Supports configuration via:
  - Azure Communication Service **connection string**, or
  - Azure Communication Service **endpoint URI**.
- Callback to configure `EmailClient` / `EmailClientOptions`:
  - Authentication (managed identity, connection string, etc.).
  - Retry policy, logging, diagnostics, and other Azure client options.

## Basic configuration

### Using connection string

```csharp
using Microsoft.Extensions.DependencyInjection;
using PosInformatique.Foundations.EmailAddresses;
using PosInformatique.Foundations.Emailing;
using PosInformatique.Foundations.Emailing.Azure;

var services = new ServiceCollection();

// Your ACS connection string
var acsConnectionString = configuration["AzureCommunicationService:ConnectionString"];

services
    .AddEmailing(options =>
    {
        options.SenderEmailAddress = EmailAddress.Parse("no-reply@myapp.com");

        // Register your templates here...
        // options.RegisterTemplate(EmailTemplateIdentifiers.Invitation, invitationTemplate);
    })
    .UseAzureCommunicationService(acsConnectionString);
```

### Using endpoint URI and Azure identity (managed identity)

You can also configure the provider using the ACS endpoint URI, and configure authentication
(for example with managed identity) and client options using the `clientBuilder` parameter.

```csharp
using Azure.Communication.Email;
using Azure.Identity;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using PosInformatique.Foundations.Emailing;
using PosInformatique.Foundations.Emailing.Azure;

var services = new ServiceCollection();

var acsEndpoint = new Uri(configuration["AzureCommunicationService:Endpoint"]);

services
    .AddEmailing(options =>
    {
        options.SenderEmailAddress = EmailAddress.Parse("no-reply@myapp.com");

        // Register your templates here...
    })
    .UseAzureCommunicationService(
        acsEndpoint,
        clientBuilder =>
        {
            // Configure EmailClient and EmailClientOptions here

            clientBuilder.ConfigureOptions((EmailClientOptions options) =>
            {
                // Example: configure retry, diagnostics, etc.
                options.Retry.MaxRetries = 5;
            });

            // Example: use managed identity for authentication
            clientBuilder.WithCredential(new DefaultAzureCredential());
        });
```

## Typical usage end-to-end

1. Configure emailing and templates with `AddEmailing(...)`.
2. Configure the Azure provider using `UseAzureCommunicationService(...)`.
3. Inject `IEmailManager` and create an email from a template identifier.
4. Add recipients and models.
5. Call `SendAsync(...)` to send via Azure Communication Service.

## Links

- [NuGet package: Emailing (core library)](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing/)
- [NuGet package: Emailing.Azure](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing.Azure/)
- [Source code](https://github.com/PosInformatique/PosInformatique.Foundations)
- [Azure Communication Services – Email documentation](https://learn.microsoft.com/azure/communication-services/concepts/email/)