# PosInformatique.Foundations.People.Json

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.People.Json)](https://www.nuget.org/packages/PosInformatique.Foundations.People.Json/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.People.Json)](https://www.nuget.org/packages/PosInformatique.Foundations.People.Json/)

## Introduction
Provides `System.Text.Json` converters for the `FirstName` and `LastName` value objects from
[PosInformatique.Foundations.People](../People/README.md). Enables seamless serialization and deserialization of validated names within JSON documents.

## Install
You can install the package from NuGet:

```powershell
dotnet add package PosInformatique.Foundations.People.Json
```

This package depends on the base package [PosInformatique.Foundations.People](https://www.nuget.org/packages/PosInformatique.Foundations.People/).

## Features
- `JsonConverter<FirstName>` and `JsonConverter<LastName>` for serialization and deserialization.
- Validation on deserialization using `FirstName.TryCreate` and `LastName.TryCreate` (throws `JsonException` on invalid input).
- Usable via `[JsonConverter]` attribute or via `JsonSerializerOptions` extension method `AddPeopleConverters()`.

## Use cases
- Serialization: Persist `FirstName` and `LastName` as JSON strings.
- Validation: Ensure only valid names are accepted in JSON payloads.
- Integration: Plug directly into `System.Text.Json` configuration.

## Examples

### Example 1: DTOs with [JsonConverter] attributes
```csharp
using System.Text.Json;
using System.Text.Json.Serialization;
using PosInformatique.Foundations;
using PosInformatique.Foundations.People.Json;

public sealed class PersonDto
{
    [JsonConverter(typeof(FirstNameJsonConverter))]
    public FirstName? FirstName { get; set; }

    [JsonConverter(typeof(LastNameJsonConverter))]
    public LastName? LastName { get; set; }
}

// Serialization
var dto = new PersonDto
{
    FirstName = FirstName.Create("John"),
    LastName = LastName.Create("Doe")
};

var json = JsonSerializer.Serialize(dto);
// Result: {"FirstName":"John","LastName":"Doe"}

// Deserialization
var input = "{ \"FirstName\": \"Alice\", \"LastName\": \"Smith\" }";
var deserialized = JsonSerializer.Deserialize<PersonDto>(input);
```

### Example 2: Register converters globally with options
```csharp
using System.Text.Json;
using PosInformatique.Foundations;
using PosInformatique.Foundations.People.Json;

public sealed class EmployeeDto
{
    public FirstName? FirstName { get; set; }
    public LastName? LastName { get; set; }
}

var options = new JsonSerializerOptions().AddPeopleConverters();

// Serialization
var employee = new EmployeeDto
{
    FirstName = FirstName.Create("Bob"),
    LastName = LastName.Create("Marley")
};

var json = JsonSerializer.Serialize(employee, options);
// Result: {"FirstName":"Bob","LastName":"Marley"}

// Deserialization
var input = "{ \"FirstName\": \"Carol\", \"LastName\": \"Johnson\" }";
var deserialized = JsonSerializer.Deserialize<EmployeeDto>(input, options);
```

### Example 3: Handling nulls and invalid values
```csharp
using System.Text.Json;
using PosInformatique.Foundations;
using PosInformatique.Foundations.People.Json;

var options = new JsonSerializerOptions().AddPeopleConverters();

// Null handling
var jsonWithNulls = "{ \"FirstName\": null, \"LastName\": \"Doe\" }";
var obj = JsonSerializer.Deserialize<EmployeeDto>(jsonWithNulls, options);
// obj.FirstName == null, obj.LastName == "Doe"

// Invalid value causes JsonException
try
{
    var invalid = "{ \"FirstName\": \"\", \"LastName\": \"Doe\" }";

    JsonSerializer.Deserialize<EmployeeDto>(invalid, options);
}
catch (JsonException ex)
{
    // "'': is not a valid first name." (message from converter)
}
```

## Links
- [NuGet package: People.Json](https://www.nuget.org/packages/PosInformatique.Foundations.People.Json/)
- [NuGet package: People (core library)](https://www.nuget.org/packages/PosInformatique.Foundations.People/)
- [Source code](https://github.com/PosInformatique/PosInformatique.Foundations)
