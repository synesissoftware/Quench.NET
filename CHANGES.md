# Quench.NET - Changes <!-- omit in toc -->


## 0.1.0 - 19th September 2026

* first SDK-style port of Framework **Quench.Core** behaviour from Hautacam tagged tree **0.1.1** (directory label **0.1.1**; historical **AssemblyVersion** **0.1.0.0** — NuGet **0.1.0** is that coincidence, not a patch on a prior modern release);
* public API: **Quench.Core** (`MustBeRethrown` / `MayBeQuenched` / `IsPreciselySpecified` / `SetProcessGlobalLogger`), **Quench.Deems.CaughtException**, **Quench.Extensions.ExceptionExtensions** (renamed from the Framework GUID type name);
* safety default preserved: no configuration ⇒ **QuenchAction.Throw** (must rethrow);
* programmatic configuration via **Core.Configure** / **ResetConfiguration**, plus optional **Core.ConfigureFromXml** for the 0.1.1 `<quench>` schema; Framework **App.config** auto-load is not enabled on `net8.0` / `netstandard2.0`;
* thin in-box **Quench.Diagnostics.ISimpleLogger** (`DebugSimpleLogger`, `NullSimpleLogger`); no Diagnosticism.NET or Microsoft.Extensions.Logging dependency;
* inheritance arbitration step 3.3 now walks parent exception types with the specific catching type, matching the 0.1.1 comments (the tagged sources reused the child exception type and skipped that match);
* **Quench.Fluent** is not shipped (Framework Fluent called an unimplemented Core registration API);


## 0.0.1 - 19th September 2026

* SDK-style cross-platform shell: multi-target library (`net8.0`, `netstandard2.0`), xUnit tests, QuickStart sample;
* Synesis markdown set (**AUTHORS.md**, **CHANGES.md**, **CONTRIBUTING.md**, **EXAMPLES.md**, **FAQ.md**, **INSTALL.md**, **NEWS.md**, **README.md**, **TODO.md**);
* GitHub Actions **ci.yml** (Ubuntu, Windows, macOS) and **release.yml** NuGet publish workflow;
* NuGet packaging id **Quench** with portable PDB symbol packages (`.snupkg`) and Source Link;
* Synesis-standard build scripts (**build.sh**, **build.ps1**) and **.sis/** project identity;
* stub public API: **Quench.LibraryVersion** (`Major` / `Minor` / `Patch` / `VersionString`);
* historical Framework **Quench.Core** trees were labelled **0.1** / **0.1.1** with assembly version **0.1.0.0** (VS2010 / .NET Framework 4.0) and are not this package;


<!-- ########################### end of file ########################### -->
