# PosInformatique.Foundations.EmailAddresses.EntityFramework

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.EmailAddresses.EntityFramework)](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses.EntityFramework/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.EmailAddresses.EntityFramework)](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses.EntityFramework/)

## Introduction
Provides **Entity Framework Core** integration for the `EmailAddress` value object from
[PosInformatique.Foundations.EmailAddresses](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses/).  
This package enables seamless mapping of RFC 5322 compliant email addresses as strongly-typed properties in Entity Framework Core entities.

It ensures proper SQL type mapping, validation, and conversion to `VARCHAR` when persisted to the database.

## Install
You can install the package from NuGet:

```powershell
dotnet add package PosInformatique.Foundations.EmailAddresses.EntityFramework
```

This package depends on the base package [PosInformatique.Foundations.EmailAddresses](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses/).

## Features
- Provides an extension method `IsEmailAddress()` to configure EF Core properties for `EmailAddress`.
- Maps to `VARCHAR(320)` database columns using the SQL type `EmailAddress` (you must define the SQL type `EmailAddress` mapped to `VARCHAR(320)` in your database).
- Ensures validation, normalization, and safe conversion to/from database fields.
- Built on top of the core `EmailAddress` value object.

## Use cases
- **Entity mapping**: enforce strong typing for email addresses at the persistence layer.
- **Consistency**: ensure the same validation rules are applied in your entities and database.
- **Safety**: prevent invalid strings being stored in your database

## Examples

> ⚠️ To use `IsEmailAddress()`, you must first define the SQL type `EmailAddress` mapped to `VARCHAR(320)` in your database.  
For SQL Server, you can create it with:

```sql
CREATE TYPE EmailAddress FROM VARCHAR(320) NOT NULL;
```

### Example: Configure an entity
```csharp
using Microsoft.EntityFrameworkCore;
using PosInformatique.Foundations;

public class User
{
    public int Id { get; set; }
    public EmailAddress Email { get; set; }
}

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .IsEmailAddress();
    }
}
```

This will configure the `Email` property of the `User` entity with:
- `VARCHAR(320)` (Non-unicode) column length
- SQL column type `EmailAddress`

## Links
- [NuGet package: EmailAddresses.EntityFramework](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses.EntityFramework/)
- [NuGet package: EmailAddresses (core library)](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses/)
- [Source code](https://github.com/PosInformatique/PosInformatique.Foundations)
