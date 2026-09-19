# Quench.NET - Installation and Use <!-- omit in toc -->


## Table of Contents <!-- omit in toc -->

- [Install the package](#install-the-package)
- [From a source checkout](#from-a-source-checkout)
- [Using the library](#using-the-library)


## Install the package

```bash
dotnet add package Quench
```


## From a source checkout

Requires the [.NET SDK](https://dotnet.microsoft.com/download) version specified in [`global.json`](./global.json).

```bash
dotnet restore Quench.NET.sln
dotnet build Quench.NET.sln --configuration Release
dotnet test Quench.NET.sln --configuration Release
```

Or `./build.sh` (Linux / macOS) / `./build.ps1` (Windows PowerShell).


## Using the library

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

See [README.md](./README.md) for a Quick start and configuration, and
[EXAMPLES.md](./EXAMPLES.md) for the sample program.


<!-- ########################### end of file ########################### -->
