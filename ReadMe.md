# Shimakaze.NetHost

Shimakaze.NetHost is a set of managed wrapper libraries for the .NET Hosting API, enabling you to safely call the native `nethost` and `hostfxr` interfaces from C#. It lets you start and control the .NET runtime from native code, plugin systems, script hosts, or NativeAOT applications.

[![Build](https://github.com/ShimakazeProject/Shimakaze.NetHost/actions/workflows/build.yaml/badge.svg)](https://github.com/ShimakazeProject/Shimakaze.NetHost/actions/workflows/build.yaml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow)](./LICENSE)
[![NuGet Shimakaze.NetHost](https://img.shields.io/nuget/v/Shimakaze.NetHost?label=Shimakaze.NetHost)![NuGet Shimakaze.NetHost Downloads](https://img.shields.io/nuget/dt/Shimakaze.NetHost)](https://www.nuget.org/packages/Shimakaze.NetHost)
[![NuGet Shimakaze.HostFxr](https://img.shields.io/nuget/v/Shimakaze.HostFxr?label=Shimakaze.HostFxr)![NuGet Shimakaze.HostFxr Downloads](https://img.shields.io/nuget/dt/Shimakaze.HostFxr)](https://www.nuget.org/packages/Shimakaze.HostFxr)

---

## Table of Contents

- [Project Overview](#project-overview)
- [Package Structure](#package-structure)
- [Features](#features)
- [Target Frameworks](#target-frameworks)
- [Quick Start](#quick-start)
- [API Overview](#api-overview)
- [NativeAOT Support](#nativeaot-support)
- [Build and Package](#build-and-package)
- [References](#references)
- [License](#license)

---

## Project Overview

The .NET Hosting API allows native code to locate, load, and start the .NET runtime. It is commonly used for:

- Embedding the .NET runtime into a C/C++ native host application.
- Writing plugin hosts that can load managed assemblies.
- Invoking managed code from a NativeAOT application via `hostfxr`.

The [.NET official documentation](https://learn.microsoft.com/dotnet/core/tutorials/netcore-hosting) provides C/C++ examples. **Shimakaze.NetHost** wraps these native interfaces into an easy-to-use C# API and includes the native object files required for NativeAOT static linking, saving you from manually configuring P/Invoke and MSBuild.

---

## Package Structure

| Package | Path | Primary Responsibility |
|---------|------|------------------------|
| `Shimakaze.NetHost` | `src/Shimakaze.NetHost` | Wraps `nethost`'s `get_hostfxr_path` to locate the `hostfxr` library. Includes `NetHost.targets` to automatically resolve NativeAOT static-linking assets. |
| `Shimakaze.HostFxr` | `src/Shimakaze.HostFxr` | Wraps all core `hostfxr` functions: runtime initialization, assembly loading, delegate retrieval, runtime property access, environment queries, and more. |

---

## Features

- **Cross-platform**: Automatically detects Windows, Linux, and macOS, handling Unicode (Windows) and UTF-8 (Unix) string encodings.
- **Broad target framework support**: Compatible from `.NET 10` down to `.NET 2.0` / `.NET Standard 1.1`.
- **NativeAOT compatible**: Targets for .NET 8.0+ are marked as `IsAotCompatible`.
- **Automatic native asset resolution**: `NetHost.targets` automatically locates `libnethost` static and dynamic libraries and statically links them when publishing NativeAOT.
- **Safe wrapper**: Uses `IDisposable`; unsafe pointers are kept internal, exposing only managed strings and arrays to consumers.
- **Zero runtime dependencies**: The wrapper libraries themselves do not depend on any additional NuGet packages.

---

## Target Frameworks

Specified centrally in `Directory.Build.props`:

- `.NET 10` / `.NET 9` / `.NET 8` / `.NET 7` / `.NET 6` / `.NET 5`
- `.NET Core 3.1` / `3.0` / `2.2` / `2.1` / `2.0` / `1.1` / `1.0`
- `.NET Framework 4.8.1` down to `4.0`, plus `3.5` / `2.0`
- `.NET Standard 2.1` / `2.0` / `1.6` / `1.5` / `1.4` / `1.3` / `1.2` / `1.1`

---

## Quick Start

### 1. Install the NuGet Packages

```bash
dotnet add package Shimakaze.NetHost
dotnet add package Shimakaze.HostFxr
```

### 2. Locate hostfxr and Load It

```csharp
using Shimakaze;

// Get the path to the hostfxr library (provided by nethost)
string hostfxrPath = NetHost.GetHostFxrPath();

// Load hostfxr
using var hostfxr = new HostFXR(hostfxrPath);
```

### 3. Initialize the Runtime and Load an Assembly

```csharp
// Initialize the runtime via runtimeconfig.json
var parameters = new InitializeParameters
{
    HostPath = "MyApp.exe",
    DotnetRoot = @"C:\Program Files\dotnet"
};

hostfxr.InitializeForRuntimeConfig(
    "MyApp.runtimeconfig.json",
    parameters,
    out HostFXRHandle context);

// Load an assembly and retrieve the entry-point delegate
context.LoadAssemblyAndGetFunctionPointer(
    "MyAssembly.dll",
    "MyNamespace.MyClass",
    "MyMethod",
    null,
    out nint delegatePtr);

// Close the context
context.Close();
```

### 4. Run a .NET Application

```csharp
hostfxr.InitializeForDotnetCommandLine(
    new[] { "dotnet", "MyApp.dll" },
    parameters,
    out HostFXRHandle context);

int exitCode = context.RunApp();
context.Close();
```

---

## API Overview

### `NetHost`

| Method | Description |
|--------|-------------|
| `NetHost.GetHostFxrPath()` | Calls `nethost.get_hostfxr_path` and returns the full path to the `hostfxr` library. |

### `HostFXR`

| Method | Description |
|--------|-------------|
| `Main` / `MainStartupinfo` / `MainBundleStartupinfo` | Directly invoke the `hostfxr_main` family of entry points. |
| `InitializeForDotnetCommandLine` | Parse command-line arguments and initialize a runtime context. |
| `InitializeForRuntimeConfig` | Initialize a runtime context via `runtimeconfig.json`. |
| `SetErrorWriter` / `GetDotnetEnvironmentInfo` | Set an error writer / enumerate installed SDKs and frameworks. |
| `GetRuntimePropertyValue` / `SetRuntimePropertyValue` / `GetRuntimeProperties` | Read and write runtime properties. |
| `RunApp` | Run the .NET application associated with the current context. |
| `Close` | Close the `hostfxr` context. |
| `Dispose` | Release the `hostfxr` library handle. |

### `HostFXRHandle`

| Method | Description |
|--------|-------------|
| `LoadAssemblyAndGetFunctionPointer` | Load an assembly and retrieve a method delegate for a type. |
| `GetFunctionPointer` | Retrieve a method pointer for a type in a loaded assembly. |
| `LoadAssembly` | Load an assembly into the runtime context. |
| `LoadAssemblyBytes` | Load an assembly from in-memory bytes (optional PDB). |
| `GetRuntimePropertyValue` / `SetRuntimePropertyValue` / `GetRuntimeProperties` | Read and write context properties. |
| `RunApp` | Run the context application. |
| `Close` / `Dispose` | Close the context. |

### `DelegateType`

Corresponds to `hostfxr_delegate_type`, including:

- `LoadAssemblyAndGetFunctionPointer`
- `GetFunctionPointer`
- `LoadAssembly`
- `LoadAssemblyBytes`
- `ComActivation`
- `WinrtActivation`
- `ComRegister` / `ComUnregister`
- `LoadInMemoryAssembly`

---

## NativeAOT Support

`Shimakaze.NetHost` ships with `NetHost.targets` in the NuGet package, which automatically:

1. Resolves the current Runtime Identifier (`RuntimeIdentifier` / `DefaultAppHostRuntimeIdentifier` / `NETCoreSdkRuntimeIdentifier`).
2. Locates `Microsoft.NETCore.App.Host.<RID>` from the SDK targeting pack or the NuGet global packages directory.
3. Passes `libnethost.a` / `libnethost.lib` as `NativeLibrary` to the NativeAOT linker.
4. Declares `DirectPInvoke` for `nethost` so that P/Invoke can be statically resolved.
5. Appends the `-lstdc++` linker argument on Linux and macOS.
6. Copies the dynamic library `nethost.dll` / `libnethost.so` / `libnethost.dylib` to the output directory for non-AOT runtime use.

> When using `Shimakaze.NetHost`, you do not need to manually download or configure `nethost` native libraries.

---

## Build and Package

This project uses .NET SDK 10 + GitVersion.MsBuild 6.x for versioning, with Central Package Management enabled.

```bash
# Restore
dotnet restore --graph --artifacts-path artifacts

# Build Release
dotnet build --graph --artifacts-path artifacts --configuration Release --no-restore

# Pack (includes symbols package)
dotnet pack --graph --artifacts-path artifacts --configuration Release --no-restore --no-build --include-symbols
```

The CI/CD configuration is located at `.github/workflows/build.yaml`:

- Builds are triggered on push and pull_request to the `master` branch.
- GitHub Releases are created and NuGet packages are pushed on `v*` tags.

---

## References

- [AustinWise/DotNetHostingAndNativeAot](https://github.com/AustinWise/DotNetHostingAndNativeAot)

## Related Documentation

- [Write a custom .NET host to control the .NET runtime from your native code](https://learn.microsoft.com/dotnet/core/tutorials/netcore-hosting)
- [Native code interop with Native AOT](https://learn.microsoft.com/dotnet/core/deploying/native-aot/interop)

---

## License

This project is licensed under the [MIT License](./LICENSE).

Copyright © 2025 frg2089 <frg2089@outlook.com>
