# Quench.NET <!-- omit in toc -->

Customisable exception-quenching library, for .NET

![Language](https://img.shields.io/badge/.NET-512BD4?style=flat&logo=dotnet&logoColor=white)
[![License](https://img.shields.io/badge/License-BSD_3--Clause-blue.svg)](https://opensource.org/licenses/BSD-3-Clause)
[![GitHub release](https://img.shields.io/github/v/release/synesissoftware/Quench.NET.svg)](https://github.com/synesissoftware/Quench.NET/releases/latest)
[![Last Commit](https://img.shields.io/github/last-commit/synesissoftware/Quench.NET)](https://github.com/synesissoftware/Quench.NET/commits/master)
[![CI](https://github.com/synesissoftware/Quench.NET/actions/workflows/ci.yml/badge.svg)](https://github.com/synesissoftware/Quench.NET/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Quench.svg)](https://www.nuget.org/packages/Quench/)
![TFM](https://img.shields.io/badge/TFM-net8.0%20%7C%20netstandard2.0-lightgrey)


## Table of Contents <!-- omit in toc -->

- [Introduction](#introduction)
- [Installation](#installation)
- [Quick start](#quick-start)
- [Configuration](#configuration)
- [Components](#components)
- [Platform support](#platform-support)
- [Repository layout](#repository-layout)
- [Building from source](#building-from-source)
- [Examples](#examples)
- [Project Information](#project-information)
  - [Where to get help](#where-to-get-help)
  - [Contribution guidelines](#contribution-guidelines)
  - [Dependencies](#dependencies)
    - [Efferent (fan-out)](#efferent-fan-out)
    - [Development Dependencies](#development-dependencies)
    - [Afferent (fan-in)](#afferent-fan-in)
  - [Related projects](#related-projects)
  - [License](#license)


## Introduction

**Quench** provides customisable exception-quenching utilities.
**Quench.NET** is the **.NET** implementation. This repository ships an
SDK-style multi-target library (`net8.0`, `netstandard2.0`) with CI and
NuGet packaging.

**0.1.0** is the first SDK-style port of Framework **Quench.Core** behaviour
from Hautacam tagged tree **0.1.1**. That tree’s directory label was
**0.1.1** while **AssemblyVersion** / **AssemblyFileVersion** were
**0.1.0.0**; this NuGet **VersionPrefix** is **0.1.0** by coincidence of
that assembly label, not as a patch on a prior modern release. See
[FAQ.md](./FAQ.md).

With no configuration, Quench **rethrows** (`QuenchAction.Throw`). That
safety default is preserved from the Framework **Arbitrator** static
constructor.


## Installation

```bash
dotnet add package Quench
```

See [INSTALL.md](./INSTALL.md) for source checkout restore, build, and test.


## Quick start

```csharp
using Quench;
using Quench.Deems;

try
{
    // work
}
catch (Exception x)
{
    if (CaughtException.MustBeRethrown(x))
    {
        throw;
    }
}
```

See [`samples/Quench.NET.QuickStart`](./samples/Quench.NET.QuickStart) for a
runnable example. A short index is in [EXAMPLES.md](./EXAMPLES.md).


## Configuration

**0.1.0** does not auto-load Framework **App.config**. Configure in code,
which is sufficient for all unit tests on `net8.0` / `netstandard2.0`:

```csharp
using System.IO;

using Quench;

Core.Configure(cfg =>
{
    cfg.DefaultAction = QuenchAction.Throw;
    cfg.ForException<OutOfMemoryException>(QuenchAction.Quench);
    cfg.ForException(typeof(IOException), QuenchAction.Throw)
        .ExceptWhen(typeof(MyService), QuenchAction.Quench);
});
```

Optional XML uses the 0.1.1 `<quench>` schema (program default action,
`forException`, `exceptWhen`):

```csharp
Core.ConfigureFromXml(File.ReadAllText("quench.xml"));
```

`Core.ResetConfiguration()` restores default-rethrow with no rules.

A JSON / Options adapter, an **App.config** section-handler for
.NET Framework hosts, and the incomplete **Quench.Fluent** package are
out of scope for **0.1.0** (see [TODO.md](./TODO.md)).


## Components

* **`LibraryVersion`** — Major, Minor, Patch, and VersionString;
* **`Core`** — `MustBeRethrown`, `MayBeQuenched`, `IsPreciselySpecified`,
  `Configure`, `ConfigureFromXml`, `ResetConfiguration`,
  `SetProcessGlobalLogger`;
* **`Quench.Deems.CaughtException`** — fluent-reading façade over **Core**;
* **`Quench.Extensions.ExceptionExtensions`** — `MustBeRethrown` /
  `MayBeQuenched` on **Exception**;
* **`QuenchAction`** — `Throw` (default) or `Quench`;
* **`Quench.Diagnostics.ISimpleLogger`** — in-box logger (no Diagnosticism
  or Microsoft.Extensions.Logging dependency);


## Platform support

The library multi-targets:

| Target | Rationale |
| --- | --- |
| `net8.0` | Modern .NET runtime / AOT-friendly surface |
| `netstandard2.0` | Broad consumer reach (.NET Framework and older runtimes) |


## Repository layout

| Path | Purpose |
| --- | --- |
| `src/Quench.NET/` | Main library (published to NuGet as `Quench`) |
| `tests/Quench.NET.Tests/` | Unit tests |
| `samples/Quench.NET.QuickStart/` | Minimal consumer example |


## Building from source

Requires the [.NET SDK](https://dotnet.microsoft.com/download) version
specified in [`global.json`](./global.json).

```bash
./build.sh        # Linux / macOS
./build.ps1       # Windows PowerShell
```

Or:

```bash
dotnet restore Quench.NET.sln
dotnet build Quench.NET.sln --configuration Release
dotnet test Quench.NET.sln --configuration Release
dotnet pack src/Quench.NET/Quench.NET.csproj --configuration Release --output artifacts/packages
```


## Examples

See [EXAMPLES.md](./EXAMPLES.md).


## Project Information


### Where to get help

[GitHub Issues](https://github.com/synesissoftware/Quench.NET/issues)


### Contribution guidelines

Defect reports, feature requests, and pull requests are welcome. See
[CONTRIBUTING.md](./CONTRIBUTING.md).


### Dependencies


#### Efferent (fan-out)

None.


#### Development Dependencies

* [**coverlet.collector**](https://github.com/coverlet-coverage/coverlet);
* [**Microsoft.CodeAnalysis.NetAnalyzers**](https://github.com/dotnet/roslyn-analyzers);
* [**Microsoft.NET.Test.Sdk**](https://github.com/microsoft/vstest);
* [**Microsoft.SourceLink.GitHub**](https://github.com/dotnet/sourcelink);
* [**xunit**](https://github.com/xunit/xunit);
* [**xunit.runner.visualstudio**](https://github.com/xunit/visualstudio.xunit);


#### Afferent (fan-in)

None (currently).


### Related projects

* [**Quench.Python**](https://github.com/synesissoftware/Quench.Python/);
* [**Quench.Ruby**](https://github.com/synesissoftware/Quench.Ruby/);

**.NET 0.1.0** is the reference API for seeding those language ports.


### License

**Quench.NET** is released under the 3-clause BSD license. See
[LICENSE](./LICENSE) for details.


<!-- ########################### end of file ########################### -->
