# PosInformatique.Foundations.Bcl

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.Bcl)](https://www.nuget.org/packages/PosInformatique.Foundations.Bcl/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.Bcl)](https://www.nuget.org/packages/PosInformatique.Foundations.Bcl/)

## Introduction

Extensions for common .NET types. This package supports .NET 8, .NET 9, .NET 10 and .NET 11,
trimming and Native AOT. The extensions are in the `System` namespace.

## Install

```powershell
dotnet add package PosInformatique.Foundations.Bcl
```

## Date / Time

`GetUtcToday()` returns the current UTC date from a `TimeProvider`. `GetLocalToday()` returns the
date in the provider's `LocalTimeZone`, which is not necessarily the user's time zone. The two
dates can differ around midnight.

```csharp
using System;

var utcToday = TimeProvider.System.GetUtcToday();
var localToday = TimeProvider.System.GetLocalToday();
```

## Reflection

`IsGeneric(genericType)` checks whether a type has the specified generic type definition.
It does not search base types or implemented interfaces. The argument must be a generic type
definition; otherwise, an `ArgumentException` is thrown.

```csharp
using System;
using System.Collections.Generic;

var isList = typeof(List<int>).IsGeneric(typeof(List<>)); // true
var isDictionary = typeof(List<int>).IsGeneric(typeof(Dictionary<,>)); // false
```

## String

`Truncate(maxLength)` returns the original string if it fits, or a prefix of at most the specified
number of UTF-16 code units. A negative length throws `ArgumentOutOfRangeException`. Truncation
may split a surrogate pair.

```csharp
using System;

var text = "Hello world".Truncate(5); // "Hello"
```

## Uri

`AppendPath(paths)` appends path components to an absolute URI while preserving its query and fragment.
Null and empty components are ignored, and leading and trailing slashes between components are
normalized to one slash. A relative URI throws `InvalidOperationException`.

```csharp
using System;

var uri = new Uri("https://example.com/api?key=1")
    .AppendPath("users", "42"); // https://example.com/api/users/42?key=1
```

All methods throw `ArgumentNullException` for null arguments, except individual `AppendPath()` path
components, which may be null and are ignored.

## Changelog

See [CHANGELOG.md](./CHANGELOG.md) for release history.

## Links

- [NuGet package](https://www.nuget.org/packages/PosInformatique.Foundations.Bcl/)
- [Source code](https://github.com/PosInformatique/PosInformatique.Foundations)