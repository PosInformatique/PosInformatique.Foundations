# PosInformatique.Foundations.Collections

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.Collections)](https://www.nuget.org/packages/PosInformatique.Foundations.Collections/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.Collections)](https://www.nuget.org/packages/PosInformatique.Foundations.Collections/)

## Introduction

Extensions for .NET collections and sequences. This package supports .NET 8, .NET 9, .NET 10
and .NET 11, trimming and Native AOT. The extensions are in the
`System.Collections.Generic` namespace.

## Install

```powershell
dotnet add package PosInformatique.Foundations.Collections
```

## Usage

### Add a sequence of elements to a collection

`AddRange()` adds each element of an `IEnumerable<T>` to an `ICollection<T>` in enumeration order.

```csharp
using System.Collections.Generic;

ICollection<int> numbers = new List<int> { 1, 2 };
IEnumerable<int> additional = new[] { 3, 4 };
numbers.AddRange(additional); // 1, 2, 3, 4
```

### Create a sequence containing one element

`AsSequence()` returns a sequence containing exactly the supplied value. If the value is itself a
collection, the sequence contains that collection as one element rather than enumerating it.

```csharp
using System.Collections.Generic;

var sequence = 42.AsSequence(); // A sequence containing 42
```

### Find the index of a matching element

`IndexOf()` searches an `IReadOnlyList<T>` and returns the first matching index, or `null` when
no element matches.

```csharp
using System.Collections.Generic;

IReadOnlyList<int> numbers = new[] { 3, 2, 1 };
int? index = numbers.IndexOf(n => n == 2); // 1
```

### Remove matching elements from a collection

`RemoveAll()` removes elements matching a predicate from an `ICollection<T>`. It throws
`ArgumentException` when the collection is read-only.

```csharp
using System.Collections.Generic;

ICollection<int> numbers = new List<int> { 1, 2, 3, 4 };
numbers.RemoveAll(n => n > 3); // 1, 2, 3
```

### Swap two elements in a list

`Swap()` exchanges two elements in an `IList<T>`. An invalid index throws
`ArgumentOutOfRangeException`.

```csharp
using System.Collections.Generic;

IList<int> list = new List<int> { 1, 2, 3 };
list.Swap(0, 2); // 3, 2, 1
```

### Create a read-only snapshot of a sequence

`ToReadOnlyCollection()` enumerates any `IEnumerable<T>`, including an `IReadOnlyList<T>`, into a
new `ReadOnlyCollection<T>`. Later changes to the source are not reflected in the snapshot.

```csharp
using System.Collections.Generic;
using System.Collections.ObjectModel;

var source = new List<int> { 1, 2, 3 };
IReadOnlyList<int> list = source;
ReadOnlyCollection<int> snapshot = list.ToReadOnlyCollection();
source.Add(4); // snapshot still contains 1, 2, 3
```

Null arguments, including the value passed to `AsSequence()`, throw `ArgumentNullException`.

When a concrete collection already has an instance method with the same name, C# selects that
instance method. Use an `ICollection<T>` reference to call the collection extensions in that case.

The framework also defines a `System.Collections.Generic.CollectionExtensions` class. Use extension
method syntax as shown above; referring to the class by name is ambiguous when both assemblies are
referenced.

## Changelog

See [CHANGELOG.md](./CHANGELOG.md) for release history.

## Links

- [NuGet package](https://www.nuget.org/packages/PosInformatique.Foundations.Collections/)
- [Source code](https://github.com/PosInformatique/PosInformatique.Foundations)