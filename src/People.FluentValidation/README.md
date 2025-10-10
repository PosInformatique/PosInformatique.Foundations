# PosInformatique.Foundations.People.FluentValidation

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.People.FluentValidation)](https://www.nuget.org/packages/PosInformatique.Foundations.People.FluentValidation/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.People.FluentValidation)](https://www.nuget.org/packages/PosInformatique.Foundations.People.FluentValidation/)

## Introduction
This package provides [FluentValidation](https://fluentvalidation.net/) extensions for validating first names and last names using the
`FirstName` and `LastName` value objects from [PosInformatique.Foundations.People](https://www.nuget.org/packages/PosInformatique.Foundations.People/).

It simplifies the integration of robust name validation into your [FluentValidation](https://fluentvalidation.net/) rules,
ensuring that string properties conform to the defined business rules for first and last names.

## Install
You can install the package from NuGet:

```powershell
dotnet add package PosInformatique.Foundations.People.FluentValidation
```

## Features
- [FluentValidation](https://fluentvalidation.net/) extension for first name and last name validation based on the business rules
of the [PosInformatique.Foundations.People](https://www.nuget.org/packages/PosInformatique.Foundations.People/) package.
- Uses the same parsing and validation rules as the `FirstName` and `LastName` value objects
- Clear and consistent error messages
> `null` values are accepted (combine with `NotNull()` validator to forbid nulls)

## Use cases
- **Validation**: Ensure that user inputs for first and last names adhere to your domain's business rules.
- **Type safety**: Leverage the strong typing of `FirstName` and `LastName` within your validation logic.
- **Consistency**: Apply a single, robust name validation logic across all projects using FluentValidation.

## Examples

### Validating FirstName
```csharp
public class PersonValidator : AbstractValidator<Person>
{
    public PersonValidator()
    {
        // FirstName must be a valid FirstName (e.g., "John", "Jean-Pierre")
        // Null values are allowed by default. Use NotNull() to disallow.
        RuleFor(x => x.FirstName).MustBeFirstName();

        // Example with NotNull()
        RuleFor(x => x.FirstName)
            .NotNull()
            .MustBeFirstName();
    }
}

public class Person
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}

// Usage
var validator = new PersonValidator();

// Valid
var result1 = validator.Validate(new Person { FirstName = "John", LastName = "DOE" }); // IsValid: true

// Invalid (contains invalid character)
var result2 = validator.Validate(new Person { FirstName = "John_123", LastName = "DOE" }); // IsValid: false

// Invalid (too long)
var result3 = validator.Validate(new Person { FirstName = new string('A', 51), LastName = "DOE" }); // IsValid: false

// Null (valid by default for MustBeFirstName)
var result4 = validator.Validate(new Person { FirstName = null, LastName = "DOE" }); // IsValid: true

// Null (invalid if NotNull() is used)
var validatorNotNull = new PersonValidator();
validatorNotNull.RuleFor(x => x.FirstName).NotNull().MustBeFirstName();
var result5 = validatorNotNull.Validate(new Person { FirstName = null, LastName = "DOE" }); // IsValid: false
```

### Validating LastName
```csharp
public class PersonValidator : AbstractValidator<Person>
{
    public PersonValidator()
    {
        // LastName must be a valid LastName (e.g., "DOE", "SMITH-JOHNSON")
        // Null values are allowed by default. Use NotNull() to disallow.
        RuleFor(x => x.LastName).MustBeLastName();

        // Example with NotNull()
        RuleFor(x => x.LastName)
            .NotNull()
            .MustBeLastName();
    }
}

public class Person
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}

// Usage
var validator = new PersonValidator();

// Valid
var result1 = validator.Validate(new Person { FirstName = "John", LastName = "DOE" }); // IsValid: true

// Invalid (contains invalid character)
var result2 = validator.Validate(new Person { FirstName = "John", LastName = "DOE_123" }); // IsValid: false

// Invalid (too long)
var result3 = validator.Validate(new Person { FirstName = "John", LastName = new string('A', 51) }); // IsValid: false

// Null (valid by default for MustBeLastName)
var result4 = validator.Validate(new Person { FirstName = "John", LastName = null }); // IsValid: true

// Null (invalid if NotNull() is used)
var validatorNotNull = new PersonValidator();
validatorNotNull.RuleFor(x => x.LastName).NotNull().MustBeLastName();
var result5 = validatorNotNull.Validate(new Person { FirstName = "John", LastName = null }); // IsValid: false
```

## Links
- [NuGet package: People.FluentValidation](https://www.nuget.org/packages/PosInformatique.Foundations.People.FluentValidation/)
- [NuGet package: People (core library)](https://www.nuget.org/packages/PosInformatique.Foundations.People/)
- [Source code](https://github.com/PosInformatique/PosInformatique.Foundations)
- [FluentValidation](https://fluentvalidation.net/)
