# Updating the vendored MimeKit code

This document describes how to update the MimeKit source code copied in the `MailKit` folder
of the `EmailAddresses.csproj` project. It is written to be used by a developer or by an AI agent.

## Context

- `EmailAddress.cs` relies on `MailboxAddress` (and `ParserOptions`) to validate e-mail addresses.
- `MailboxAddress` is in the **MimeKit** repository (`https://github.com/jstedfast/MimeKit`), not in MailKit.
- To avoid a NuGet dependency, the needed source code is copied (vendored) in the `MailKit` folder,
  then pruned to keep only what is required.

## Current version

- MimeKit tag: `5.0.0`
- Date of the last update: `2026-11-16`

## Entry points (what `EmailAddress.cs` uses)

Only the code reachable from these members must be kept. Everything else is removed.

```csharp
// Options used by EmailAddress.cs
var options = new ParserOptions()
{
    AllowAddressesWithoutDomain = false,
    AddressParserComplianceMode = RfcComplianceMode.Strict,
    Rfc2047ComplianceMode = RfcComplianceMode.Strict,
    AllowUnquotedCommasInAddresses = false,
};

// Parsing
MailboxAddress.TryParse(ParserOptions options, string text, out MailboxAddress mailbox)

// Properties read on the result
mailbox.Address
mailbox.IsInternational
```

Types explicitly used: `MailboxAddress`, `ParserOptions`, `RfcComplianceMode`.
Other types (`InternetAddress`, `Rfc2047`, `CharsetUtils`, etc.) are kept only if they are
reachable from the members above.

If `EmailAddress.cs` starts using other members, add them to this list before pruning.

## Rules

1. Take the source code from MimeKit as is (same namespaces, same file names, same code).
2. Keep the MIT license header of each copied file.
3. All types must be `internal` (no `public` type, no `public` nested type).
4. Do not reformat the code (no auto-format, no `dotnet format`, same line endings as the original files).
5. No refactoring: no renaming, no change in the logic of the kept code.
6. Pruning is allowed only by **removing** code (members, fields, properties, constructors, overloads,
   events, `#if` blocks, unused `using`, whole files). The body of a kept member is never modified.
7. Keep the XML comments of the kept members as is.

## Procedure

### 1. Get the sources

```powershell
git clone https://github.com/jstedfast/MimeKit.git $env:TEMP\MimeKit
cd $env:TEMP\MimeKit
$tag = "5.0.0"        # replace with the requested release tag
git ls-remote --tags --refs origin "*$tag*"
git checkout $tag     # use the exact tag found above; do not assume a "v" prefix
git rev-parse HEAD    # record the resolved commit for reproducibility
```

For the 5.0.0 update, the upstream tag is `5.0.0`, not `v5.0.0`. Verify the tag name for
each future release before checking it out.

### 2. Replace the existing vendored code

- Delete the content of `src\EmailAddresses\MailKit` (preserve `UPDATE-MimeKit.md`, which is outside it).
- Copy `MimeKit\MailboxAddress.cs`, `MimeKit\ParserOptions.cs` and `MimeKit\RfcComplianceMode.cs` as is.
- In Visual Studio, disable the automatic formatting before editing
  (Tools > Options > Text Editor > C# > Code Style > Formatting > General: uncheck the automatic formatting options).
  Do not use `Ctrl+K, Ctrl+D`.

### 3. Copy the dependencies

- Build `src\EmailAddresses\EmailAddresses.csproj` in Release. Copy each missing dependency from
  the checked-out MimeKit source and rebuild until all supported target frameworks compile.
- Copy only source files needed by the dependency closure; avoid copying whole directories such as
  `Utils` or `Encodings`.

### 4. Make all types internal

Use "Replace in Files" (regex enabled) on `src\EmailAddresses\MailKit`:

- Find: `^(\s*)public (sealed |abstract |static |partial |readonly |unsafe )*(class|struct|enum|interface|delegate) `
- Replace: `$1internal $2$3 `

Check that there is no remaining public type:

```powershell
Get-ChildItem .\src\EmailAddresses\MailKit -Recurse -Filter *.cs |
    Select-String -Pattern '^\s*public\s+(sealed |abstract |static |partial |readonly |unsafe )*(class|struct|enum|interface|delegate)\s'
```

Members inside the types are not modified.

### 5. Prune the unused code

Do this step **after** step 4 so the analyzers and "Find All References" are reliable.

1. Start from the entry points listed above. Everything not reachable from them is a candidate for removal.
2. In each file, remove the unused:
   - constructors, methods, properties, fields, events, overloads, nested types;
   - `#if` branches that are not compiled for any target framework in `EmailAddresses.csproj`;
   - `using` directives.
3. Remove the files that are not referenced anymore.
4. Useful tools:
   - Check the effective `.editorconfig` first: the vendored folder may suppress analyzer diagnostics.
     `dotnet format analyzers` can be asked to inspect selected diagnostics without changing files:

     ```powershell
     dotnet format analyzers src\EmailAddresses\EmailAddresses.csproj --no-restore --verify-no-changes `
         --diagnostics IDE0051 IDE0052 IDE0060 IDE0005 --severity info --include MailKit
     ```

     IDE diagnostics are not proof that internal members are unused; use reference search as well.
   - `Shift+F12` (Find All References) for `internal` members;
   - `rg` or equivalent to search references across `MailKit` and the `EmailAddresses` tests;
   - ReSharper / Rider solution-wide analysis if available.
5. Before removing a member, trace calls from every retained member, including calls within the same
   class. An apparently unused helper may still be required by another retained method (for example,
   decoder `Flush` methods call `Reset`). Conversely, if a helper's only callers are removed, prune
   that helper and its now-unreachable dependencies together.
6. Check each overload separately. A byte-array parser entry point may be unused while byte-array
   input types remain necessary internally—for example, display-name decoding can reuse the RFC 2047
   decoder. Remove the unused factory overload, not types that still have reachable call sites.
7. Test-only hooks and acceleration paths are candidates only after searching production code and
   this repository's tests. In the 5.0.0 integration, `HwAccelDecode` had no callers after the
   byte-array test hooks were pruned, so its SIMD kernels and acceleration option were also removable.
   Similarly, verify filters such as `CreateNewLineFilter` have no references before deleting them.
8. Build after each coherent batch of removals and repeat the reference search until no more code
   can be removed.

Remember: remove only whole code elements. Never change the body of what is kept.

### 6. Validation

- Build in `Release` without errors and without new warnings.
- Run the `EmailAddresses` unit tests (the safety net for the upgrade):

  ```powershell
  dotnet build src\EmailAddresses\EmailAddresses.csproj -c Release
  dotnet test tests\EmailAddresses.Tests\EmailAddresses.Tests.csproj -c Release
  ```

- Check `git status --short` as well as `git diff --stat`; newly copied, untracked source files
  do not appear in `git diff --stat`.
- Compare with the previous vendored version to check that only expected changes appear:

```powershell
git diff --stat
git diff -w -- src\EmailAddresses\MailKit
```

  Expected differences: upstream changes of the new version, removed code, `internal` keyword.
- Check that no `public` type remains (see step 4).
- Update the **Current version** section at the top of this file.

## Notes for an AI agent

- Follow the rules and the procedure strictly, in order.
- Do not run any code formatter.
- Preserve the exact upstream tag/commit and source license headers; do not infer a tag prefix.
- Do not infer that a whole file or input type is dead from one unused public overload. Follow its
  remaining call sites from the parser, including RFC 2047 name decoding.
- After pruning, search for both dangling references and newly orphaned files/methods, then run the
  Release build and tests across all target frameworks.
- If a dependency seems to require changing the kept code, stop and report it instead of refactoring.
- At the end, give the list of the copied files, the removed members and the files deleted.
