# BlazorCpSmxInteractions
A Razor Class Library (RCL) to interact with Check Point SmartConsole Extensions (SMX) Platform for Blazor WebAssembly Apps.

## Installation

BlazorCpSmxInteractions is available via NuGet: [![NuGet](https://img.shields.io/nuget/vpre/BlazorCpSmxInteractions.svg?label=NuGet)](https://www.nuget.org/packages/BlazorCpSmxInteractions)


## Get Started

### Prerequisites

[.NET 5.0 SDK or later](https://dotnet.microsoft.com/download/dotnet/5.0)

### Create a Blazor WebAssembly App

```sh
dotnet new blazorwasm -o SmxApplication1
```
### Change to the new project directory

```sh
cd SmxApplication1
```

### Add the BlazorCpSmxInteractions NuGet package

```sh
dotnet add package BlazorCpSmxInteractions
```

### Use Visual Studio Code for cross-platform Blazor development

```sh
code .
```

### Modify the project

Add an using statement for BlazorCpSmxInteractions in `Program.cs` and register injectable dependencies:

```csharp
...
using BlazorCpSmxInteractions;
...
builder.Services.AddSingleton<SmartConsoleInteractions>();
...
```

Add an `@using` statement in the root `_Imports.razor` file:

```csharp
...
@using BlazorCpSmxInteractions
...
```

Add the following `@inject` Razor directives in `Pages\Index.razor`

```csharp
...
@inject IJSRuntime JSRuntime;
@inject SmartConsoleInteractions SmartConsoleInteractions;
...
```

and the required SmartConsole interactions.

Add a SmartConsole extension manifest file `extension.json` in the folder `wwwroot`.


### Build and start the app

```sh
dotnet watch run
```


### Build the app for production 

```sh
dotnet publish -c Release
```


## Additional resources

- [Introduction to ASP.NET Core Blazor](https://docs.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-5.0)
- [SmartConsole Extension Developer Guide](https://sc1.checkpoint.com/documents/SmartConsole/Extensions/index.html?ref=git)
