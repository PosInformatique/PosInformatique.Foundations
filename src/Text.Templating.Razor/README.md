### PosInformatique.Foundations.Text.Templating.Razor

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.Text.Templating.Razor)](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating.Razor/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.Text.Templating.Razor)](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating.Razor/)

## Introduction

This package provides a simple way to generate text from Razor components (views) outside of ASP.NET Core MVC/Blazor pages.
It is an implementation of the core [PosInformatique.Foundations.Text.Templating](../Text.Templating/README.md) library.

You define a Razor component with a `Model` parameter, and the library renders it to a `TextWriter` by using a `RazorTextTemplate<TModel>` implementation. The Razor component can also inject any service registered in the application `IServiceCollection`.

## Install

You can install the package from [NuGet](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating.Razor/):

```powershell
dotnet add package PosInformatique.Foundations.Text.Templating.Razor
```

## Features

- Render text from Razor components (Blazor-style components)
- Strongly-typed model passed via a `Model` parameter
- Integrates with dependency injection (`IServiceCollection` / `IServiceProvider`)
- Ability to inject any registered service directly in the Razor component with `@inject`
- Simple registration via `AddRazorTextTemplating(IServiceCollection)`

## Basic usage

### 1. Register the Razor text templating

You must register the Razor text templating rendering infrastructure in your DI container:

```csharp
var services = new ServiceCollection();

// Register application services
services.AddLogging();

// Register Razor text templating
services.AddRazorTextTemplating();

// Build the service provider used as context
var serviceProvider = services.BuildServiceProvider();
```

### 2. Create a Razor component (view)

Create a Razor component that will be used as a template, for example `HelloTemplate.razor`:

```razor
@using System
@using Microsoft.Extensions.Logging

@inherits ComponentBase

@code {
    // The model automatically injected by RazorTextTemplate<TModel>
    [Parameter]
    public MyEmailModel? Model { get; set; }

    protected override void OnInitialized()
    {
        // You can use Blazor event as usual (OnInitialized, OnParametersSet, etc.)
        this.Model.Now = DateTime.UtcNow;
    }
}

Hello this.Model.UserName !
Today is this.Model.Now:U
```

Key points:

- The model is received via a `Model` parameter (it is automatically set by the library).
- You can inject any service registered in `IServiceCollection` using `@inject` (or `[Inject]` attribute in code-behind).

### 3. Use `RazorTextTemplate<TModel>.RenderAsync()`

You can now create a `RazorTextTemplate<TModel>` instance, build a rendering context that exposes an `IServiceProvider`, and call `RenderAsync()`:

```csharp
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PosInformatique.Foundations.Text.Templating;
using PosInformatique.Foundations.Text.Templating.Razor;

// Example of a simple ITextTemplateRenderContext implementation
public class TextTemplateRenderContext : ITextTemplateRenderContext
{
    public TextTemplateRenderContext(IServiceProvider serviceProvider)
    {
        this.ServiceProvider = serviceProvider;
    }

    public IServiceProvider ServiceProvider { get; }
}

public static class RazorTemplateSample
{
    public static async Task GenerateAsync()
    {
        var services = new ServiceCollection();
        services.AddRazorTextTemplating();

        var serviceProvider = services.BuildServiceProvider();

        // Create the Razor text template that uses the HelloTemplate component
        var template = new RazorTextTemplate<MyEmailModel>(typeof(HelloTemplate));

        // Build the context that provides IServiceProvider
        var context = new TextTemplateRenderContext(serviceProvider);

        using var writer = new StringWriter();

        // Render the template with a string model
        var model = new MyEmailModel { UserName = "John" };
        await template.RenderAsync(model, writer, context, CancellationToken.None);

        var result = writer.ToString();
        Console.WriteLine(result);
    }
}
```

### 4. Injecting other services in the Razor view

Any service registered in your `IServiceCollection` and available through `IServiceProvider` can be injected in the Razor component.

Example:

```csharp
@using MyApp.Services
@inherits ComponentBase

@code {
    [Parameter]
    public MyEmailModel? Model { get; set; }

    [Inject]
    public IDateTimeProvider DateTimeProvider { get; set; } = default!

    [Inject]
    public IMyFormatter Formatter { get; set; } = default!
}

Hello @Model?.Name,

Current time: @this.DateTimeProvider.UtcNow
Formatted data: @this.Formatter.Format(Model)
```

As long as `IDateTimeProvider` and `IMyFormatter` are registered in the `IServiceCollection`, they are available during template rendering.

## Links

- [NuGet package: Emailing.Templates.Razor](https://www.nuget.org/packages/PosInformatique.Foundations.Emailing.Templates.Razor/)
- [NuGet package: Text.Templating (core library)](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating/)
- [NuGet package: Text.Templating.Razor](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating/)
- [Source code](https://github.com/PosInformatique/PosInformatique.Foundations)