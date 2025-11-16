# PosInformatique.Foundations.Text.Templating.Scriban

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.Text.Templating.Scriban)](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating.Scriban/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.Text.Templating.Scriban)](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating.Scriban/)

## Introduction

This package provides a simple way to generate text using [Scriban](https://github.com/scriban/scriban) templates.

You define a Scriban template string with mustache-style syntax (`{{ }}`) and the library renders it to a
`TextWriter` by using a `ScribanTextTemplate<TModel>` implementation.
The model properties are automatically exposed to the template.

## Install

You can install the package from [NuGet](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating.Scriban/):

```powershell
dotnet add package PosInformatique.Foundations.Text.Templating.Scriban
```

## Features

- Render text from Scriban templates using mustache-style syntax
- Strongly-typed model with automatic property exposure
- Supports both POCO objects and `ExpandoObject`
- Simple integration with `ITextTemplateRenderContext`
- Lightweight and fast text generation

## Basic usage

### 1. Create a model

Define a model class with the data you want to render:

```csharp
public class EmailModel
{
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime Date { get; set; }
}
```

### 2. Create a Scriban template

Define a Scriban template string using mustache-style syntax:

```csharp
var templateContent = @"
Hello {{ Name }},

Your email address is: {{ Email }}
Today is: {{ Date }}

Thank you!
";
```

### 3. Use `ScribanTextTemplate<TModel>.RenderAsync()`

Create a `ScribanTextTemplate<TModel>` instance and call `RenderAsync()`:

```csharp
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using PosInformatique.Foundations.Text.Templating;
using PosInformatique.Foundations.Text.Templating.Scriban;

// Example of a simple ITextTemplateRenderContext implementation
public class TextTemplateRenderContext : ITextTemplateRenderContext
{
    public TextTemplateRenderContext(IServiceProvider serviceProvider)
    {
        this.ServiceProvider = serviceProvider;
    }

    public IServiceProvider ServiceProvider { get; }
}

public static class ScribanTemplateSample
{
    public static async Task GenerateAsync()
    {
        var templateContent = @"
Hello {{ Name }},

Your email address is: {{ Email }}
Today is: {{ Date }}

Thank you!
";

        // Create the Scriban text template
        var template = new ScribanTextTemplate<EmailModel>(templateContent);

        // Create a model
        var model = new EmailModel
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Date = DateTime.UtcNow
        };

        // Build the context (can use an empty IServiceProvider if not needed)
        var context = new TextTemplateRenderContext(serviceProvider: null);

        using var writer = new StringWriter();

        // Render the template
        await template.RenderAsync(model, writer, context, CancellationToken.None);

        var result = writer.ToString();
        Console.WriteLine(result);
    }
}
```

Output:

```
Hello John Doe,

Your email address is: john.doe@example.com
Today is: 2025-01-16 10:30:00

Thank you!
```

### 4. Using ExpandoObject

You can also use `ExpandoObject` for dynamic models:

```csharp
dynamic model = new ExpandoObject();
model.Name = "Alice";
model.Email = "alice@example.com";
model.Date = DateTime.UtcNow;

var templateContent = "Hello {{ Name }}, your email is {{ Email }}.";
var template = new ScribanTextTemplate<ExpandoObject>(templateContent);

using var writer = new StringWriter();
await template.RenderAsync(model, writer, context, CancellationToken.None);

Console.WriteLine(writer.ToString());
// Output: Hello Alice, your email is alice@example.com.
```

## Scriban syntax

Scriban supports a rich template syntax. Here are some common examples:

### Variables

```
Hello {{ Name }}!
```

### Conditionals

```
{{ if IsActive }}
User is active
{{ else }}
User is inactive
{{ end }}
```

### Loops

```
{{ for item in Items }}
- {{ item.Name }}
{{ end }}
```

### Filters

```
{{ Name | upcase }}
{{ Date | date.to_string '%Y-%m-%d' }}
```

For more details, see the [Scriban documentation](https://github.com/scriban/scriban/blob/master/doc/language.md).

## Links

- [NuGet package: Text.Templating (core library)](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating/)
- [NuGet package: Text.Templating.Scriban](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating.Scriban/)
- [Source code](https://github.com/PosInformatique/PosInformatique.Foundations)
- [Scriban GitHub repository](https://github.com/scriban/scriban)