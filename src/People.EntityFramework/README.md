# PosInformatique.Foundations.People.EntityFramework

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.People.EntityFramework)](https://www.nuget.org/packages/PosInformatique.Foundations.People.EntityFramework/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.People.EntityFramework)](https://www.nuget.org/packages/PosInformatique.Foundations.People.EntityFramework/)

## Introduction
This package provides **Entity Framework Core** extensions and value converters to persist the
[PosInformatique.Foundations.People](https://www.nuget.org/packages/PosInformatique.Foundations.People/) value objects:
- `FirstName` stored as `NVARCHAR(50)` with proper conversion and comparisons.
- `LastName` stored as `NVARCHAR(50)` with proper conversion and comparisons.

It exposes fluent configuration helpers to simplify mapping in your DbContext model configuration.

## Install
You can install the package from NuGet:
```powershell
dotnet add package PosInformatique.Foundations.People.EntityFramework
```

This package depends on the base package [PosInformatique.Foundations.People](https://www.nuget.org/packages/PosInformatique.Foundations.People/).

## Features
- Provides an extension method `IsFirstName()` and `IsLastName()` to configure EF Core properties for `FirstName` and `LastName`.
- Easy EF Core mapping for `FirstName` and `LastName` properties.
- Maps to NVARCHAR(50), Unicode, non-fixed-length columns.
- Built on top of the core `FirstName` and `FirstName` value objects.
- Keeps domain normalization rules in the database boundary.

## Examples

### Configure model with IsFirstName and IsLastName
```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PosInformatique.Foundations.People;

public sealed class PersonEntity
{
    public int Id { get; set; }

    // Persisted as NVARCHAR(50) using the FirstName converter and comparer
    public FirstName FirstName { get; set; } = null!;

    // Persisted as NVARCHAR(50) using the LastName converter and comparer
    public LastName LastName { get; set; } = null!;
}

public sealed class PeopleDbContext : DbContext
{
    public DbSet<PersonEntity> People => Set<PersonEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var person = modelBuilder.Entity<PersonEntity>();

        person.HasKey(p => p.Id);

        // Configure FirstName as NVARCHAR(50) with conversions
        person.Property(p => p.FirstName)
              .IsFirstName();

        // Configure LastName as NVARCHAR(50) with conversions
        person.Property(p => p.LastName)
              .IsLastName();
    }
}
```

This will configure:
- The `FirstName` property of the `PersonEntity` entity with:
  - `NVARCHAR(50)` (Unicode) column length
- The `LastName` property of the `PersonEntity` entity with:
  - `NVARCHAR(50)` (Unicode) column length

### Saving and querying
```csharp
var options = new DbContextOptionsBuilder<PeopleDbContext>()
    .UseSqlServer(connectionString)
    .Options;

using var db = new PeopleDbContext(options);

// Insert
var person = new PersonEntity
{
    FirstName = FirstName.Create("jean-paul"), // normalized to "Jean-Paul"
    LastName = LastName.Create("dupont")       // normalized to "DUPONT"
};
db.Add(person);
await db.SaveChangesAsync();

// Query (comparison and ordering use normalized string values via comparer/converter)
var ordered = await db.People
    .OrderBy(p => p.LastName)   // "DUPONT" etc.
    .ThenBy(p => p.FirstName)   // "Jean-Paul" etc.
    .ToListAsync();
```

### Using With Separate Configuration Class
```csharp
public sealed class PersonEntityConfiguration : IEntityTypeConfiguration<PersonEntity>
{
    public void Configure(EntityTypeBuilder<PersonEntity> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.FirstName)
               .IsFirstName();

        builder.Property(p => p.LastName)
               .IsLastName();
    }
}

public sealed class PeopleDbContext : DbContext
{
    public DbSet<PersonEntity> People => Set<PersonEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PersonEntityConfiguration());
    }
}
```

## Notes
- The mapping enforces the maximum length defined by the domain (`FirstName.MaxLength` and `LastName.MaxLength`), ensuring alignment between code and database.
- Converters/Comparers guarantee consistent persistence and querying semantics with the normalized value objects.

## Links
- [NuGet package: People.EntityFramework](https://www.nuget.org/packages/PosInformatique.Foundations.People.EntityFramework/)
- [NuGet package: People (core library)](https://www.nuget.org/packages/PosInformatique.Foundations.People/)
- [Source code](https://github.com/PosInformatique/PosInformatique.Foundations)
