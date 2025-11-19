# PosInformatique.Foundations.PhoneNumbers.Json

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.PhoneNumbers.Json)](https://www.nuget.org/packages/PosInformatique.Foundations.PhoneNumbers.Json/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.PhoneNumbers.Json)](https://www.nuget.org/packages/PosInformatique.Foundations.PhoneNumbers.Json/)

## Introduction

This package provides `System.Text.Json` integration for the `PhoneNumber` value object
from [PosInformatique.Foundations.PhoneNumbers](../PhoneNumbers/README.md).

It adds a `JsonConverter<PhoneNumber>` that serializes and deserializes phone numbers as JSON strings in **E.164** format,
and an extension method to easily register the converter on your `JsonSerializerOptions`.

## Install

You can install the package from [NuGet](https://www.nuget.org/packages/PosInformatique.Foundations.PhoneNumbers.Json/):

```powershell
dotnet add package PosInformatique.Foundations.PhoneNumbers.Json
```

## Features

- `System.Text.Json` converter for the `PhoneNumber` value object
- Serializes `PhoneNumber` as a JSON string in **E.164** format
- Deserializes from a JSON string to a `PhoneNumber` instance
- Gracefully handles `null` and empty/whitespace JSON string values as `null`
- Convenient extension method `AddPhoneNumbersConverters` on `JsonSerializerOptions`

## Use cases

- Persist and exchange `PhoneNumber` values as JSON with APIs
- Use `PhoneNumber` in your DTOs without custom mapping code
- Share a consistent JSON representation (E.164) across microservices and clients

## Examples

### Register the converter globally

```csharp
using System.Text.Json;
using PosInformatique.Foundations.PhoneNumbers;
using PosInformatique.Foundations.PhoneNumbers.Json;

// Configure JsonSerializerOptions
var options = new JsonSerializerOptions()
    .AddPhoneNumbersConverters();
```

### Serialize a PhoneNumber

```csharp
using System.Text.Json;
using PosInformatique.Foundations.PhoneNumbers;

var options = new JsonSerializerOptions().AddPhoneNumbersConverters();

PhoneNumber phone = "+33123456789";

var json = JsonSerializer.Serialize(phone, options);
// json == "\"+33123456789\""
```

### Deserialize a PhoneNumber

```csharp
using System.Text.Json;
using PosInformatique.Foundations.PhoneNumbers;

var options = new JsonSerializerOptions().AddPhoneNumbersConverters();

var json = "\"+33123456789\"";

var phone = JsonSerializer.Deserialize<PhoneNumber>(json, options);
// phone represents "+33123456789"
```

### Use PhoneNumber in DTOs

```csharp
using System.Text.Json;
using PosInformatique.Foundations.PhoneNumbers;

public sealed class ContactDto
{
    public string Name { get; set; } = default!;
    public PhoneNumber? Mobile { get; set; }
}

var options = new JsonSerializerOptions().AddPhoneNumbersConverters();

var contact = new ContactDto
{
    Name = "John Doe",
    Mobile = PhoneNumber.Parse("+33123456789")
};

var json = JsonSerializer.Serialize(contact, options);
// {
//   "Name": "John Doe",
//   "Mobile": "+33123456789"
// }

var deserialized = JsonSerializer.Deserialize<ContactDto>(json, options);
```

### Handling null and empty values

- JSON `null` is deserialized as `null` `PhoneNumber`.
- Empty or whitespace JSON strings are also deserialized as `null`.

```csharp
var options = new JsonSerializerOptions().AddPhoneNumbersConverters();

var jsonNull = "null";
var phoneNull = JsonSerializer.Deserialize<PhoneNumber?>(jsonNull, options); // null

var jsonEmpty = "\"\"";
var phoneEmpty = JsonSerializer.Deserialize<PhoneNumber?>(jsonEmpty, options); // null
```

## Links

- [NuGet package: PhoneNumbers (core library)](https://www.nuget.org/packages/PosInformatique.Foundations.PhoneNumbers/)
- [NuGet package: PhoneNumbers.Json](https://www.nuget.org/packages/PosInformatique.Foundations.PhoneNumbers.Json/)
- [Source code](https://github.com/PosInformatique/PosInformatique.Foundations)