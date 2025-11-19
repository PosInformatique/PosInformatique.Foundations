# PosInformatique.Foundations.People.DataAnnotations

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.People.DataAnnotations)](https://www.nuget.org/packages/PosInformatique.Foundations.People.DataAnnotations/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.People.DataAnnotations)](https://www.nuget.org/packages/PosInformatique.Foundations.People.DataAnnotations/)

## Introduction
This package provides .NET `DataAnnotations` attributes to validate first names and last names using the `FirstName` and `LastName` value objects
from [PosInformatique.Foundations.People](https://www.nuget.org/packages/PosInformatique.Foundations.People/).

It allows you to apply robust name validation directly on your models with attributes like `[FirstName]` attribute and `[LastName]`, ensuring that string properties conform to the business rules for first and last names.

## Install
You can install the package from NuGet:

```powershell
dotnet add package PosInformatique.Foundations.People.DataAnnotations
```

## Features
- `DataAnnotations` attributes for first name and last name validation based on the business rules of the [PosInformatique.Foundations.People](https://www.nuget.org/packages/PosInformatique.Foundations.People/) package.
- Uses the same parsing and validation rules as the `FirstName` and `LastName` value objects.
- Clear and consistent error messages.
> `null` values are accepted (combine with `[Required]` attribute to forbid nulls).

## Examples

### Validating FirstName
```csharp
using System.ComponentModel.DataAnnotations;
using PosInformatique.Foundations.People.DataAnnotations;

public class Person
{
    // FirstName must be a valid FirstName (e.g., "John", "Jean-Pierre")
    // Null values are allowed by default. Use [Required] to disallow.
    [FirstName]
    public string? FirstName { get; set; }

    public string? LastName { get; set; }
}

// Usage
var person1 = new Person { FirstName = "John", LastName = "DOE" };
var context = new ValidationContext(person1);
var results = new List<ValidationResult>();
var isValid1 = Validator.TryValidateObject(person1, context, results, validateAllProperties: true); // true

var person2 = new Person { FirstName = "John_123", LastName = "DOE" };
context = new ValidationContext(person2);
results = new List<ValidationResult>();
var isValid2 = Validator.TryValidateObject(person2, context, results, validateAllProperties: true); // false

var person3 = new Person { FirstName = new string('A', 51), LastName = "DOE" };
context = new ValidationContext(person3);
results = new List<ValidationResult>();
var isValid3 = Validator.TryValidateObject(person3, context, results, validateAllProperties: true); // false

// Null (valid by default for [FirstName])
var person4 = new Person { FirstName = null, LastName = "DOE" };
context = new ValidationContext(person4);
results = new List<ValidationResult>();
var isValid4 = Validator.TryValidateObject(person4, context, results, validateAllProperties: true); // true

// Null (invalid if [Required] is used)
public class PersonRequiredFirstName
{
    [Required]
    [FirstName]
    public string? FirstName { get; set; }
}

var person5 = new PersonRequiredFirstName { FirstName = null };
context = new ValidationContext(person5);
results = new List<ValidationResult>();
var isValid5 = Validator.TryValidateObject(person5, context, results, validateAllProperties: true); // false
```

### Validating LastName
```csharp
using System.ComponentModel.DataAnnotations;
using PosInformatique.Foundations.People.DataAnnotations;

public class Person
{
    public string? FirstName { get; set; }

    // LastName must be a valid LastName (e.g., "DOE", "SMITH-JOHNSON")
    // Null values are allowed by default. Use [Required] to disallow.
    [LastName]
    public string? LastName { get; set; }
}

// Usage
var person1 = new Person { FirstName = "John", LastName = "DOE" };
var context = new ValidationContext(person1);
var results = new List<ValidationResult>();
var isValid1 = Validator.TryValidateObject(person1, context, results, validateAllProperties: true); // true

var person2 = new Person { FirstName = "John", LastName = "DOE_123" };
context = new ValidationContext(person2);
results = new List<ValidationResult>();
var isValid2 = Validator.TryValidateObject(person2, context, results, validateAllProperties: true); // false

var person3 = new Person { FirstName = "John", LastName = new string('A', 51) };
context = new ValidationContext(person3);
results = new List<ValidationResult>();
var isValid3 = Validator.TryValidateObject(person3, context, results, validateAllProperties: true); // false

// Null (valid by default for [LastName])
var person4 = new Person { FirstName = "John", LastName = null };
context = new ValidationContext(person4);
results = new List<ValidationResult>();
var isValid4 = Validator.TryValidateObject(person4, context, results, validateAllProperties: true); // true

// Null (invalid if [Required] is used)
public class PersonRequiredLastName
{
    public string? FirstName { get; set; }

    [Required]
    [LastName]
    public string? LastName { get; set; }
}

var person5 = new PersonRequiredLastName { FirstName = "John", LastName = null };
context = new ValidationContext(person5);
results = new List<ValidationResult>();
var isValid5 = Validator.TryValidateObject(person5, context, results, validateAllProperties: true); // false
```

## Links
- [NuGet package: People.DataAnnotations](https://www.nuget.org/packages/PosInformatique.Foundations.People.DataAnnotations/)
- [NuGet package: People (core library)](https://www.nuget.org/packages/PosInformatique.Foundations.People/)
- [Source code](https://github.com/PosInformatique/PosInformatique.Foundations)
