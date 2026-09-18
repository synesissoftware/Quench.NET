# Quench.NET - FAQ <!-- omit in toc -->

The FAQ list is under (constant) development. If you post a question on the
[Issues](https://github.com/synesissoftware/Quench.NET/issues) forum
it will be used to create one.


## Table of Contents <!-- omit in toc -->

- [Q1: "How do I install this library?"](#q1-how-do-i-install-this-library)
- [Q2: "Why is NuGet 0.0.1 not Framework 0.1 / 0.1.1?"](#q2-why-is-nuget-001-not-framework-01--011)


# FAQs: <!-- omit in toc -->


## Q1: "How do I install this library?"

Install via **NuGet**:

```bash
dotnet add package Quench
```

See [INSTALL.md](./INSTALL.md) and [README.md](./README.md) for usage.


## Q2: "Why is NuGet 0.0.1 not Framework 0.1 / 0.1.1?"

Framework-era **Quench.Core** (Hautacam / VS2010, **.NET Framework 4.0**)
used directory labels **0.1** and **0.1.1**, while **AssemblyVersion** /
**AssemblyFileVersion** were always **0.1.0.0**. There was no modern
**VersionPrefix**. This **0.0.1** package is the SDK-style packaging
skeleton only and does not claim behavioural parity with **0.1.1**.
Recovered Core API is planned as **0.1.0** (see [TODO.md](./TODO.md)).


<!-- ########################### end of file ########################### -->
