# PosInformatique.Foundations.EntityFramework.Converters

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.EntityFramework.Converters)](https://www.nuget.org/packages/PosInformatique.Foundations.EntityFramework.Converters/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.EntityFramework.Converters)](https://www.nuget.org/packages/PosInformatique.Foundations.EntityFramework.Converters/)

## Introduction

Provides a collection of basic **Entity Framework Core** value converters and extension methods to
simplify common property configuration scenarios.

The converters currently available are:

- `IsUtc()`: an extension method for `PropertyBuilder<DateTime>`, used to configure `DateTime` properties as UTC.
- `EnumToBooleanConverter<TEnum>`: a `ValueConverter` to map an enumeration with only two values to a `bool` column.

More converters will be added in future versions of this package.

## Install

You can install the package from NuGet:

```powershell
dotnet add package PosInformatique.Foundations.EntityFramework.Converters
```

## Available converters

### `DateTime` as UTC

- Provides an `IsUtc()` extension method to mark a `DateTime` (or nullable `DateTime`) property as UTC.
- Supports both `PropertyBuilder<DateTime>` and `PropertyBuilder<DateTime?>`.

#### ⚠️ Important: no actual date conversion is performed

The `IsUtc()` method does **not** convert the value between time zones. It only sets the `DateTimeKind` of the
value to `DateTimeKind.Utc` (using `DateTime.SpecifyKind`) when the value is read from or written to the
database, it never changes the underlying value.

SQL Server `datetime`/`datetime2` columns do not store any time zone information, and Entity Framework Core
returns `DateTime` values with `DateTimeKind.Unspecified` by default, which can lead to ambiguous or incorrect
behavior when comparing or serializing dates (for example with `System.Text.Json`, which treats `Unspecified`
dates differently than `Utc` dates).

This package is therefore intended **only** for applications that already persist their `DateTime` values as
UTC in `datetime`/`datetime2` SQL Server columns, and that consider the whole column (or table) to contain UTC
dates. If your database stores local (non-UTC) dates, you must convert the values to UTC yourself before using
this package, as `IsUtc()` will only flag the value as UTC without changing its actual value.

#### Examples

##### Example: Configure a `DateTime` property as UTC

```csharp
using Microsoft.EntityFrameworkCore;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(o => o.CreatedAt)
            .IsUtc();
    }
}
```

##### Example: Configure a nullable `DateTime` property as UTC

```csharp
builder.Property(o => o.ShippedAt)
    .IsUtc();
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