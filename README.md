# EtsyBDD

**Prerequisites:**
- Microsoft Visual Studio 2019
- .NET Framework 5

**Run tests**
```
dotnet test -s .\Settings\env1.runsettings
```
Passing .runsettings is mandatory 

**Generate report**
* Install SpecFlow CLI tool 
```
dotnet tool install --global SpecFlow.Plus.LivingDoc.CLI
```
* Execute 
```
livingdoc test-assembly bin\Debug\netcoreapp3.1\EtsyBDD.dll -t bin\Debug\netcoreapp3.1\TestExecution.json  --output Reports\
```
