# PosInformatique.Foundations.PhoneNumbers

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.PhoneNumbers)](https://www.nuget.org/packages/PosInformatique.Foundations.PhoneNumbers/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.PhoneNumbers)](https://www.nuget.org/packages/PosInformatique.Foundations.PhoneNumbers/)

## Introduction

This package provides a strongly-typed `PhoneNumber` value object that represents a phone number in **E.164** format.

It centralizes validation, parsing, comparison and formatting logic for phone numbers, and ensures that only valid international phone numbers can be instantiated.

It is recommended to use E.164 format everywhere in your code. When parsing a **local** phone number (not already in E.164), you must explicitly specify the **region** to avoid ambiguity.

This library uses the [libphonenumber-csharp](https://www.nuget.org/packages/libphonenumber-csharp) library under the hood for parsing and formatting.

## Install

You can install the package from [NuGet](https://www.nuget.org/packages/PosInformatique.Foundations.PhoneNumbers/):

```powershell
dotnet add package PosInformatique.Foundations.PhoneNumbers
```

## Features

- Strongly-typed phone number value object based on **E.164** format
- Validation and parsing of international and local phone numbers
- Always stored and returned in **E.164** format by default
- Implements `IEquatable<PhoneNumber>` for value-based equality
- Implements `IFormattable` and `IParsable<PhoneNumber>` for easy integration with .NET APIs (parsing/formatting)
- Implicit conversion between `string` and `PhoneNumber`
- Helpers to format in **international** and **national** formats

## Use cases

- **Validation**: prevent invalid phone numbers from being stored in domain entities.
- **Type safety**: avoid handling raw strings for phone numbers across the application.
- **Standardization**: use E.164 format as the single source of truth everywhere.
- **Integration**: use `IParsable` / `IFormattable` in generic components (bindings, configuration, serialization, etc.).

## Examples

### Create and validate phone numbers

```csharp
using PosInformatique.Foundations.PhoneNumbers;

// Implicit conversion from string (expects a valid E.164 number)
PhoneNumber phone = "+33123456789";

// To string (E.164 by default)
Console.WriteLine(phone);          // "+33123456789"

// Validation
var valid = PhoneNumber.IsValid("+33123456789"); // true
var invalid = PhoneNumber.IsValid("1234");       // false
```

### Parsing E.164 phone numbers

```csharp
var phone = PhoneNumber.Parse("+14155552671");
Console.WriteLine(phone); // "+14155552671"
```

### Parsing local numbers with an explicit region

When you parse a local phone number (not starting with "+"), you must specify the region (ISO 3166-1 alpha-2, e.g. "FR", "US", ...).

```csharp
// Local French number, region must be specified
var frenchPhone = PhoneNumber.Parse("01 23 45 67 89", defaultRegion: "FR");
Console.WriteLine(frenchPhone); // E.164: "+33123456789"

// TryParse with region
if (PhoneNumber.TryParse("06 12 34 56 78", out var mobile, defaultRegion: "FR"))
{
    Console.WriteLine(mobile); // "+33612345678"
}
```

It is recommended to always work with E.164 numbers in your code (storage, comparison, APIs),
and only handle local formats at the boundaries (UI, input parsing) by specifying the region explicitly.

### Formatting: E.164, international and national

```csharp
var phone = PhoneNumber.Parse("+14155552671");

// Default ToString() = E.164
Console.WriteLine(phone.ToString());              // "+14155552671"

// International format
Console.WriteLine(phone.ToInternationalString()); // "+1 415-555-2671" (example output)

// National format
Console.WriteLine(phone.ToNationalString());      // "(415) 555-2671" (example output)
```

### Using implicit conversions

```csharp
// string -> PhoneNumber (implicit)
PhoneNumber phone = "+447911123456";

// PhoneNumber -> string (implicit, E.164)
string phoneString = phone;

Console.WriteLine(phoneString); // "+447911123456"
```

### Using `IParsable<PhoneNumber>` generically

Because `PhoneNumber` implements `IParsable<PhoneNumber>`, it can be used in generic parsing scenarios.

```csharp
// Generic parsing using IParsable<T>
static T ParseValue<T>(string value)
    where T : IParsable<T>
{
    var result = T.Parse(value, provider: null);
    return result;
}

var phone = ParseValue<PhoneNumber>("+33123456789");
Console.WriteLine(phone); // "+33123456789"
```

### Using `IFormattable` generically

`PhoneNumber` also implements `IFormattable`, so it can be formatted via APIs that rely on `IFormattable`.

```csharp
static string FormatValue(IFormattable value)
{
    // format and provider are ignored in this implementation,
    // but this allows generic handling of different value objects.
    return value.ToString(format: null, formatProvider: null);
}

var phone = PhoneNumber.Parse("+33123456789");
var formatted = FormatValue(phone);

Console.WriteLine(formatted); // "+33123456789"
```

## Recommendations

- Always store and exchange phone numbers in **E.164 format** (e.g. in your database, APIs, events).
- Only accept local phone numbers at the boundaries (UI, import, etc.), and **always** specify the `defaultRegion` when parsing:
  - `PhoneNumber.Parse(localNumber, defaultRegion: "FR")`
  - `PhoneNumber.TryParse(localNumber, out var number, defaultRegion: "FR")`
- Avoid keeping raw strings. Use the `PhoneNumber` value object everywhere to centralize validation and formatting.

## Links

- [NuGet package (core library)](https://www.nuget.org/packages/PosInformatique.Foundations.PhoneNumbers/)
- [NuGet package: PhoneNumbers.EntityFramework](https://www.nuget.org/packages/PosInformatique.Foundations.PhoneNumbers.EntityFramework/)
- [NuGet package: PhoneNumbers.Json](https://www.nuget.org/packages/PosInformatique.Foundations.PhoneNumbers.Json/)
- [Source code](https://github.com/PosInformatique/PosInformatique.Foundations)