# PosInformatique.Foundations.PhoneNumbers.FluentValidation

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.PhoneNumbers.FluentValidation)](https://www.nuget.org/packages/PosInformatique.Foundations.PhoneNumbers.FluentValidation/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.PhoneNumbers.FluentValidation)](https://www.nuget.org/packages/PosInformatique.Foundations.PhoneNumbers.FluentValidation/)

## Introduction

This package provides [FluentValidation](https://fluentvalidation.net/) integration for the `PhoneNumber` value object
from [PosInformatique.Foundations.PhoneNumbers](../PhoneNumbers/README.md).

It adds a dedicated validator and extension method to validate that string properties contain valid phone numbers
in **E.164** format, using the same parsing and validation logic as the core `PhoneNumber` type.

## Install

You can install the package from [NuGet](https://www.nuget.org/packages/PosInformatique.Foundations.PhoneNumbers.FluentValidation/):

```powershell
dotnet add package PosInformatique.Foundations.PhoneNumbers.FluentValidation
```

## Features

- FluentValidation integration for phone number validation
- Extension method `MustBePhoneNumber()` for `string` properties
- Validation based on the core `PhoneNumber.IsValid()` logic (E.164 format)
- `null` values are considered valid by default (combine with `NotNull()` / `NotEmpty()` when needed)
- Consistent validation rules across your application

## Use cases

- Validate incoming DTOs or commands that contain phone numbers as strings
- Ensure only valid E.164 phone numbers are accepted at the boundaries of your system
- Reuse the same validation logic used by the `PhoneNumber` value object everywhere

## Examples

### Basic validation with MustBePhoneNumber

```csharp
using FluentValidation;

public sealed class ContactDto
{
    public string Name { get; set; } = default!;
    public string? Mobile { get; set; }
}

public sealed class ContactDtoValidator : AbstractValidator<ContactDto>
{
    public ContactDtoValidator()
    {
        RuleFor(x => x.Mobile)
            .MustBePhoneNumber(); // Validates Mobile as an E.164 phone number (or null)
    }
}
```

- If `Mobile` is `null`, the rule passes.
- If `Mobile` is not `null`, it must be a valid phone number in E.164 format, otherwise validation fails with the default message:
  - `"'Mobile' must be a valid phone number in E.164 format."`

### Combine with NotNull / NotEmpty

If you want to make the phone number mandatory, combine `MustBePhoneNumber()` with standard FluentValidation rules:

```csharp
public sealed class RequiredContactDtoValidator : AbstractValidator<ContactDto>
{
    public RequiredContactDtoValidator()
    {
        RuleFor(x => x.Mobile)
            .NotEmpty()
            .MustBePhoneNumber();
    }
}
```

This enforces:

- `Mobile` is not `null` or empty.
- `Mobile` must be a valid phone number in E.164 format.

## Links

- [NuGet package: PhoneNumbers (core library)](https://www.nuget.org/packages/PosInformatique.Foundations.PhoneNumbers/)
- [NuGet package: PhoneNumbers.FluentValidation](https://www.nuget.org/packages/PosInformatique.Foundations.PhoneNumbers.FluentValidation/)
- [Source code](https://github.com/PosInformatique/PosInformatique.Foundations)