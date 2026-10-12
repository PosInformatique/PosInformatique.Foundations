# PosInformatique.Foundations.Text.Templating

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.Text.Templating)](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.Text.Templating)](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating/)

## Introduction

This package provides a small abstraction to generate text from templates, independently of the underlying templating engine.

It defines the `TextTemplate<TModel>` base class and the `ITextTemplateRenderContext` interface, which can be implemented by concrete template engines.

Currently only the following text engine implementations are provided in [PosInformatique.Foundations](https://github.com/PosInformatique/PosInformatique.Foundations):
- [PosInformatique.Foundations.Text.Templating.Razor](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating.Razor/)
- [PosInformatique.Foundations.Text.Templating.Scriban](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating.Scriban/)

## Install

You can install the package from [NuGet](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating/):

```powershell
dotnet add package PosInformatique.Foundations.Text.Templating
```

## Features

- Abstraction to represent a text template with a strongly-typed model: `TextTemplate<TModel>`
- Asynchronous rendering API through `RenderAsync`
- Pluggable rendering context via `ITextTemplateRenderContext` to access services during template execution
- Engine-agnostic design: can be used with different template engines (Razor, etc.)
- `ConstantTextTemplate<TModel>` implementation to generate a constant (fixed) text, regardless of the data model

## Usage

This package only provides the abstraction (base classes and interfaces).
To actually render templates using Razor components, use one of the dedicated implementation packages:

- [PosInformatique.Foundations.Text.Templating.Razor](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating.Razor/)
- [PosInformatique.Foundations.Text.Templating.Scriban](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating.Scriban/)

## Trimming and Native AOT

The implementations of `TextTemplate<TModel>` (Razor, Scriban,...) access the members of the `TModel` model through **reflection**.
The trimmer cannot detect this usage statically, so when your application is published with trimming or Native AOT enabled,
the model type and its members can be removed, which causes errors at runtime.

The `TModel` type passed to `RenderAsync()` **must therefore be preserved** by the application using the `DynamicDependencyAttribute`.
Apply it on the method (or the class) which calls `RenderAsync()`, and reference the model type:

```csharp
using System.Diagnostics.CodeAnalysis;

public class MyEmailModel
{
    public string UserName { get; set; }

    public DateTime Now { get; set; }
}

public class EmailSender
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(MyEmailModel))]
    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "MyEmailModel is preserved by the DynamicDependency attribute.")]
    public async Task SendAsync(TextTemplate<MyEmailModel> template, MyEmailModel model, TextWriter writer, ITextTemplateRenderContext context)
    {
        await template.RenderAsync(model, writer, context);
    }
}
```

Notes:

- Every type of the model graph accessed by the template (nested objects, items of collections,...) must be preserved as well, by adding one `DynamicDependencyAttribute` per type.
- `RenderAsync()` is annotated with `RequiresUnreferencedCode`: without the `UnconditionalSuppressMessage` (or `RequiresUnreferencedCode` on your own method), the trimming analyzer raises the `IL2026` warning on the call.

## Links

- [NuGet package: Text.Templating (core library)](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating/)
- [NuGet package: Text.Templating.Razor](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating.Razor/)
- [NuGet package: Text.Templating.Scriban](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating.Scriban/)
- [Source code](https://github.com/PosInformatique/PosInformatique.Foundations)