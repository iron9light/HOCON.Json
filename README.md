# HOCON.Json
Convert Lightbend's HOCON (Human-Optimized Object Configuration Notation) to Json

[![Build Status](https://iron9light.visualstudio.com/github/_apis/build/status/iron9light.HOCON.Json?branchName=master)](https://iron9light.visualstudio.com/github/_build/latest?definitionId=1&branchName=master)
[![NuGet](https://img.shields.io/nuget/v/Hocon.Json.svg)](https://www.nuget.org/packages/Hocon.Json/)

```csharp
var hoconRoot = Parser.Parse(hoconString);
var jToken = hoconRoot.ToJToken(); // Newtonsoft.Json.Linq.JToken object
```
