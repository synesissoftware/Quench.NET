# Quench.NET - TODO <!-- omit in toc -->


## Table of Contents <!-- omit in toc -->

- [Functional improvements](#functional-improvements)
- [Performance improvements](#performance-improvements)
- [Packaging improvements](#packaging-improvements)


## Functional improvements

* [x] ~~~port Framework **Quench.Core** (**0.1.1**) to modern SDK-style **0.1.0**~~~ - ✅;
* [x] ~~~programmatic configuration sufficient to run unit tests without **App.config**~~~ - ✅;
* [ ] optional **App.config** / **IConfigurationSectionHandler** adapter for .NET Framework hosts;
* [ ] JSON / Options configuration adapter for **net8.0**;
* [ ] complete the Fluent API (Framework-era Fluent was incomplete);
* [ ] optional **ISimpleLogger** adapter to Microsoft.Extensions.Logging or Diagnosticism;


## Performance improvements

* \<none


## Packaging improvements

* [x] ~~~local restore / build / test / pack smoke (`./build.sh` → **Quench.0.0.1.nupkg**)~~~ - ✅;
* [x] ~~~local restore / build / test / pack smoke (`./build.sh` → **Quench.0.1.0.nupkg**)~~~ - ✅;
* [ ] first NuGet.org publish (**0.1.0** via GitHub Release / **release.yml**);
* [ ] remove local **_legacy/** tree once operators no longer need the Framework snapshot;


<!-- ########################### end of file ########################### -->
