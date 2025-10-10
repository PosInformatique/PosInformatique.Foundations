# PosInformatique.Foundations

<img src="Icon.png" alt="PosInformatique.Foundations icon" width="64" height="64" />

PosInformatique.Foundations is a collection of small, focused .NET libraries that provide **simple, reusable building blocks** for your applications.  

The goal is to avoid shipping a monolithic framework by creating **modular NuGet packages**, each addressing a single responsibility.

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

| |Package | Description | NuGet |
|--|---------|-------------|-------|
|<img src="./src/EmailAddresses/Icon.png" alt="PosInformatique.Foundations.EmailAddresses icon" width="48" height="48" />|[**PosInformatique.Foundations.EmailAddresses**](./src/EmailAddresses/README.md) | Strongly-typed value object representing an email address with validation and normalization as RFC 5322 compliant. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.EmailAddresses)](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses) |
|<img src="./src/EmailAddresses/Icon.png" alt="PosInformatique.Foundations.EmailAddresses.EntityFramework icon" width="48" height="48" />|[**PosInformatique.Foundations.EmailAddresses.EntityFramework**](./src/EmailAddresses.EntityFramework/README.md) | Entity Framework Core integration for the EmailAddress value object, including property configuration and value converter for seamless database persistence. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.EmailAddresses.EntityFramework)](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses.EntityFramework) |
|<img src="./src/EmailAddresses/Icon.png" alt="PosInformatique.Foundations.EmailAddresses.FluentValidation icon" width="48" height="48" />|[**PosInformatique.Foundations.EmailAddresses.FluentValidation**](./src/EmailAddresses.FluentValidation/README.md) | FluentValidation integration for the EmailAddress value object, providing dedicated validators and rules to ensure RFC 5322 compliant email addresses. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.EmailAddresses.FluentValidation)](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses.FluentValidation) |
|<img src="./src/EmailAddresses/Icon.png" alt="PosInformatique.Foundations.EmailAddresses.Json icon" width="48" height="48" />|[**PosInformatique.Foundations.EmailAddresses.Json**](./src/EmailAddresses.Json/README.md) | `System.Text.Json` converter for the EmailAddress value object, enabling seamless serialization and deserialization of RFC 5322 compliant email addresses. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.EmailAddresses.Json)](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses.Json) |
|<img src="./src/People/Icon.png" alt="PosInformatique.Foundations.People icon" width="48" height="48" />|[**PosInformatique.Foundations.People**](./src/People/README.md) | Strongly-typed value objects for first and last names with validation and normalization. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.People)](https://www.nuget.org/packages/PosInformatique.Foundations.People) |
|<img src="./src/People/Icon.png" alt="PosInformatique.Foundations.People.DataAnnotations icon" width="48" height="48" />|[**PosInformatique.Foundations.People.DataAnnotations**](./src/People.DataAnnotations/README.md) | DataAnnotations attributes for `FirstName` and `LastName` value objects. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.People.DataAnnotations)](https://www.nuget.org/packages/PosInformatique.Foundations.People.DataAnnotations) |
|<img src="./src/People/Icon.png" alt="PosInformatique.Foundations.People.EntityFramework icon" width="48" height="48" />|[**PosInformatique.Foundations.People.EntityFramework**](./src/People.EntityFramework/README.md) | Entity Framework Core integration for `FirstName` and `LastName` value objects, providing fluent property configuration and value converters. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.People.EntityFramework)](https://www.nuget.org/packages/PosInformatique.Foundations.People.EntityFramework) |
|<img src="./src/People/Icon.png" alt="PosInformatique.Foundations.People.FluentAssertions icon" width="48" height="48" />|[**PosInformatique.Foundations.People.FluentAssertions**](./src/People.FluentAssertions/README.md) | [FluentAssertions](https://fluentassertions.com/) extensions for `FirstName` and `LastName` to avoid ambiguity and provide `Should().Be(string)` assertions (case-sensitive on normalized values). | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.People.FluentAssertions)](https://www.nuget.org/packages/PosInformatique.Foundations.People.FluentAssertions) |
|<img src="./src/People/Icon.png" alt="PosInformatique.Foundations.People.FluentValidation icon" width="48" height="48" />|[**PosInformatique.Foundations.People.FluentValidation**](./src/People.FluentValidation/README.md) | [FluentValidation](https://fluentvalidation.net/) extensions for `FirstName` and `LastName` value objects. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.People.FluentValidation)](https://www.nuget.org/packages/PosInformatique.Foundations.People.FluentValidation) |
|<img src="./src/People/Icon.png" alt="PosInformatique.Foundations.People.Json icon" width="48" height="48" />|[**PosInformatique.Foundations.People.Json**](./src/People.Json/README.md) | `System.Text.Json` converters for `FirstName` and `LastName`, with validation and easy registration via `AddPeopleConverters()`. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.People.Json)](https://www.nuget.org/packages/PosInformatique.Foundations.People.Json) |

> Note: Each package is completely independent. You install only what you need.

## 🚀 Why use PosInformatique.Foundations?

- Avoid reinventing common value objects and utilities.  
- Apply standards-based implementations (RFC, E.164, ...).  
- Improve consistency across your projects.  
- Get lightweight, modular libraries tailored to single responsibilities.  
- Add missing building blocks to your projects without introducing a heavyweight framework.  

## 📄 License

Licensed under the [MIT License](./LICENSE).
