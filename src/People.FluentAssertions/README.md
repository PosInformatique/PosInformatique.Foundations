# PosInformatique.Foundations.People.FluentAssertions

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.People.FluentAssertions)](https://www.nuget.org/packages/PosInformatique.Foundations.People.FluentAssertions/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.People.FluentAssertions)](https://www.nuget.org/packages/PosInformatique.Foundations.People.FluentAssertions/)

## Introduction
Assertion extensions for `FirstName` and `LastName` value objects from
[PosInformatique.Foundations.People](https://www.nuget.org/packages/PosInformatique.Foundations.FluentAssertions/)
using [FluentAssertions](https://fluentassertions.com/).

This package resolves the ambiguity that occurs when using [FluentAssertions](https://fluentassertions.com/) directly on these value
objects and provides a simple, idiomatic assertion API where `Be(string)` compares the literal content
(case-sensitive for `FirstName`, uppercased for `LastName` as per normalization).

Why this package?
- `FirstName` and `LastName` implement both `IEnumerable<char>` and `IComparable<T>`.
- Calling `Should()` from [FluentAssertions](https://fluentassertions.com/) on such types leads to a compile-time ambiguity:
  - The call is ambiguous between the following methods or properties: `AssertionExtensions.Should<T>(IEnumerable<T>)`
    and `AssertionExtensions.Should<T>(IComparable<T>)`
- This package introduces dedicated `Should()` extensions that return specialized assertions to avoid that ambiguity.

## Install
You can install the package from NuGet:

```powershell
dotnet add package PosInformatique.Foundations.People.FluentAssertions
```

## Features
- `Should()` extension for `FirstName` returning `FirstNameAssertions`.
- `Should()` extension for `LastName` returning `LastNameAssertions`.
- `Be(string)` compares the value object to a string using the normalized literal content:
  - For `FirstName`, comparison is case-sensitive against the normalized first name (e.g., "Jean-Pierre").
  - For `LastName`, comparison is case-sensitive against the normalized last name (e.g., "DUPONT").

## Examples
Basic usage with `FirstName`:
```csharp
using FluentAssertions;
using PosInformatique.Foundations.People;

var firstName = FirstName.Create("jean-pierre");

// Passes: "jean-pierre" is normalized to "Jean-Pierre"
firstName.Should().Be("Jean-Pierre");

// Fails (case-sensitive): expected "JEAN-PIERRE"
firstName.Should().Be("JEAN-PIERRE");
```

Basic usage with `LastName`:
```csharp
using FluentAssertions;
using PosInformatique.Foundations.People;

var lastName = LastName.Create("dupont");

// Passes: normalization uppercases to "DUPONT"
lastName.Should().Be("DUPONT");

// Fails (case-sensitive): expected "Dupont"
lastName.Should().Be("Dupont");
```

Using with your domain model:
```csharp
using FluentAssertions;
using PosInformatique.Foundations.People;

public sealed class User
{
    public User(string firstName, string lastName)
    {
        FirstName = FirstName.Create(firstName);
        LastName = LastName.Create(lastName);
    }

    public FirstName FirstName { get; }
    public LastName LastName { get; }
}

var user = new User("alice", "martin");
user.FirstName.Should().Be("Alice");
user.LastName.Should().Be("MARTIN");
```

## Links
- [NuGet package: People.FluentAssertions](https://www.nuget.org/packages/PosInformatique.Foundations.People.FluentAssertions/)
- [NuGet package: People (core library)](https://www.nuget.org/packages/PosInformatique.Foundations.FluentAssertions/)
- [Source code](https://github.com/PosInformatique/PosInformatique.Foundations)
