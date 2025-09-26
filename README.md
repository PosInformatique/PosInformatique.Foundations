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
|<img src="./src/EmailAddresses/Icon.png" alt="PosInformatique.Foundations.EmailAddresses.Json icon" width="48" height="48" />|[**PosInformatique.Foundations.EmailAddresses.Json**](./src/EmailAddresses.Json/README.md) | System.Text.Json converter for the EmailAddress value object, enabling seamless serialization and deserialization of RFC 5322 compliant email addresses. | [![NuGet](https://img.shields.io/nuget/v/PosInformatique.Foundations.EmailAddresses.Json)](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses.Json) |

> Note: Each package is completely independent. You install only what you need.

## 🚀 Why use PosInformatique.Foundations?

- Avoid reinventing common value objects and utilities.  
- Apply standards-based implementations (RFC, E.164, ...).  
- Improve consistency across your projects.  
- Get lightweight, modular libraries tailored to single responsibilities.  
- Add missing building blocks to your projects without introducing a heavyweight framework.  

## 📄 License

Licensed under the [MIT License](./LICENSE).
