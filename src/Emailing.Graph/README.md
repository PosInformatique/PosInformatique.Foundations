### PosInformatique.Foundations.Emailing.Graph

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.Emailing.Graph)](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing.Graph/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.Emailing.Graph)](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing.Graph/)

## Introduction

[PosInformatique.Foundations.Emailing.Graph](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing.Graph/)
provides an `IEmailProvider`
implementation for [PosInformatique.Foundations.Emailing](../Emailing/README.md) based on the **Microsoft Graph** API.

It uses [Microsoft.Graph.GraphServiceClient](https://learn.microsoft.com/en-us/graph/sdks/create-client?tabs=csharp)
to send templated emails (created via `IEmailManager`)
through a Microsoft 365 mailbox, using Azure AD authentication.

Authentication is fully driven by an
[Azure.Core.TokenCredential](https://learn.microsoft.com/en-us/dotnet/api/azure.core.tokencredential?view=azure-dotnet)
instance, allowing you to use:

- Managed identity
- Client credentials (client id/secret or certificate)
- Interactive login, device code, etc.

## Install

You can install the package from [NuGet](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing.Graph/):

```powershell
dotnet add package PosInformatique.Foundations.Emailing.Graph
```

## Features

- `IEmailProvider` implementation using `Microsoft.Graph.GraphServiceClient`.
- Simple configuration through `AddEmailing().UseGraph(...)`.
- Authentication configured via `TokenCredential`:
  - `DefaultAzureCredential` (managed identity, VS, CLI, etc.)
  - `ClientSecretCredential`, `ClientCertificateCredential`, etc.
- Optional `baseUrl` parameter to customize the Graph endpoint (defaults to `https://graph.microsoft.com/v1.0`).
- Sends HTML emails using the `EmailMessage` produced by [PosInformatique.Foundations.Emailing](../Emailing/README.md).

## Basic configuration

### Using DefaultAzureCredential (managed identity or local dev)

```csharp
using Azure.Identity;
using Microsoft.Extensions.DependencyInjection;
using PosInformatique.Foundations.EmailAddresses;
using PosInformatique.Foundations.Emailing;
using PosInformatique.Foundations.Emailing.Graph;

var services = new ServiceCollection();

// TokenCredential for Microsoft Graph (for example: managed identity or local dev)
var credential = new DefaultAzureCredential();

services
    .AddEmailing(options =>
    {
        options.SenderEmailAddress = EmailAddress.Parse("sender@yourtenant.onmicrosoft.com");

        // Register your templates here...
        // options.RegisterTemplate(EmailTemplateIdentifiers.Invitation, invitationTemplate);
    })
    .UseGraph(credential);
```

### Using client credentials (app registration)

If you want to authenticate with a client id / tenant id / client secret:

```csharp
using Azure.Identity;
using PosInformatique.Foundations.Emailing.Graph;

var tenantId = configuration["AzureAd:TenantId"];
var clientId = configuration["AzureAd:ClientId"];
var clientSecret = configuration["AzureAd:ClientSecret"];

var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

services
    .AddEmailing(options =>
    {
        options.SenderEmailAddress = EmailAddress.Parse("sender@yourtenant.onmicrosoft.com");
    })
    .UseGraph(credential);
```

The `TokenCredential` you provide is responsible for acquiring tokens for the Microsoft Graph API. The provider does not manage scopes or credentials itself; this is entirely delegated to the credential implementation.

### Custom Graph endpoint

You can optionally customize the Graph base URL (for example, for national clouds):

```csharp
var baseUrl = "https://graph.microsoft.com/v1.0/beta";

services
    .AddEmailing(options =>
    {
        options.SenderEmailAddress = EmailAddress.Parse("sender@yourtenant.onmicrosoft.com");
    })
    .UseGraph(credential, baseUrl);
```

If `baseUrl` is `null`, `https://graph.microsoft.com/v1.0` is used by default.

## Typical end-to-end usage

1. Configure emailing (sender address, templates) with `AddEmailing(...)`.
2. Configure the Graph provider with `UseGraph(TokenCredential, baseUrl?)`.
3. Inject `IEmailManager` and create emails from template identifiers.
4. Add recipients and models.
5. Call `SendAsync(...)` to send emails via Microsoft Graph.

## Links

- [NuGet package: Emailing](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing/)
s- [Microsoft Graph .NET SDK](https://learn.microsoft.com/graph/sdks/sdks-overview)
- [Azure Identity (TokenCredential)](https://learn.microsoft.com/dotnet/azure/sdk/authentication/)
- [Source code](https://github.com/PosInformatique/PosInformatique.Foundations)