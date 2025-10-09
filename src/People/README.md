# PosInformatique.Foundations.People

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.People)](https://www.nuget.org/packages/PosInformatique.Foundations.People/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.People)](https://www.nuget.org/packages/PosInformatique.Foundations.People/)

## Introduction
This package provides lightweight, strongly-typed value objects to standardize first names and last names across your applications:
- `FirstName`: normalized, properly cased first names.
- `LastName`: normalized, fully uppercased last names.

It also includes:
- `IPerson`: a small interface to represent nominative people with FirstName and LastName.
- Extension methods on `IPerson` for consistent display/order names and initials.
- `NameNormalizer`: helper methods when you only have the separated first/last names.

Typical use cases: domain entities (User, Customer, Employee, Contact), consistent UI display (Blazor components, lists), alphabetical ordering (directories), and avatar initials.

## Install
You can install the package from NuGet:
```powershell
dotnet add package PosInformatique.Foundations.People
```

## Features
- Strongly-typed FirstName and LastName with validation and normalization
- Business rules:
  - `FirstName`: title-cased words; letters only; separators: space, hyphen
  - `LastName`: fully uppercased; letters only; separators: space, hyphen
  - No consecutive/trailing separators; max length 50
- Acts like read-only char arrays: indexer and Length
- Implements `IParsable<T>`, `IFormattable`, `IComparable<T>`, `IEquatable<T>`
- Implicit conversions to/from string
- `IPerson` interface for nominative person abstraction
- `PersonExtensions` for display name, ordering name, and initials
- `NameNormalizer` utilities for first/last names without `IPerson`

## Business rules

### FirstName
- Max length: 50.
- Allowed characters: letters only; separators: ' ' and '-'.
- Normalization:
  - Each word starts uppercase and continues lowercase (e.g., "John", "John Henri-Smith").
  - No consecutive separators, ths trailing separators are removed.

### LastName
- Max length: 50.
- Allowed characters: letters only; separators: ' ' and '-'.
- Normalization:
  - Entire value uppercased (e.g., "DOE", "SMITH-JOHNSON").
  - No consecutive separators, the trailing separators are removed.

### FirstName examples

#### Create / implicit conversion
```csharp
// Implicit conversion validates and normalizes:
FirstName firstName = "john  henri-smith";   // -> "John Henri-Smith"

// Explicit create:
var firstName2 = FirstName.Create("  alice--marie "); // -> "Alice-Marie"

// IsValid
var ok = FirstName.IsValid("Élodie");       // true
var notOk = FirstName.IsValid("John_123");  // false
```

#### Parse / TryParse
```csharp
// Parse (throws on invalid)
var firstName = FirstName.Parse("béAtrice", provider: null); // "Béatrice"

// TryParse (no exceptions)
if (FirstName.TryParse("jeAn  -  pierre", provider: null, out var firstName))
{
    Console.WriteLine(firstName); // "Jean-Pierre"
}
```

#### Length and indexer
```csharp
var firstName = FirstName.Create("Jean-Pierre");
var len = firstName.Length;     // 11
var firstLetter = firstName[0]; // 'J'
var dash = firstName[4];        // '-'

// Enumerate characters
foreach (var c in firstName) { /* ... */ }
```

#### Comparisons and equality
```csharp
var a = FirstName.Create("Alice");
var b = FirstName.Create("ALICE");   // normalized: "Alice"

Console.WriteLine(a == b);  // true
Console.WriteLine(a != b);  // false
Console.WriteLine(a <= b);  // true
Console.WriteLine(a.CompareTo(b)); // 0

var list = new List<FirstName> { FirstName.Create("Zoé"), FirstName.Create("Ana") };
list.Sort(); // alphabetical by normalized value
```

### LastName examples

#### Create / implicit conversion
```csharp
LastName lastName = "dupond  durand";  // -> "DUPOND DURAND"
var lastName2 = LastName.Create("le--gall"); // -> "LE GALL"
var ok = LastName.IsValid("O'Connor"); // false (apostrophe not allowed)
```

#### Parse / TryParse
```csharp
var parsed = LastName.Parse("martin", provider: null); // "MARTIN"

if (LastName.TryParse("van  -  damme", provider: null, out var lastName))
{
    Console.WriteLine(lastName); // "VAN DAMME"
}
```

#### Length and indexer
```csharp
var lastName = LastName.Create("LE GALL");
var len = lastName.Length;     // 7
var firstLetter = lastName[0]; // 'L'
var space = lastName[2];       // ' '

// Enumerate characters
foreach (var c in lastName) { /* ... */ }
```

#### Comparisons and equality
```csharp
var a = LastName.Create("DURAND");
var b = LastName.Create("durand"); // normalized to "DURAND"

Console.WriteLine(a == b);  // true
Console.WriteLine(a < LastName.Create("MARTIN")); // true

var list = new List<LastName> { LastName.Create("ZOLA"), LastName.Create("ABEL") };
list.Sort(); // alphabetical by normalized value
```

### IPerson
`IPerson` represents a nominative person with a normalized `FirstName` and `LastName`. Implement this in domain types like User, Customer, Employee, Contact.

```csharp
public sealed class User : IPerson
{
    public User(FirstName firstName, LastName lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public FirstName FirstName { get; }
    public LastName LastName { get; }
}
```

You can also accept string and normalize:
```csharp
public sealed class Customer : IPerson
{
    public Customer(string firstName, string lastName)
    {
        // Implicit conversions validate and normalize
        FirstName = firstName;
        LastName = lastName;
    }

    public FirstName FirstName { get; }
    public LastName LastName { get; }
}
```

### PersonExtensions
Utilities for consistent display, ordering, and initials on any IPerson.

- `GetFullNameForDisplay()`: "First-Name LASTNAME" (e.g., "John DOE"). Use in UI display (e.g., Blazor component).
- `GetFullNameForOrder()`: "LASTNAME First-Name" (e.g., "DOE John"). Use for alphabetical directories by last name.
- `GetInitials()`: first letter of FirstName + first letter of LastName (e.g., "JD"). Use as fallback when avatar image is not available.

Examples:
```csharp
var user = new User("jean-paul", "dupont");

var display = user.GetFullNameForDisplay(); // "Jean-Paul DUPONT"
var order = user.GetFullNameForOrder();     // "DUPONT Jean-Paul"
var initials = user.GetInitials();          // "JD"
```

Blazor example:
```csharp
@code {
    [Parameter] public IPerson Person { get; set; } = default!;
}
<h3>@Person.GetFullNameForDisplay()</h3>
<span class="avatar">@Person.GetInitials()</span>
```

Sorting by order name:
```csharp
var people = new List<IPerson>
{
    new User("alice", "martin"),
    new User("bob", "durand"),
    new User("Élodie", "zola")
};

var ordered = people
    .OrderBy(p => p.GetFullNameForOrder(), StringComparer.Ordinal)
    .ToList();
// "DURAND Bob", "MARTIN Alice", "ZOLA Élodie"
```

### NameNormalizer
When you only have separate `FirstName` and `LastName` (not an `IPerson`), use `NameNormalizer`.

- `GetFullNameForDisplay(firstName, lastName)` => "First-Name LASTNAME"
- `GetFullNameForOrder(firstName, lastName)` => "LASTNAME First-Name"

Examples:
```csharp
var firstName = FirstName.Create("marie-claire");
var lastName = LastName.Create("le gall");

var display = NameNormalizer.GetFullNameForDisplay(firstName, lastName); // "Marie-Claire LE GALL"
var order = NameNormalizer.GetFullNameForOrder(firstName, lastName);     // "LE GALL Marie-Claire"
```

### Error handling tips
- Use `TryParse` / `TryCreate` when you want to avoid exceptions:
```csharp
if (FirstName.TryParse(inputFirstName, null, out var firstName) &&
    LastName.TryParse(inputLastName, null, out var lastName))
{
    var user = new User(firstName, lastName);
}
else
{
    // handle invalid inputs
}
```

## Links
- [NuGet package: People (core library)](https://www.nuget.org/packages/PosInformatique.Foundations.People/)
- [Source code](https://github.com/PosInformatique/PosInformatique.Foundations)
