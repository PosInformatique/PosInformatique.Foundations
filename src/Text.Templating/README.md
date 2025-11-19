# PosInformatique.Foundations.Text.Templating

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.Text.Templating)](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.Text.Templating)](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating/)

## Introduction

This package provides a small abstraction to generate text from templates, independently of the underlying templating engine.

It defines the `TextTemplate<TModel>` base class and the `ITextTemplateRenderContext` interface, which can be implemented by concrete template engines.

Currently only the following text engine implementation are provided in [PosInformatique.Foundations](https://github.com/PosInformatique/PosInformatique.Foundations):
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

## Usage

This package only provides the abstraction (base classes and interfaces).
To actually render templates using Razor components, use one of the dedicated implementation package:

- [PosInformatique.Foundations.Text.Templating.Razor](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating.Razor/)
- [PosInformatique.Foundations.Text.Templating.Scriban](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating.Scriban/)

## Links

- [NuGet package: Text.Templating (core library)](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating/)
- [NuGet package: Text.Templating.Razor](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating.Razor/)
- [NuGet package: Text.Templating.Scriban](https://www.nuget.org/packages/PosInformatique.Foundations.Text.Templating.Scriban/)
- [Source code](https://github.com/PosInformatique/PosInformatique.Foundations)