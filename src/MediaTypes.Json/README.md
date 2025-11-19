# PosInformatique.Foundations.MediaTypes.Json

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.MediaTypes.Json)](https://www.nuget.org/packages/PosInformatique.Foundations.MediaTypes.Json/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.MediaTypes.Json)](https://www.nuget.org/packages/PosInformatique.Foundations.MediaTypes.Json/)

## Introduction
Provides a **System.Text.Json** converter for the `MimeType` value object from
[PosInformatique.Foundations.MediaTypes](../MediaTypes/README.md).
Enables seamless serialization and deserialization of MIME types (e.g. `application/json`, `image/png`) within JSON documents.

## Install
You can install the package from NuGet:

```powershell
dotnet add package PosInformatique.Foundations.MediaTypes.Json
```

This package depends on the base package [PosInformatique.Foundations.MediaTypes](https://www.nuget.org/packages/PosInformatique.Foundations.MediaTypes/).

## Features
- Provides a `JsonConverter<MimeType>` for serialization and deserialization.
- Validates MIME type strings when deserializing (throws `JsonException` on invalid value).
- Handles `null` values correctly when reading JSON.
- Can be used via attributes (`[JsonConverter]`) or through a `JsonSerializerOptions` extension method.
- Ensures consistency with the base `MimeType` value object.

## Use cases
- **Serialization**: Convert `MimeType` value objects into JSON strings.
- **Validation**: Ensure only valid MIME type strings are accepted in JSON payloads.
- **Integration**: Plug directly into `System.Text.Json` configuration.

## Examples

### Example 1: DTO with `[JsonConverter]` attribute

```csharp
using System.Text.Json;
using System.Text.Json.Serialization;
using PosInformatique.Foundations.MediaTypes;
using PosInformatique.Foundations.MediaTypes.Json;

public class MediaResourceDto
{
    [JsonConverter(typeof(MimeTypeJsonConverter))]
    public MimeType? ContentType { get; set; }
}

// Serialization
var dto = new MediaResourceDto { ContentType = MimeType.Parse("application/json") };
var json = JsonSerializer.Serialize(dto);
// Result: {"ContentType":"application/json"}

// Deserialization
var input = "{ \"ContentType\": \"image/png\" }";
var deserialized = JsonSerializer.Deserialize<MediaResourceDto>(input);

Console.WriteLine(deserialized!.ContentType); // "image/png"
```

### Example 2: Use `AddMediaTypesConverters()` without attributes

The library provides an extension method `AddMediaTypesConverters()` on `JsonSerializerOptions` to register the `MimeTypeJsonConverter` globally.

```csharp
using System.Text.Json;
using PosInformatique.Foundations.MediaTypes;
using PosInformatique.Foundations.MediaTypes.Json;

public class FileMetadataDto
{
    public MimeType? ContentType { get; set; }
}

var options = new JsonSerializerOptions()
    .AddMediaTypesConverters(); // Registers MimeTypeJsonConverter

// Serialization
var dto = new FileMetadataDto
{
    ContentType = MimeType.Parse("application/pdf")
};

var json = JsonSerializer.Serialize(dto, options);
// Result: {"ContentType":"application/pdf"}

// Deserialization
var input = "{ \"ContentType\": \"text/plain\" }";
var deserialized = JsonSerializer.Deserialize<FileMetadataDto>(input, options);

Console.WriteLine(deserialized!.ContentType); // "text/plain"
```

### Example 3: Null and invalid values

```csharp
using System.Text.Json;
using PosInformatique.Foundations.MediaTypes;

public class DocumentDto
{
    public MimeType? ContentType { get; set; }
}

var options = new JsonSerializerOptions().AddMediaTypesConverters();

// Null value
var jsonWithNull = "{ \"ContentType\": null }";
var docWithNull = JsonSerializer.Deserialize<DocumentDto>(jsonWithNull, options);
// docWithNull.ContentType is null

// Invalid MIME type -> throws JsonException
var invalidJson = "{ \"ContentType\": \"not a mime\" }";
try
{
    var invalidDoc = JsonSerializer.Deserialize<DocumentDto>(invalidJson, options);
}
catch (JsonException ex)
{
    Console.WriteLine(ex.Message); // "'not a mime' is not a valid MIME type."
}
```

## Links
- [NuGet package: MediaTypes.Json](https://www.nuget.org/packages/PosInformatique.Foundations.MediaTypes.Json/)
- [NuGet package: MediaTypes (core library)](https://www.nuget.org/packages/PosInformatique.Foundations.MediaTypes/)
- [Source code](https://github.com/PosInformatique/PosInformatique.Foundations)