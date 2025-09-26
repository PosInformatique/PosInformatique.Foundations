# PosInformatique.Foundations.EmailAddresses.Json

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.EmailAddresses.Json)](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses.Json/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.EmailAddresses.Json)](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses.Json/)

## Introduction
Provides a **System.Text.Json** converter for the `EmailAddress` value object from
[PosInformatique.Foundations.EmailAddresses](../EmailAddresses/README.md). Enables seamless serialization and deserialization of **RFC 5322 compliant** email addresses within JSON documents.

## Install
You can install the package from NuGet:

```powershell
dotnet add package PosInformatique.Foundations.EmailAddresses.Json
```

This package depends on the base package [PosInformatique.Foundations.EmailAddresses](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses/).

## Features
- Provides a `JsonConverter<EmailAddress>` for serialization and deserialization.
- Ensures validation of **RFC 5322 compliant** email addresses when deserializing.
- Can be used via attributes (`[JsonConverter]`) or through `JsonSerializerOptions` extension method.
- Ensures consistency with the base `EmailAddress` value object.

## Use cases
- **Serialization**: Convert value objects into JSON strings without losing semantics  
- **Validation**: Guarantee that only valid RFC 5322 email addresses are accepted in JSON payloads  
- **Integration**: Plug directly into `System.Text.Json` configuration  

## Examples

### Example 1: DTO with `[JsonConverter]` attribute
```csharp
using System.Text.Json;
using System.Text.Json.Serialization;
using PosInformatique.Foundations;

public class UserDto
{
    [JsonConverter(typeof(EmailAddressJsonConverter))]
    public EmailAddress Email { get; set; } = default!;
}

// Serialization
var dto = new UserDto { Email = "john.doe@example.com" };
var json = JsonSerializer.Serialize(dto);
// Result: {"Email":"john.doe@example.com"}

// Deserialization
var input = "{ "Email": "alice@company.org" }";
var deserialized = JsonSerializer.Deserialize<UserDto>(input);
Console.WriteLine(deserialized!.Email); // "alice@company.org"
```

### Example 2: Use extension method without attributes
```csharp
using System.Text.Json;
using PosInformatique.Foundations;

public class CustomerDto
{
    public EmailAddress Email { get; set; } = default!;
}

var options = new JsonSerializerOptions().AddEmailAddressesConverters();

// Serialization
var dto = new CustomerDto { Email = "bob@myapp.com" };
var json = JsonSerializer.Serialize(dto, options);
// Result: {"Email":"bob@myapp.com"}

// Deserialization
var input = "{ "Email": "carol@myapp.com" }";
var deserialized = JsonSerializer.Deserialize<CustomerDto>(input, options);
Console.WriteLine(deserialized!.Email); // "carol@myapp.com"
```

## Links
- [NuGet package: EmailAddresses.Json](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses.Json/)
- [NuGet package: EmailAddresses (core library)](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses/)
- [Source code](https://github.com/PosInformatique/PosInformatique.Foundations)
