# PosInformatique.Foundations.MediaTypes

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.MediaTypes)](https://www.nuget.org/packages/PosInformatique.Foundations.MediaTypes/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.MediaTypes)](https://www.nuget.org/packages/PosInformatique.Foundations.MediaTypes/)

## Introduction

[PosInformatique.Foundations.MediaTypes](https://www.nuget.org/packages/PosInformatique.Foundations.MediaTypes) provides
a lightweight way to represent media types (MIME types) in .NET.
It offers an immutable `MimeType` value object, a set of well-known media types, helpers for mapping between
file extensions and media types, and a few convenience extension methods.

## Install

You can install the package from NuGet:

```powershell
dotnet add package PosInformatique.Foundations.MediaTypes
```

## Features

- Immutable `MimeType` value object (`type/subtype`, e.g. `application/json`, `image/png`).
- Parsing and safe parsing from `string` (`Parse` / `TryParse`).
- Resolve a `MimeType` from a file extension (with or without leading dot).
- Resolve a default file extension from a `MimeType`.
- Set of common `application/*` and `image/*` media types.
- Simple extension methods, e.g. `IsPdf()` and `IsImage()`.

## Usage

### Parsing media types

```csharp
using PosInformatique.Foundations.MediaTypes;

var json = MimeType.Parse("application/json");
Console.WriteLine(json.Type);    // "application"
Console.WriteLine(json.Subtype); // "json"

if (MimeType.TryParse("image/png", out var png))
{
    Console.WriteLine(png); // "image/png"
}
```

### Well-known media types

```csharp
using PosInformatique.Foundations.MediaTypes;

var pdf = MimeTypes.Application.Pdf;   // application/pdf
var docx = MimeTypes.Application.Docx; // application/vnd.openxmlformats-officedocument.wordprocessingml.document
var jpeg = MimeTypes.Image.Jpeg;       // image/jpeg
```

### From file extension to media type

```csharp
using PosInformatique.Foundations.MediaTypes;

var pdfFromExt = MimeType.FromExtension(".pdf"); // application/pdf
var pngFromExt = MimeType.FromExtension("png");  // image/png

// Unknown extensions fall back to application/octet-stream
var unknown = MimeType.FromExtension(".unknown"); // application/octet-stream
```

### From media type to default file extension

```csharp
using PosInformatique.Foundations.MediaTypes;

var pdf = MimeTypes.Application.Pdf;
var pdfExtension = pdf.GetExtension(); // ".pdf"

var webp = MimeTypes.Image.WebP;
var webpExtension = webp.GetExtension(); // ".webp"
```

### Extension methods

```csharp
using PosInformatique.Foundations.MediaTypes;

var mimeType = MimeTypes.Application.Pdf;

if (mimeType.IsPdf())
{
    Console.WriteLine("This is a PDF document.");
}

var image = MimeTypes.Image.Png;
if (image.IsImage())
{
    Console.WriteLine("This is an image type.");
}

var drawing = MimeTypes.Image.Dwg;
if (drawing.IsAutoCad())
{
    Console.WriteLine("This is an AutoCAD drawing type.");
}
```

## API overview

### MimeType

- Immutable value object representing `type/subtype`.
- Implements `IEquatable<MimeType>` and `IParsable<MimeType>`.
- Main members:
  - `string Type { get; }`
  - `string Subtype { get; }`
  - `static MimeType Parse(string s)`
  - `static MimeType Parse(string s, IFormatProvider? provider)`
  - `static bool TryParse(string? s, out MimeType? result)`
  - `static bool TryParse(string? s, IFormatProvider? provider, out MimeType? result)`
  - `static MimeType FromExtension(string extension)`
  - `string GetExtension()`

### MimeTypes

Provides common media types and mapping helpers.

- `MimeTypes.Application`
  - `OctetStream` (`application/octet-stream`)
  - `Pdf` (`application/pdf`)
  - `Docx` (`application/vnd.openxmlformats-officedocument.wordprocessingml.document`)

- `MimeTypes.Image`
  - `Bmp` (`image/bmp`)
  - `Dxf` (`image/x-dxf`)
  - `Dwg` (`image/x-dwg`)
  - `Jpeg` (`image/jpeg`)
  - `Png` (`image/png`)
  - `Tiff` (`image/tiff`)
  - `WebP` (`image/webp`)

### MimeTypeExtensions

- `bool IsAutoCad(this MimeType mimeType)`
- `bool IsImage(this MimeType mimeType)`
- `bool IsPdf(this MimeType mimeType)`

## Links

- [NuGet package](https://www.nuget.org/packages/PosInformatique.Foundations.MediaTypes/)
- [Source code](https://github.com/PosInformatique/PosInformatique.Foundations)