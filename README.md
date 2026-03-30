# PosInformatique.Foundations

<img src="Icon.png" alt="PosInformatique.Foundations icon" width="64" height="64" />

[PosInformatique.Foundations](https://github.com/PosInformatique/PosInformatique.Foundations) is a collection
of small, focused .NET libraries that provide **simple, reusable building blocks** for your applications.  

The goal is to avoid shipping a monolithic framework by creating **modular NuGet packages**,
each addressing a single responsibility.

## ✨ Philosophy

- **Granular**: each library is independent, lightweight, and minimal.  
- **Composable**: you bring exactly the pieces you need, nothing more.  
- **Practical**: packages can be foundational (value objects, abstractions, contracts) or technical utilities (helpers, validation rules, extensions).  
- **Consistent**: all packages follow the same naming convention and version alignment.  
- **Standards-based**: whenever possible, implementations follow well-known standards (e.g. RFC 5322 for email addresses, E.164 for phone numbers,...).  

➡️ Each package has **no strong dependency** on another. You are free to pick only what you need.  
➡️ These libraries are **not structuring frameworks**; they are small utilities meant to fill missing gaps in your applications.  

## 📦 Packages Overview

You can install any package using the .NET CLI or NuGet Package Manager.

| |Package (prefixed by PosInformatique.Foundations) | Description | NuGet |
|--|---------|-------------|-------|
|<img src="./src/EmailAddresses/Icon.png" alt="PosInformatique.Foundations.EmailAddresses icon" width="48" height="48" />|[**EmailAddresses**](./src/EmailAddresses/README.md) | Strongly-typed value object representing an email address with validation and normalization as RFC 5322 compliant. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.EmailAddresses)](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses) |
|<img src="./src/EmailAddresses/Icon.png" alt="PosInformatique.Foundations.EmailAddresses.EntityFramework icon" width="48" height="48" />|[**EmailAddresses.EntityFramework**](./src/EmailAddresses.EntityFramework/README.md) | Entity Framework Core integration for the `EmailAddress` value object, including property configuration and value converter for seamless database persistence. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.EmailAddresses.EntityFramework)](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses.EntityFramework) |
|<img src="./src/EmailAddresses/Icon.png" alt="PosInformatique.Foundations.EmailAddresses.FluentValidation icon" width="48" height="48" />|[**EmailAddresses.FluentValidation**](./src/EmailAddresses.FluentValidation/README.md) | FluentValidation integration for the `EmailAddress` value object, providing dedicated validators and rules to ensure RFC 5322 compliant email addresses. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.EmailAddresses.FluentValidation)](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses.FluentValidation) |
|<img src="./src/EmailAddresses/Icon.png" alt="PosInformatique.Foundations.EmailAddresses.Json icon" width="48" height="48" />|[**EmailAddresses.Json**](./src/EmailAddresses.Json/README.md) | `System.Text.Json` converter for the `EmailAddress` value object, enabling seamless serialization and deserialization of RFC 5322 compliant email addresses. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.EmailAddresses.Json)](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses.Json) |
|<img src="./src/Emailing/Icon.png" alt="PosInformatique.Foundations.Emailing icon" width="48" height="48" />|[**Emailing**](./src/Emailing/README.md) | Template-based emailing infrastructure for .NET that lets you register strongly-typed email templates, create emails from models, and send them through pluggable providers. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.Emailing)](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing) |
|<img src="./src/Emailing/Icon.png" alt="PosInformatique.Foundations.Emailing.Azure icon" width="48" height="48" />|[**Emailing.Azure**](./src/Emailing.Azure/README.md) | `IEmailProvider` implementation for [PosInformatique.Foundations.Emailing](./src/Emailing/README.md) using **Azure Communication Service**. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.Emailing.Azure)](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing.Azure) |
|<img src="./src/Emailing/Icon.png" alt="PosInformatique.Foundations.Emailing.Graph icon" width="48" height="48" />|[**Emailing.Graph**](./src/Emailing.Graph/README.md) | `IEmailProvider` implementation for [PosInformatique.Foundations.Emailing](./src/Emailing/README.md) using **Microsoft Graph API**. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.Emailing.Graph)](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing.Graph) |
|<img src="./src/Emailing/Icon.png" alt="PosInformatique.Foundations.Emailing.Templates.Razor icon" width="48" height="48" />|[**Emailing.Templates.Razor**](./src/Emailing.Templates.Razor/README.md) | Helpers to build EmailTemplate instances from Razor components for subject and HTML body, supporting strongly-typed models and reusable layouts. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.Emailing.Templates.Razor)](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing.Templates.Razor) |
|<img src="./src/MediaTypes/Icon.png" alt="PosInformatique.Foundations.MediaTypes icon" width="48" height="48" />|[**MediaTypes**](./src/MediaTypes/README.md) | Immutable `MimeType` value object with well-known media types and helpers to map between media types and file extensions. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.MediaTypes)](https://www.nuget.org/packages/PosInformatique.Foundations.MediaTypes) |
|<img src="./src/MediaTypes/Icon.png" alt="PosInformatique.Foundations.MediaTypes.EntityFramework icon" width="48" height="48" />|[**MediaTypes.EntityFramework**](./src/MediaTypes.EntityFramework/README.md) | Entity Framework Core integration for the `MimeType` value object, including property configuration and value converter for seamless database persistence. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.MediaTypes.EntityFramework)](https://www.nuget.org/packages/PosInformatique.Foundations.MediaTypes.EntityFramework) |
|<img src="./src/MediaTypes/Icon.png" alt="PosInformatique.Foundations.MediaTypes.Json icon" width="48" height="48" />|[**MediaTypes.Json**](./src/MediaTypes.Json/README.md) | `System.Text.Json` converter for the `MimeType` value object, enabling seamless serialization and deserialization of MIME types within JSON documents. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.MediaTypes.Json)](https://www.nuget.org/packages/PosInformatique.Foundations.MediaTypes.Json) |
|<img src="./src/People/Icon.png" alt="PosInformatique.Foundations.People icon" width="48" height="48" />|[**People**](./src/People/README.md) | Strongly-typed value objects for first and last names with validation and normalization. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.People)](https://www.nuget.org/packages/PosInformatique.Foundations.People) |
|<img src="./src/People/Icon.png" alt="PosInformatique.Foundations.People.DataAnnotations icon" width="48" height="48" />|[**People.DataAnnotations**](./src/People.DataAnnotations/README.md) | DataAnnotations attributes for `FirstName` and `LastName` value objects. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.People.DataAnnotations)](https://www.nuget.org/packages/PosInformatique.Foundations.People.DataAnnotations) |
|<img src="./src/People/Icon.png" alt="PosInformatique.Foundations.People.EntityFramework icon" width="48" height="48" />|[**People.EntityFramework**](./src/People.EntityFramework/README.md) | Entity Framework Core integration for `FirstName` and `LastName` value objects, providing fluent property configuration and value converters. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.People.EntityFramework)](https://www.nuget.org/packages/PosInformatique.Foundations.People.EntityFramework) |
|<img src="./src/People/Icon.png" alt="PosInformatique.Foundations.People.FluentAssertions icon" width="48" height="48" />|[**People.FluentAssertions**](./src/People.FluentAssertions/README.md) | [FluentAssertions](https://fluentassertions.com/) extensions for `FirstName` and `LastName` to avoid ambiguity and provide `Should().Be(string)` assertions (case-sensitive on normalized values). | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.People.FluentAssertions)](https://www.nuget.org/packages/PosInformatique.Foundations.People.FluentAssertions) |
|<img src="./src/People/Icon.png" alt="PosInformatique.Foundations.People.FluentValidation icon" width="48" height="48" />|[**People.FluentValidation**](./src/People.FluentValidation/README.md) | [FluentValidation](https://fluentvalidation.net/) extensions for `FirstName` and `LastName` value objects. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.People.FluentValidation)](https://www.nuget.org/packages/PosInformatique.Foundations.People.FluentValidation) |
|<img src="./src/People/Icon.png" alt="PosInformatique.Foundations.People.Json icon" width="48" height="48" />|[**People.Json**](./src/People.Json/README.md) | `System.Text.Json` converters for `FirstName` and `LastName`, with validation and easy registration via `AddPeopleConverters()`. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.People.Json)](https://www.nuget.org/packages/PosInformatique.Foundations.People.Json) |
|<img src="./src/PhoneNumbers/Icon.png" alt="PosInformatique.Foundations.PhoneNumbers icon" width="48" height="48" />|[**PhoneNumbers**](./src/PhoneNumbers/README.md) | Strongly-typed value object representing a phone number in E.164 format, with parsing (including region-aware local numbers), validation, comparison, and formatting helpers. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.PhoneNumbers)](https://www.nuget.org/packages/PosInformatique.Foundations.PhoneNumbers) |
|<img src="./src/PhoneNumbers/Icon.png" alt="PosInformatique.Foundations.PhoneNumbers.EntityFramework icon" width="48" height="48" />|[**PhoneNumbers.EntityFramework**](./src/PhoneNumbers.EntityFramework/README.md) | Entity Framework Core integration for the `PhoneNumber` value object, mapping it to a SQL `PhoneNumber` column type backed by `VARCHAR(16)` using a dedicated value converter. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.PhoneNumbers.EntityFramework)](https://www.nuget.org/packages/PosInformatique.Foundations.PhoneNumbers.EntityFramework) |
|<img src="./src/PhoneNumbers/Icon.png" alt="PosInformatique.Foundations.PhoneNumbers.FluentValidation icon" width="48" height="48" />|[**PhoneNumbers.FluentValidation**](./src/PhoneNumbers.FluentValidation/README.md) | FluentValidation integration for the `PhoneNumber` value object, providing dedicated validators and rules to ensure E.164 compliant phone numbers. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.PhoneNumbers.FluentValidation)](https://www.nuget.org/packages/PosInformatique.Foundations.PhoneNumbers.FluentValidation) |
|<img src="./src/PhoneNumbers/Icon.png" alt="PosInformatique.Foundations.PhoneNumbers.Json icon" width="48" height="48" />|[**PhoneNumbers.Json**](./src/PhoneNumbers.Json/README.md) | `System.Text.Json` converter for the `PhoneNumber` value object, enabling seamless serialization and deserialization of E.164 compliant phone numbers. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.PhoneNumbers.Json)](https://www.nuget.org/packages/PosInformatique.Foundations.PhoneNumbers.Json) |
|<img src="./src/Text.Templating/Icon.png" alt="PosInformatique.Foundations.Text.Templating icon" width="48" height="48" />|[**Text.Templating**](./src/Text.Templating/README.md) | Abstractions for text templating, including the `TextTemplate<TModel>` base class and `ITextTemplateRenderContext` interface, to be used by concrete templating engine implementations such as Razor-based text templates. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.Text.Templating)](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating) |
|<img src="./src/Text.Templating/Icon.png" alt="PosInformatique.Foundations.Text.Templating.Razor icon" width="48" height="48" />|[**Text.Templating.Razor**](./src/Text.Templating.Razor/README.md) | Razor-based text templating using Blazor components, allowing generation of text from Razor views with a strongly-typed Model parameter and full dependency injection integration. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.Text.Templating.Razor)](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating.Razor) |
|<img src="./src/Text.Templating/Icon.png" alt="PosInformatique.Foundations.Text.Templating.Scriban icon" width="48" height="48" />|[**Text.Templating.Scriban**](./src/Text.Templating.Scriban/README.md) | Scriban-based text templating with mustache-style syntax, allowing generation of text from templates using a strongly-typed model and automatic property exposure. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.Text.Templating.Scriban)](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating.Scriban) |

> Note: Most of the packages are completely independent. You install only what you need.

## 🚀 Why use PosInformatique.Foundations?

- Avoid reinventing common value objects and utilities.  
- Apply standards-based implementations (RFC, E.164, ...).  
- Improve consistency across your projects.  
- Get lightweight, modular libraries tailored to single responsibilities.  
- Add missing building blocks to your projects without introducing a heavyweight framework.

## 📜 Changelog
Each package maintains its own changelog in its respective `CHANGELOG.md` file located in the package's source directory, but
for your convenience, we also provide a consolidated [changelog here](./CHANGELOG.md).

## 📌 .NET and dependency compatibility

All [PosInformatique.Foundations](https://github.com/PosInformatique/PosInformatique.Foundations) packages are designed to be compatible with **.NET 8.0**, **.NET 9.0** and **.NET 10.0**.

To maximize backward compatibility with existing projects, dependencies on external libraries (such as `Microsoft.Graph`, etc.)
intentionally target **relatively old versions**. This avoids forcing you to update your entire solution to the
latest versions used internally by [PosInformatique.Foundations](https://github.com/PosInformatique/PosInformatique.Foundations).

> Important: It is the responsibility of the application developer to explicitly reference and update
any **transitive dependencies** in their own project if they want to use newer versions.
> See [NuGet dependency resolution](https://learn.microsoft.com/en-us/nuget/concepts/dependency-resolution)
and [transitive dependencies in Visual Studio](https://devblogs.microsoft.com/dotnet/introducing-transitive-dependencies-in-visual-studio/)
for more details.

### Example with Microsoft.Graph

The [PosInformatique.Foundations.Emailing.Graph](https://www.nuget.org/packages/[PosInformatique.Foundations.Emailing.Graph/)
package depends on [Microsoft.Graph](https://www.nuget.org/packages/Microsoft.Graph/) **5.89.0**
for backward compatibility with a wide range of existing projects.

If your application requires a newer version, you can simply add an explicit reference in your project, for example:

```xml
<ItemGroup>
  <PackageReference Include="PosInformatique.Foundations.Emailing.Graph" Version="x.y.z" />
  <PackageReference Include="Microsoft.Graph" Version="5.96.0" />
</ItemGroup>
```

In this case, your project will use [Microsoft.Graph](https://www.nuget.org/packages/Microsoft.Graph/) **5.96.0**
while still consuming
[PosInformatique.Foundations.Emailing.Graph](https://www.nuget.org/packages/[PosInformatique.Foundations.Emailing.Graph/).
This is **recommended**, especially to benefit from the latest security updates and bug fixes of the underlying dependencies.

> The next versions of [PosInformatique.Foundations](https://github.com/PosInformatique/PosInformatique.Foundations) packages
> will updated their dependencies to newer versions, if there is security vulnerabilities reported by NuGet or GitHub advisories.
> This to force also developers to avoid using vulnerable versions of the dependencies when upgrading to the new versions of the packages.

## 📄 License

Licensed under the [MIT License](./LICENSE).
