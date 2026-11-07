# PosInformatique.Foundations.EntityFramework.Converters

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.EntityFramework.Converters)](https://www.nuget.org/packages/PosInformatique.Foundations.EntityFramework.Converters/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.EntityFramework.Converters)](https://www.nuget.org/packages/PosInformatique.Foundations.EntityFramework.Converters/)

## Introduction

Provides a collection of basic **Entity Framework Core** value converters and extension methods to
simplify common property configuration scenarios.

The converters and configuration extensions currently available are:

- `AssumeUtc()`: an extension method for `PropertyBuilder<DateTime>` and `PropertyBuilder<DateTime?>`. It assumes persisted values already represent UTC and sets their `DateTime.Kind` to `Utc`; it does not convert the time.
- `EnumToBooleanConverter<TEnum>`: a `ValueConverter` to map an enumeration with only two values to a `bool` column.

More converters will be added in future versions of this package.

## Install

You can install the package from NuGet:

```powershell
dotnet add package PosInformatique.Foundations.EntityFramework.Converters
```

## Available converters

### Assume UTC for a `DateTime` property

- `AssumeUtc()` assumes values in the database already represent UTC and sets `DateTime.Kind` to `Utc` when values are read from or written to the database.
- Supports both `PropertyBuilder<DateTime>` and `PropertyBuilder<DateTime?>`.

#### ⚠️ Important: this does not convert times to UTC

The `AssumeUtc()` method does **not** convert a value between time zones or change its clock time. It only sets
the `DateTime.Kind` to `DateTimeKind.Utc` (using `DateTime.SpecifyKind`) when the value is read from or written
to the database.

SQL Server `datetime`/`datetime2` columns do not store any time zone information, and Entity Framework Core
returns `DateTime` values with `DateTimeKind.Unspecified` by default, which can lead to ambiguous or incorrect
behavior when comparing or serializing dates (for example with `System.Text.Json`, which treats `Unspecified`
dates differently than `Utc` dates).

Use this extension only when the `DateTime` values persisted in your `datetime`/`datetime2` SQL Server columns
already represent UTC. If the database stores local (non-UTC) times, convert them to UTC yourself before using
this extension; otherwise, they will be labeled UTC without being converted.

#### Examples

##### Example: Assume a `DateTime` property contains UTC values

```csharp
using Microsoft.EntityFrameworkCore;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(o => o.CreatedAt)
            .AssumeUtc();
    }
}
```

##### Example: Assume a nullable `DateTime` property contains UTC values

```csharp
builder.Property(o => o.ShippedAt)
    .AssumeUtc();
```

### Enum to boolean

- Provides the `EnumToBooleanConverter<TEnum>` value converter to map an enumeration to a `bool` column (and vice versa).
- The `TEnum` type must be an enumeration containing exactly two distinct values, one that can be represented as `0` (`false`) and one that can be represented as `1` (`true`).
- An `ArgumentException` (wrapped in a `TypeInitializationException`) is thrown when accessing the `Instance` property if the enumeration does not comply with this constraint.

#### Examples

##### Example: Configure an enum property as a boolean column

```csharp
using Microsoft.EntityFrameworkCore;

public enum OrderStatus
{
    Pending = 0,
    Completed = 1,
}

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(o => o.Status)
            .HasConversion(EnumToBooleanConverter<OrderStatus>.Instance);
    }
}
```

## Links

- [NuGet package: EntityFramework.Converters](https://www.nuget.org/packages/PosInformatique.Foundations.EntityFramework.Converters/)
- [Source code](https://github.com/PosInformatique/PosInformatique.Foundations)