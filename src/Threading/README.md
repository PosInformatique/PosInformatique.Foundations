# PosInformatique.Foundations.Threading

[![NuGet version](https://img.shields.io/nuget/v/PosInformatique.Foundations.Threading)](https://www.nuget.org/packages/PosInformatique.Foundations.Threading/)
[![NuGet downloads](https://img.shields.io/nuget/dt/PosInformatique.Foundations.Threading)](https://www.nuget.org/packages/PosInformatique.Foundations.Threading/)

## Introduction

This library provides utilities and extensions related to the `System.Threading` APIs of .NET.
It supports .NET 8, .NET 9, .NET 10 and .NET 11, trimming and Native AOT.

Introduced in v1.7.0, `Debouncer` delays an asynchronous action and cancels the previous invocation
whenever a new invocation is requested. It is available in the `PosInformatique.Foundations.Threading` namespace.

## Install

```powershell
dotnet add package PosInformatique.Foundations.Threading --version 1.7.0
```

## Usage

```csharp
using PosInformatique.Foundations.Threading;

using var debouncer = new Debouncer(
    async token =>
    {
        await Task.Delay(TimeSpan.FromMilliseconds(50), token);
        Console.WriteLine("Action executed.");
    },
    TimeSpan.FromMilliseconds(300));

var first = debouncer.InvokeAsync();
var second = debouncer.InvokeAsync();

// Only the latest pending invocation executes the action.
await Task.WhenAll(first, second);
```

## Behavior

- Each `InvokeAsync()` cancels the previous invocation and restarts the delay.
- The action receives a cancellation token and should observe it. Cancellation is cooperative:
  a running action that ignores its token may overlap a newer action.
- `IsInProgress` indicates whether the latest invocation is waiting or executing. It does not track
  older actions that continue after cancellation.
- Cancellation requested by a newer invocation or `Dispose()` completes the invocation normally.
  Other exceptions, including unrelated cancellation, are propagated to the caller.
- `Dispose()` requests cancellation without waiting for completion. Await the invocation tasks to
  observe their completion and any exceptions, including exceptions from cancellation callbacks.
- Calling `InvokeAsync()` after disposal throws `ObjectDisposedException`.
- A null action throws `ArgumentNullException`. The delay must be between zero and
  4,294,967,294 milliseconds; otherwise, the constructor throws `ArgumentOutOfRangeException`.
- Concurrent invocations and disposal are synchronized.

## Changelog

See [CHANGELOG.md](./CHANGELOG.md) for release history.

## Links

- [NuGet package](https://www.nuget.org/packages/PosInformatique.Foundations.Threading/)
- [Source code](https://github.com/PosInformatique/PosInformatique.Foundations)
