# PosInformatique.Foundations.MediaTypes.EntityFramework

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.MediaTypes.EntityFramework)](https://www.nuget.org/packages/PosInformatique.Foundations.MediaTypes.EntityFramework/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.MediaTypes.EntityFramework)](https://www.nuget.org/packages/PosInformatique.Foundations.MediaTypes.EntityFramework/)

## Introduction

Provides **Entity Framework Core** integration for the `MimeType` value object from
[PosInformatique.Foundations.MediaTypes](https://www.nuget.org/packages/PosInformatique.Foundations.MediaTypes/).
This package enables seamless mapping of MIME types as strongly-typed properties in Entity Framework Core entities.

It ensures proper SQL type mapping, validation, and conversion to `VARCHAR(128)` when persisted to the database.

## Install

You can install the package from NuGet:

```powershell
dotnet add package PosInformatique.Foundations.MediaTypes.EntityFramework
```

This package depends on the base package [PosInformatique.Foundations.MediaTypes](https://www.nuget.org/packages/PosInformatique.Foundations.MediaTypes/).

## Features

- Provides an extension method `IsMimeType()` to configure EF Core properties for `MimeType`.
- Maps to `VARCHAR(128)` database columns using the SQL type `MimeType` (you must define the SQL type `MimeType` mapped to `VARCHAR(128)` in your database).
- Ensures validation and safe conversion to/from database fields.
- Built on top of the core `MimeType` value object.

## Use cases

- **Entity mapping**: enforce strong typing for MIME types at the persistence layer.
- **Consistency**: ensure the same rules are applied in your entities and database.
- **Safety**: prevent invalid or malformed MIME type strings being stored in your database.

## Examples

> ⚠️ To use `IsMimeType()`, you must first define the SQL type `MimeType` mapped to `VARCHAR(128)` in your database.
> For SQL Server, you can create it with:

```sql
CREATE TYPE MimeType FROM VARCHAR(128) NOT NULL;
```

### Example: Configure an entity

```csharp
using Microsoft.EntityFrameworkCore;
using PosInformatique.Foundations.MediaTypes;

public class Document
{
    public int Id { get; set; }

    public MimeType ContentType { get; set; }
}

public class ApplicationDbContext : DbContext
{
    public DbSet<Document> Documents => Set<Document>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Document>()
            .Property(d => d.ContentType)
            .IsMimeType();
    }
}
```

This will configure the `ContentType` property of the `Document` entity with:

- `VARCHAR(128)` (non-unicode) column length
- SQL column type `MimeType`
- Proper conversion between `MimeType` and `string`

## Links

- [NuGet package: MediaTypes.EntityFramework](https://www.nuget.org/packages/PosInformatique.Foundations.MediaTypes.EntityFramework/)
- [NuGet package: MediaTypes (core library)](https://www.nuget.org/packages/PosInformatique.Foundations.MediaTypes/)
- [Source code](https://github.com/PosInformatique/PosInformatique.Foundations)