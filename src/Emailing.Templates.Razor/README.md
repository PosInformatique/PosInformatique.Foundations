# PosInformatique.Foundations.Emailing.Templates.Razor

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.Emailing.Templates.Razor)](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing.Templates.Razor/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.Emailing.Templates.Razor)](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing.Templates.Razor/)

## Introduction

[PosInformatique.Foundations.Emailing.Templates.Razor](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing.Templates.Razor/)
provides helpers to create `EmailTemplate<TModel>` instances using Razor components as text templates for the subject and HTML body.

It is built on top of:

- [PosInformatique.Foundations.Emailing](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing.Templates.Razor/)
- [PosInformatique.Foundations.Text.Templates.Razor](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing.Templates.Razor/)

This allows you to design your email content with Blazor-style Razor components, benefiting from layout reuse, strongly-typed models, and familiar Razor syntax.

## Install

You can install the package from [NuGet](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing.Templates.Razor/):

```powershell
dotnet add package PosInformatique.Foundations.Emailing.Templates.Razor
```

You also need a Blazor-compatible environment for compiling/executing Razor components.

## Features

- `RazorEmailTemplate<TModel>.Create<TSubjectComponent, TBodyComponent>()` to build `EmailTemplate<TModel>` from Razor components.
- Base classes for Razor components:
  - `RazorEmailTemplateSubject<TModel>` for email subject.
  - `RazorEmailTemplateBody<TModel>` for email HTML body.
- Strongly-typed model support via the `Model` parameter.
- Supports Razor layout features for the email body (reuse consistent layout across multiple templates).

## Creating Razor components for subject and body

### 1. Define the email model

Example model used by the templates:

```csharp
using PosInformatique.Foundations.Emailing;

public sealed class InvitationEmailTemplateModel : EmailModel
{
    public string FirstName { get; set; } = string.Empty;
    public string InvitationLink { get; set; } = string.Empty;
}
```

### 2. Subject component

Create a Razor component for the subject that inherits from `RazorEmailTemplateSubject<TModel>` and uses the `Model` parameter.

`InvitationEmailSubject.razor`:

```razor
@using PosInformatique.Foundations.Emailing
@using PosInformatique.Foundations.Emailing.Templates.Razor
@inherits RazorEmailTemplateSubject<InvitationEmailTemplateModel>

Invitation for @Model.FirstName
```

This component:

- Renders a single line of text.
- Uses the strongly-typed `Model` to build the subject.

### 3. Body component with layout

You can define a layout component that centralizes common HTML structure (header, footer, styles, etc.),
then reuse it across different email bodies.

`EmailLayout.razor` (layout component):

```razor
@inherits LayoutComponentBase

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>@Title</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            font-size: 14px;
        }

        .email-container {
            max-width: 600px;
            margin: 0 auto;
        }

        .email-header {
            background-color: #1f2937;
            color: #ffffff;
            padding: 16px;
            font-size: 18px;
            font-weight: bold;
        }

        .email-content {
            padding: 16px;
        }

        .email-footer {
            padding: 16px;
            font-size: 12px;
            color: #6b7280;
        }
    </style>
</head>
<body>
    <div class="email-container">
        <div class="email-header">
            @Title
        </div>

        <div class="email-content">
            @Body
        </div>

        <div class="email-footer">
            This email was sent by MyApp.
        </div>
    </div>
</body>
</html>
```

Now create the specific body component for your invitation email.

`InvitationEmailBody.razor`:

```razor
@using PosInformatique.Foundations.Emailing
@using PosInformatique.Foundations.Emailing.Templates.Razor
@inherits RazorEmailTemplateBody<InvitationEmailTemplateModel>
@layout EmailLayout

@{
    Title = $"Invitation for {Model.FirstName}";
}

<p>Hello @Model.FirstName,</p>

<p>
    You have been invited to join our platform.
    Please click the link below to accept your invitation:
</p>

<p>
    <a href="@Model.InvitationLink">@Model.InvitationLink</a>
</p>

<p>
    If you did not expect this email, you can safely ignore it.
</p>
```

Key points:

- The body component inherits from `RazorEmailTemplateBody<InvitationEmailTemplateModel>`.
- It uses `@layout EmailLayout` to reuse the common HTML structure.
- It uses the `Model` property to inject data into the HTML.

## Creating an EmailTemplate from Razor components

Use the static helper `RazorEmailTemplate<TModel>.Create<TSubjectComponent, TBodyComponent>()` to build an `EmailTemplate<TModel>` instance.

```csharp
using PosInformatique.Foundations.Emailing;
using PosInformatique.Foundations.Emailing.Templates.Razor;

var invitationTemplate = RazorEmailTemplate<InvitationEmailTemplateModel>.Create<
    InvitationEmailSubject,
    InvitationEmailBody>();
```

You can then register this template in `EmailingOptions`:

```csharp
options.RegisterTemplate(EmailTemplateIdentifiers.Invitation, invitationTemplate);
```

After that, the rest of the flow is the same as with any other `EmailTemplate<TModel>`:

- Use `IEmailManager.Create(EmailTemplateIdentifiers.Invitation)` to create the email.
- Add recipients and models.
- Call `SendAsync(...)` to send.

## Links

- [NuGet package: Emailing (core library)](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing/)
- [NuGet package: Emailing.Templates.Razor](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing.Templates.Razor/)
- [NuGet package: Text.Templating.Razor](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating.Razor/)
- [Source code](https://github.com/PosInformatique/PosInformatique.Foundations)