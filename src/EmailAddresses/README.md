# PosInformatique.Foundations.EmailAddresses

<img src="./Icon.png" alt="PosInformatique.Foundations.EmailAddresses icon" width="64" height="64" />

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.EmailAddresses.svg)](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.EmailAddresses.svg)](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses/)

## Introduction
This package provides a strongly-typed **EmailAddress** value object that ensures only valid email addresses (RFC 5322 compliant) can be instantiated.

It simplifies validation, parsing, comparison, and string formatting of email addresses.

## Install
You can install the package from [NuGet](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses/):

```powershell
dotnet add package PosInformatique.Foundations.EmailAddresses
```

## Features
- Strongly-typed email address validation and parsing
- Ensures email addresses are formatted according to **RFC 5322**
- Email values are always stored and compared in **lowercase** to avoid case-sensitive inconsistencies
- Implements `IEquatable<T>`, `IComparable<T>`, and operator overloads for equality and ordering
- Provides `IFormattable` and `IParsable<T>` for seamless integration with .NET APIs
- Implicit conversion between `string` and `EmailAddress`

## Use cases
- **Validation**: prevent invalid emails from being stored in your domain entities.
- **Type safety**: avoid dealing with raw strings when working with email addresses.
- **Conversion**: use implicit/explicit parsing and formatting seamlessly when working with APIs.
- **Consistency**: ensures a single, robust email parsing logic across all projects.

## Examples

### Create and validate email addresses
```csharp
// Implicit conversion from string
EmailAddress email = "john.doe@example.com";

// Access parts
Console.WriteLine(email.UserName); // "john.doe"
Console.WriteLine(email.Domain);   // "example.com"

// Validation
bool valid = EmailAddress.IsValid("test@domain.com"); // true
bool invalid = EmailAddress.IsValid("not-an-email");  // false
```

### Parsing
```csharp
var email = EmailAddress.Parse("alice@company.org");
Console.WriteLine(email); // "alice@company.org"

if (EmailAddress.TryParse("bob@company.org", out var parsed))
{
    Console.WriteLine(parsed.Domain); // "company.org"
}
```

### Comparisons
```csharp
var a = (EmailAddress)"alice@company.com";
var b = (EmailAddress)"bob@company.com";

Console.WriteLine(a == b);  // false
Console.WriteLine(a != b);  // true
Console.WriteLine(a < b);   // true, alphabetic order

var list = new List<EmailAddress> { b, a };
list.Sort(); // Sorted alphabetically
```

## Links
- [NuGet package](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses/)
- [Source code](https://github.com/PosInformatique/PosInformatique.Foundations)
