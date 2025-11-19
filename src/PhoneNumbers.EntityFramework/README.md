# PosInformatique.Foundations.PhoneNumbers.EntityFramework

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.PhoneNumbers.EntityFramework)](https://www.nuget.org/packages/PosInformatique.Foundations.PhoneNumbers.EntityFramework/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.PhoneNumbers.EntityFramework)](https://www.nuget.org/packages/PosInformatique.Foundations.PhoneNumbers.EntityFramework/)

## Introduction

This package provides Entity Framework Core integration for the `PhoneNumber`
value object from [PosInformatique.Foundations.PhoneNumbers](../PhoneNumbers/README.md).

It allows you to map `PhoneNumber` properties to a database column of SQL type `PhoneNumber`
(backed by `VARCHAR(16)`), using a dedicated value converter.

## Install

You can install the package from [NuGet](https://www.nuget.org/packages/PosInformatique.Foundations.PhoneNumbers.EntityFramework/):

```powershell
dotnet add package PosInformatique.Foundations.PhoneNumbers.EntityFramework
```

## Features

- Entity Framework Core support for the `PhoneNumber` value object
- Simple extension method `IsPhoneNumber()` to configure properties
- Maps `PhoneNumber` to a SQL column with type `PhoneNumber` (`VARCHAR(16)`, non-Unicode)
- Uses a `ValueConverter` to convert between `PhoneNumber` and its E.164 string representation

## Use cases

- Persist `PhoneNumber` in your EF Core entities without manual conversion logic
- Keep strong typing in your domain model while storing normalized E.164 strings in the database
- Enforce a consistent database schema for phone numbers (custom `PhoneNumber` type mapped to `VARCHAR(16)`)

## Examples

> ⚠️ To use `IsPhoneNumber()`, you must first define the SQL type `PhoneNumber` mapped to `VARCHAR(16)` in your database.
> For SQL Server, you can create it with:

```sql
CREATE TYPE MimeType FROM VARCHAR(16) NOT NULL;
```

### Configure a PhoneNumber property

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PosInformatique.Foundations.PhoneNumbers;

public sealed class Customer
{
    public int Id { get; set; }

    public PhoneNumber Phone { get; set; } = default!;
}

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(c => c.Phone)
               .IsPhoneNumber(); // Maps to SQL type PhoneNumber (VARCHAR(16))
    }
}
```

### Resulting database schema

The `IsPhoneNumber()` extension configures the property as:

- Non-Unicode (`IsUnicode(false)`)
- Maximum length: `16`
- Column type: `PhoneNumber` (which must be mapped in your database as `VARCHAR(16)`)

For example, your database column should look like:

```sql
Phone PhoneNumber NOT NULL
-- where `PhoneNumber` is mapped to VARCHAR(16)
```

### Value conversion

Under the hood, the extension uses a `ValueConverter<PhoneNumber, string>`:

- When saving, `PhoneNumber` is converted to its E.164 string representation (via `ToString()`).
- When loading, the stored string is parsed back to a `PhoneNumber` instance.

This ensures the database always stores the normalized E.164 value, while your code works with the strongly-typed `PhoneNumber` value object.

## Links

- [NuGet package: PhoneNumbers (core library)](https://www.nuget.org/packages/PosInformatique.Foundations.PhoneNumbers/)
- [NuGet package: PhoneNumbers.EntityFramework](https://www.nuget.org/packages/PosInformatique.Foundations.PhoneNumbers.EntityFramework/)
- [Source code](https://github.com/PosInformatique/PosInformatique.Foundations)