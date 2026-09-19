# Quench.NET - FAQ <!-- omit in toc -->

The FAQ list is under (constant) development. If you post a question on the
[Issues](https://github.com/synesissoftware/Quench.NET/issues) forum
it will be used to create one.


## Table of Contents <!-- omit in toc -->

- [Q1: "How do I install this library?"](#q1-how-do-i-install-this-library)
- [Q2: "Why is NuGet 0.1.0 related to Framework 0.1 / 0.1.1?"](#q2-why-is-nuget-010-related-to-framework-01--011)
- [Q3: "How do I configure quench rules on net8.0?"](#q3-how-do-i-configure-quench-rules-on-net80)


# FAQs: <!-- omit in toc -->


## Q1: "How do I install this library?"

Install via **NuGet**:

```bash
dotnet add package Quench
```

See [INSTALL.md](./INSTALL.md) and [README.md](./README.md) for usage.


## Q2: "Why is NuGet 0.1.0 related to Framework 0.1 / 0.1.1?"

Framework-era **Quench.Core** (Hautacam / VS2010, **.NET Framework 4.0**)
used directory labels **0.1** and **0.1.1**, while **AssemblyVersion** /
**AssemblyFileVersion** were always **0.1.0.0**. There was no modern
**VersionPrefix**. NuGet **0.0.1** was the SDK-style packaging skeleton.
**0.1.0** is the first SDK-style port of that **0.1.1** Core behaviour. The
match with historical **0.1.0.0** is coincidental: this is not a patch
release claiming continuity with the old directory label **0.1.1**.


## Q3: "How do I configure quench rules on net8.0?"

Call **Core.Configure** (code) or **Core.ConfigureFromXml** (the Framework
`<quench>` XML schema). With no configuration, exceptions must be rethrown.
**App.config** auto-load is not enabled in **0.1.0**; a JSON / Options
adapter is listed in [TODO.md](./TODO.md).


<!-- ########################### end of file ########################### -->
