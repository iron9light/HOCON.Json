# HOCON.Json
Convert Lightbend's [HOCON](https://github.com/akkadotnet/HOCON) (Human-Optimized Object Configuration Notation) to Json

[![Build Status](https://iron9light.visualstudio.com/github/_apis/build/status/iron9light.HOCON.Json?branchName=master)](https://iron9light.visualstudio.com/github/_build/latest?definitionId=1&branchName=master)
[![NuGet](https://img.shields.io/nuget/v/Hocon.Json.svg)](https://www.nuget.org/packages/Hocon.Json/)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=iron9light_HOCON.Json&metric=ncloc)](https://sonarcloud.io/dashboard?id=iron9light_HOCON.Json)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=iron9light_HOCON.Json&metric=coverage)](https://sonarcloud.io/dashboard?id=iron9light_HOCON.Json)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=iron9light_HOCON.Json&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=iron9light_HOCON.Json)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=iron9light_HOCON.Json&metric=security_rating)](https://sonarcloud.io/dashboard?id=iron9light_HOCON.Json)

```csharp
var hoconRoot = Parser.Parse(hoconString);
var jToken = hoconRoot.ToJToken(); // Newtonsoft.Json.Linq.JToken object
```
