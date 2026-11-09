# PosInformatique.Foundations.Linq.Expressions

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.Linq.Expressions)](https://www.nuget.org/packages/PosInformatique.Foundations.Linq.Expressions/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.Linq.Expressions)](https://www.nuget.org/packages/PosInformatique.Foundations.Linq.Expressions/)

## Introduction

This package provides expression tree utilities for .NET 8, .NET 9 and .NET 10.

`OrExpressionBuilder<T>` combines `Expression<Func<T, bool>>` predicates into a single expression using short-circuiting logical OR, without depending on Entity Framework Core or any other query provider.

The builder is available in the `System.Linq.Expressions` namespace.

## Install

You can install the package from [NuGet](https://www.nuget.org/packages/PosInformatique.Foundations.Linq.Expressions/):

```powershell
dotnet add package PosInformatique.Foundations.Linq.Expressions
```

## Features

- Combine any number of predicates using `Expression.OrElse`.
- Preserve insertion order and short-circuit evaluation when the expression is executed in memory.
- Unify predicate parameters while preserving nested lambda parameters.
- Return an always-true predicate when no expressions are added, so optional filters do not exclude any items.
- Build expression trees without introducing `Expression.Invoke` or compiling the predicates.

## Usage

```csharp
using System.Linq.Expressions;

var entities = new[]
{
    new Entity { Name = "A" },
    new Entity { Name = "B" },
    new Entity { Name = "C" },
    new Entity { Name = "D" },
    new Entity { Name = "E" },
};

var builder = new OrExpressionBuilder<Entity>();
builder.Add(entity => entity.Name == "B");
builder.Add(entity => entity.Name == "C");
builder.Add(entity => entity.Name == "E");

Expression<Func<Entity, bool>> predicate = builder.Build();

// Returns the entities named B, C and E.
var results = entities.AsQueryable().Where(predicate).ToArray();

public sealed class Entity
{
    public string Name { get; set; } = string.Empty;
}
```

For an `IEnumerable<T>`, compile the result and pass the delegate to `Where`:

```csharp
var results = entities.Where(predicate.Compile()).ToArray();
```

### Behavior

- `Add(null)` throws `ArgumentNullException`.
- With no predicates, `Build()` returns an expression equivalent to `entity => true`. This intentionally means "no filter", rather than the mathematical identity of OR (`false`).
- With one predicate, `Build()` preserves its body and parameter.
- `Build()` does not clear the builder. Later calls include any subsequently added predicates, without modifying previously built expressions.
- For `IQueryable<T>`, translation and evaluation behavior depend on the query provider and the expressions supplied. The builder does not guarantee that every predicate can be translated by every provider.

## Trimming and Native AOT

The library supports trimming and Native AOT. Building expressions does not use reflection or generate dynamic code. Expression execution and query translation remain the responsibility of the consuming application or query provider.

## Links

- [NuGet package](https://www.nuget.org/packages/PosInformatique.Foundations.Linq.Expressions/)
- [Source code](https://github.com/PosInformatique/PosInformatique.Foundations)
