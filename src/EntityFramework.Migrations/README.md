# PosInformatique.Foundations.EntityFramework.Migrations

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.EntityFramework.Migrations)](https://www.nuget.org/packages/PosInformatique.Foundations.EntityFramework.Migrations/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.EntityFramework.Migrations)](https://www.nuget.org/packages/PosInformatique.Foundations.EntityFramework.Migrations/)

## Introduction

Provides extension methods for the **Entity Framework Core** `MigrationBuilder` to simplify common
migration operations that are not natively supported, such as renaming foreign key and primary key
constraints.

## Install

You can install the package from NuGet:

```powershell
dotnet add package PosInformatique.Foundations.EntityFramework.Migrations
```

## Features

- Provides a `RenameForeignKey()` extension method to rename an existing foreign key constraint.
- Provides a `RenamePrimaryKey()` extension method to rename an existing primary key constraint.
- Currently supports only the **SQL Server** provider (`Microsoft.EntityFrameworkCore.SqlServer`).
- Throws a `NotSupportedException` when used with a provider other than SQL Server.

## Use cases

- **Migration authoring**: rename constraints created by Entity Framework Core migrations without writing raw SQL by hand.
- **Consistency**: keep constraint renaming logic centralized and reusable across migrations.
- **Safety**: values are escaped to avoid SQL injection issues when building the underlying `sp_rename` statement.

## Examples

### Example: Rename a foreign key constraint

```csharp
using Microsoft.EntityFrameworkCore.Migrations;

public partial class RenameOrderCustomerForeignKey : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameForeignKey("FK_Orders_Customers_CustomerId", "FK_Orders_Customer");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameForeignKey("FK_Orders_Customer", "FK_Orders_Customers_CustomerId");
    }
}
```

### Example: Rename a primary key constraint

```csharp
using Microsoft.EntityFrameworkCore.Migrations;

public partial class RenameOrderPrimaryKey : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenamePrimaryKey("PK_Orders", "PK_Order");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenamePrimaryKey("PK_Order", "PK_Orders");
    }
}
```

### Example: Rename a constraint in a non-default schema

```csharp
migrationBuilder.RenameForeignKey("FK_Orders_Customers_CustomerId", "FK_Orders_Customer", schema: "sales");
```

## Links

- [NuGet package: EntityFramework.Migrations](https://www.nuget.org/packages/PosInformatique.Foundations.EntityFramework.Migrations/)
- [Source code](https://github.com/PosInformatique/PosInformatique.Foundations)