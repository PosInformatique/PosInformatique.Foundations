# PosInformatique.Foundations.EmailAddresses.FluentValidation

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.EmailAddresses.FluentValidation)](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses.FluentValidation/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.EmailAddresses.FluentValidation)](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses.FluentValidation/)

## Introduction
This package provides a [FluentValidation](https://fluentvalidation.net/) extension for validating email addresses using the
[EmailAddress](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses/) value object.

It ensures that only **valid RFC 5322 compliant email addresses** are accepted when validating string properties.

## Install
You can install the package from [NuGet](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses.FluentValidation/):

```powershell
dotnet add package PosInformatique.Foundations.EmailAddresses.FluentValidation
```

This package depends on the base package [PosInformatique.Foundations.EmailAddresses](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses/).

## Features
- [FluentValidation](https://fluentvalidation.net/) extension for email address validation
- Uses the same parsing and validation rules as the [EmailAddress](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses/) value object
- Clear and consistent error messages
> `null` values are accepted (combine with `NotNull()` validator to forbid nulls)

## Usage

### Basic validation
```csharp
using FluentValidation;

public class User
{
    public string Email { get; set; }
}

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(u => u.Email).MustBeEmailAddress();
    }
}
```

### Null values are ignored
```csharp
var validator = new UserValidator();

// Valid, because null is ignored
var result1 = validator.Validate(new User { Email = null });
Console.WriteLine(result1.IsValid); // True

// Valid, because it's a valid email
var result2 = validator.Validate(new User { Email = "alice@company.com" });
Console.WriteLine(result2.IsValid); // True

// Invalid, because it's not a valid email
var result3 = validator.Validate(new User { Email = "not-an-email" });
Console.WriteLine(result3.IsValid); // False
```

### Require non-null values
```csharp
public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty()             // Disallow null and empty
            .MustBeEmailAddress();
    }
}
```

## Links
- [NuGet package: EmailAddresses.FluentValidation](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses.FluentValidation/)
- [NuGet package: EmailAddresses (core library)](https://www.nuget.org/packages/PosInformatique.Foundations.EmailAddresses/)
- [Source code](https://github.com/PosInformatique/PosInformatique.Foundations)
- [FluentValidation](https://fluentvalidation.net/)
