# Updating the vendored MimeKit code

This document describes how to update the MimeKit source code copied in the `Mailkit` folder
of the `EmailAddresses.csproj` project. It is written to be used by a developer or by an AI agent.

## Context

- `EmailAddress.cs` relies on `MailboxAddress` (and `ParserOptions`) to validate e-mail addresses.
- `MailboxAddress` is in the **MimeKit** repository (`https://github.com/jstedfast/MimeKit`), not in MailKit.
- To avoid a NuGet dependency, the needed source code is copied (vendored) in the `Mailkit` folder,
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
git checkout vX.Y.Z   # target version
```

### 2. Replace the existing vendored code

- Delete the content of the `Mailkit` folder of `EmailAddresses.csproj`.
- Copy `MimeKit\MailboxAddress.cs`, `MimeKit\ParserOptions.cs` and `MimeKit\RfcComplianceMode.cs` as is.
- In Visual Studio, disable the automatic formatting before editing
  (Tools > Options > Text Editor > C# > Code Style > Formatting > General: uncheck the automatic formatting options).
  Do not use `Ctrl+K, Ctrl+D`.

### 3. Copy the dependencies

- Build the project.
- For each missing type, copy the matching file from the MimeKit repository into the `Mailkit` folder.
- Rebuild and repeat until the project compiles.

### 4. Make all types internal

Use "Replace in Files" (regex enabled) on the `Mailkit` folder:

- Find: `^(\s*)public (sealed |abstract |static |partial |readonly |unsafe )*(class|struct|enum|interface|delegate) `
- Replace: `$1internal $2$3 `

Check that there is no remaining public type:

```powershell
Select-String -Path .\Mailkit\*.cs -Pattern '^\s*public\s+(sealed |abstract |static |partial |readonly |unsafe )*(class|struct|enum|interface|delegate)\s'
```

Members inside the types are not modified.

### 5. Prune the unused code

Do this step **after** step 4 so the analyzers and "Find All References" are reliable.

1. Start from the entry points listed above. Everything not reachable from them is a candidate for removal.
2. In each file, remove the unused:
   - constructors, methods, properties, fields, events, overloads, nested types;
   - `#if` blocks for target frameworks that are not used (keep the branch that is compiled);
   - `using` directives.
3. Remove the files that are not referenced anymore.
4. Useful tools:
   - IDE0051 (unused private member), IDE0052 (unread private member), IDE0060 (unused parameter),
     IDE0005 (unnecessary using) enabled as warnings in `.editorconfig`;
   - `Shift+F12` (Find All References) for `internal` members;
   - ReSharper / Rider solution-wide analysis if available.
5. Build after each batch of removals. Repeat until nothing more can be removed.

Remember: remove only whole code elements. Never change the body of what is kept.

### 6. Validation

- Build in `Release` without errors and without new warnings.
- Run the unit tests of the `EmailAddresses` project (they are the safety net for the upgrade).
- Compare with the previous vendored version to check that only expected changes appear:

```powershell
git diff --stat
git diff -w -- src/EmailAddresses/Mailkit
```

  Expected differences: upstream changes of the new version, removed code, `internal` keyword.
- Check that no `public` type remains (see step 4).
- Update the **Current version** section at the top of this file.

## Notes for an AI agent

- Follow the rules and the procedure strictly, in order.
- Do not run any code formatter.
- If a dependency seems to require changing the kept code, stop and report it instead of refactoring.
- At the end, give the list of the copied files, the removed members and the files deleted.
