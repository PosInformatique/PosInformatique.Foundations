# PosInformatique.Foundations.Emailing

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.Emailing)](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.Emailing)](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing/)

## Introduction

`PosInformatique.Foundations.Emailing` provides a lightweight, template-based emailing infrastructure built on top of dependency injection.

It allows you to:

- Define strongly-typed email templates associated with data models.
- Instantiate emails from registered templates via an `IEmailManager`.
- Generate and send templated emails for each recipient through an `IEmailProvider` implementation.

The actual transport (SMTP, Azure Communication Service, Graph API, etc.) is delegated to a provider implementation.
Existing implementation are available in the following packages:
- Azure Communication Service: [PosInformatique.Foundations.Emailing.Azure](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing.Azure/).
- Microsoft Graph API: [PosInformatique.Foundations.Emailing.Graph](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing.Graph/).

## Install

You can install the package from [NuGet](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing/):

```powershell
dotnet add package PosInformatique.Foundations.Emailing
```

This package also depends on:

- [PosInformatique.Foundations.EmailAddresses](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses/)
- [PosInformatique.Foundations.Text.Templating](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating/)
and one of its concrete implementations (for example
[PosInformatique.Foundations.Text.Templating.Razor](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating.Razor/) or
[PosInformatique.Foundations.Text.Templating.Scriban](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating.Scriban/)).

## Features

- Registration of email templates through `AddEmailing(...)` and `EmailingOptions.RegisterTemplate(...)`.
- Strongly-typed template identifiers via `EmailTemplateIdentifier<TModel>`.
- Data models for templates based on any custom class.
- Template-based subject and HTML body using `TextTemplate<TModel>` (e.g. Razor or Scriban).
- Per-recipient data injection using a model with `EmailRecipient<TModel>`.
- Central `IEmailManager` to create and send emails.
- Pluggable `IEmailProvider` to send the final `EmailMessage` (transport-agnostic design).
- Support of the *Importance* of the e-mails (**Low, Normal and High).

## Basic concepts

### Email models

Each email template is associated with a data model.
This model is injected into the subject and HTML body templates when generating the email content for a recipient.

```csharp
public sealed class InvitationEmailTemplateModel
{
    public string FirstName { get; set; } = string.Empty;
    public string InvitationLink { get; set; } = string.Empty;
}

public sealed class AccountDeletionEmailTemplateModel
{
    public string FirstName { get; set; } = string.Empty;
    public DateTime DeletionDate { get; set; }
}
```

### Template identifiers

Templates are registered and referenced through an `EmailTemplateIdentifier<TModel>`.
It is recommended to centralize them in a static class used by your business code:

```csharp
public static class EmailTemplateIdentifiers
{
    public static EmailTemplateIdentifier<InvitationEmailTemplateModel> Invitation { get; } =
        EmailTemplateIdentifier<InvitationEmailTemplateModel>.Create();

    public static EmailTemplateIdentifier<AccountDeletionEmailTemplateModel> AccountDeletion { get; } =
        EmailTemplateIdentifier<AccountDeletionEmailTemplateModel>.Create();
}
```

### Email templates

An `EmailTemplate<TModel>` is composed of two `TextTemplate<TModel>` instances:

- One for the subject.
- One for the HTML body.

These `TextTemplate<TModel>` come from `PosInformatique.Foundations.Text.Templating`, share the same model
instance during the e-mail generation process, and can be implemented using, for example:

- [PosInformatique.Foundations.Text.Templating.Razor](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating.Razor/)
- [PosInformatique.Foundations.Text.Templating.Scriban](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating.Scriban/)

```csharp
var invitationSubjectTemplate = new RazorTextTemplate<InvitationEmailTemplateModel>(typeof(InvitationSubjectRazorComponent));
var invitationHtmlBodyTemplate = new RazorTextTemplate<InvitationEmailTemplateModel>(typeof(InvitationBodyRazorComponent));

var invitationTemplate = new EmailTemplate<InvitationEmailTemplateModel>(
    invitationSubjectTemplate,
    invitationHtmlBodyTemplate);
```

## Configuration

### Register the emailing feature

You register the emailing feature in your `IServiceCollection` via `AddEmailing(...)`.
In the options, you must at least configure:

- The sender email address.
- The mapping between `EmailTemplateIdentifier<TModel>` and `EmailTemplate<TModel>`.

```csharp
using Microsoft.Extensions.DependencyInjection;
using PosInformatique.Foundations.EmailAddresses;
using PosInformatique.Foundations.Emailing;

var services = new ServiceCollection();

services.AddEmailing(options =>
{
    // Required: sender email address used for all outgoing emails
    options.SenderEmailAddress = EmailAddress.Parse("no-reply@myapp.com");

    // Register templates with their identifiers
    options.RegisterTemplate(EmailTemplateIdentifiers.Invitation, invitationTemplate);
    options.RegisterTemplate(EmailTemplateIdentifiers.AccountDeletion, accountDeletionTemplate);
});
```

> **Important:**
> The `AddEmailing()` method registers a scoped implementation of `IEmailManager`.
> This is required because email templates (for example Razor-based templates) may depend on scoped services
> that are tied to the currently authenticated user.
> As a consequence, every service that depends on `IEmailManager` must also be registered with a scoped lifetime.

The `AddEmailing()` method returns an `EmailingBuilder` that can be used to continue configuring the emailing infrastructure
(for example, provider registration in other packages).

### Email providers

`IEmailProvider` is responsible for sending the final `EmailMessage`.
This package only defines the abstraction. A typical provider implementation is located in another package, such as:

- [PosInformatique.Foundations.Emailing.Azure](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing.Azure/).
- [PosInformatique.Foundations.Emailing.Graph](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing.Graph/).

See the [PosInformatique.Foundations.Emailing.Azure](../Emailing.Azure/README.md)
or [PosInformatique.Foundations.Emailing.Graph](../Emailing.Graph/README.md)
documentation for an example of provider registration.

## Usage

### 1. Create an email from a template

To send an email, you first ask `IEmailManager` to create an `Email<TModel>` from a previously registered template identifier:

```csharp
using PosInformatique.Foundations.Emailing;

var emailManager = serviceProvider.GetRequiredService<IEmailManager>();

// Create an email based on the "Invitation" template
var invitationEmail = emailManager.Create(EmailTemplateIdentifiers.Invitation);

invitationEmail.Importance = EmailImportance.High;
```

At this stage:

- The `Email<TModel>` is linked to the `EmailTemplate<TModel>` previously registered in `EmailingOptions`.
- No recipient has been added yet.
- The importance of the e-mail is defined to `EmailImportance.High`.

### 2. Add recipients and models

You then populate the recipients collection with `EmailRecipient<TModel>`. Each recipient has:

- An `EmailAddress`.
- A display name.
- A data model instance (`TModel`) that will be injected into the template for that specific recipient.

```csharp
using PosInformatique.Foundations.EmailAddresses;

invitationEmail.Recipients.Add(
    EmailAddress.Parse("alice@example.com"),
    "Alice",
    new InvitationEmailTemplateModel
    {
        FirstName = "Alice",
        InvitationLink = "https://myapp.com/invite?code=ABC123"
    });

invitationEmail.Recipients.Add(
    EmailAddress.Parse("bob@example.com"),
    "Bob",
    new InvitationEmailTemplateModel
    {
        FirstName = "Bob",
        InvitationLink = "https://myapp.com/invite?code=XYZ789"
    });
```

### 3. Send the email

Once the email and its recipients are configured, you ask the `IEmailManager` to send it:

```csharp
var cancellationToken = CancellationToken.None;

await emailManager.SendAsync(invitationEmail, cancellationToken);
```

Under the hood:

1. `IEmailManager` iterates over all recipients.
2. For each recipient, it applies the associated model to the template’s subject and HTML body.
3. It builds an `EmailMessage` with:
   - The configured sender (`EmailingOptions.SenderEmailAddress`).
   - The recipient address and display name.
   - The generated subject and HTML content.
4. It calls `IEmailProvider.SendAsync(...)` to actually send the message.

The provider implementation is responsible for the technical details (SMTP, Azure Communication Service, etc.).

## Summary

The typical flow is:

1. Configure emailing:
   - Register templates and sender via `AddEmailing(...)`.
   - Register an `IEmailProvider` implementation.
2. Define and centralize template identifiers using `EmailTemplateIdentifier<TModel>`.
3. Define data models by creating custom data class.
4. At runtime, use `IEmailManager.Create(...)` to instantiate a strongly-typed email.
5. Add recipients and models through `EmailRecipientCollection<TModel>`.
6. Call `IEmailManager.SendAsync()` to generate and send emails through the `IEmailProvider`.

## Links

- [NuGet package: Emailing (core library)](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing/)
- [NuGet package: Emailing.Azure](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing.Azure/)
- [NuGet package: Emailing.Graph](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing.Graph/)
- [NuGet package: Text.Templating.Razor](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating.Razor/)
- [NuGet package: Text.Templating.Scriban](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating.Scriban/)
- [Source code](https://github.com/PosInformatique/PosInformatique.Foundations)