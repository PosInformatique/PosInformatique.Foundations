# PosInformatique.Foundations.Emailing.Mailjet

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.Emailing.Mailjet)](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing.Mailjet/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.Emailing.Mailjet)](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing.Mailjet/)

## Introduction

[PosInformatique.Foundations.Emailing.Mailjet](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing.Mailjet/) provides an `IEmailProvider`
implementation for [PosInformatique.Foundations.Emailing](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing/)
based on the `IMailjetClient` from [Mailjet.Api](https://www.nuget.org/packages/Mailjet.Api/).

It allows you to send templated emails (created via `IEmailManager`) using **Mailjet**.

## Install

You can install the package from [NuGet](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing.Mailjet/):

```powershell
dotnet add package PosInformatique.Foundations.Emailing.Mailjet
```

## Features

- `IEmailProvider` implementation using [Mailjet.Client.IMailjetClient](https://github.com/mailjet/mailjet-apiv3-dotnet).
- Simple configuration through `AddEmailing().UseMailjet(...)`.
- Supports configuration via the Mailjet **API key** and **API secret**.
- Registers and configures a named `HttpClient` (via `IHttpClientFactory`) used to communicate with the Mailjet API, so you can further customize it (handlers, timeouts, resilience policies, etc.) if needed.

## Basic configuration

### Using API key and secret

```csharp
using Microsoft.Extensions.DependencyInjection;
using PosInformatique.Foundations.EmailAddresses;

var services = new ServiceCollection();

// Your Mailjet API credentials
var apiKey = configuration["Mailjet:ApiKey"];
var apiSecret = configuration["Mailjet:ApiSecret"];

services
    .AddEmailing(options =>
    {
        options.SenderEmailAddress = EmailAddress.Parse("no-reply@myapp.com");

        // Register your templates here...
        // options.RegisterTemplate(EmailTemplateIdentifiers.Invitation, invitationTemplate);
    })
    .UseMailjet(apiKey, apiSecret);
```

## Typical usage end-to-end

1. Configure emailing and templates with `AddEmailing(...)`.
2. Configure the Mailjet provider using `UseMailjet(...)`.
3. Inject `IEmailManager` and create an email from a template identifier.
4. Add recipients and models.
5. Call `SendAsync(...)` to send via Mailjet.

## Links

- [NuGet package: Emailing (core library)](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing/)
- [NuGet package: Emailing.Mailjet](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing.Mailjet/)
- [Source code](https://github.com/PosInformatique/PosInformatique.Foundations)
- [Mailjet API documentation](https://dev.mailjet.com/email/guides/)
