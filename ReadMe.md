# Shimakaze.NetHost

Shimakaze.NetHost 是一组用于 .NET Hosting API 的托管封装库，让你能够用 C# 安全地调用 `nethost` 与 `hostfxr` 原生接口，从而在原生代码、插件系统、脚本宿主或 NativeAOT 应用中启动并控制 .NET 运行时。

[![Build](https://github.com/ShimakazeProject/Shimakaze.NetHost/actions/workflows/build.yaml/badge.svg)](https://github.com/ShimakazeProject/Shimakaze.NetHost/actions/workflows/build.yaml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow)](./LICENSE)
[![NuGet Shimakaze.NetHost](https://img.shields.io/nuget/v/Shimakaze.NetHost?label=Shimakaze.NetHost)![NuGet Shimakaze.NetHost Downloads](https://img.shields.io/nuget/dt/Shimakaze.NetHost)](https://www.nuget.org/packages/Shimakaze.NetHost)
[![NuGet Shimakaze.HostFxr](https://img.shields.io/nuget/v/Shimakaze.HostFxr?label=Shimakaze.HostFxr)![NuGet Shimakaze.HostFxr Downloads](https://img.shields.io/nuget/dt/Shimakaze.HostFxr)](https://www.nuget.org/packages/Shimakaze.HostFxr)

---

## 目录

- [项目概述](#项目概述)
- [包结构](#包结构)
- [功能特性](#功能特性)
- [目标框架](#目标框架)
- [快速开始](#快速开始)
- [API 概览](#api-概览)
- [NativeAOT 支持](#nativeaot-支持)
- [构建与打包](#构建与打包)
- [参考项目](#参考项目)
- [许可证](#许可证)

---

## 项目概述

.NET Hosting API 允许原生代码定位、加载并启动 .NET 运行时，常用于：

- 把 .NET 运行时嵌入 C/C++ 原生宿主程序；
- 编写可加载托管程序集的插件宿主；
- 在 NativeAOT 应用中通过 `hostfxr` 调用托管代码。

[.NET 官方文档](https://learn.microsoft.com/dotnet/core/tutorials/netcore-hosting) 给出了 C/C++ 示例，而 **Shimakaze.NetHost** 将这些原生接口包装成易用的 C# API，并内置了 NativeAOT 静态链接所需的目标文件，省去手工配置 P/Invoke 与 MSBuild 的繁琐步骤。

---

## 包结构

| 包名 | 路径 | 主要职责 |
|------|------|---------|
| `Shimakaze.NetHost` | `src/Shimakaze.NetHost` | 封装 `nethost` 的 `get_hostfxr_path`，用于定位 `hostfxr` 库。附带 `NetHost.targets`，自动解析 NativeAOT 静态链接资产。 |
| `Shimakaze.HostFxr` | `src/Shimakaze.HostFxr` | 封装 `hostfxr` 全部核心函数：运行时初始化、加载程序集、获取委托、读取运行时属性、环境信息查询等。 |

---

## 功能特性

- **跨平台**：自动识别 Windows / Linux / macOS，处理 Unicode（Windows）与 UTF-8（Unix）字符串编码。
- **极广的目标框架支持**：从 `.NET 10` 一直向下兼容到 `.NET 2.0` / `.NET Standard 1.1`。
- **NativeAOT 兼容**：.NET 8.0+ 目标标记为 `IsAotCompatible`。
- **自动原生资产解析**：`NetHost.targets` 自动查找 `libnethost` 静态库与动态库，并在发布 NativeAOT 时静态链接。
- **安全封装**：使用 `IDisposable`、不安全指针仅在内部使用，对外暴露托管字符串与数组。
- **零运行时依赖**：封装库本身不依赖任何额外 NuGet 包。

---

## 目标框架

由 `Directory.Build.props` 统一指定：

- `.NET 10` / `.NET 9` / `.NET 8` / `.NET 7` / `.NET 6` / `.NET 5`
- `.NET Core 3.1` / `3.0` / `2.2` / `2.1` / `2.0` / `1.1` / `1.0`
- `.NET Framework 4.8.1` 到 `4.0`，以及 `3.5` / `2.0`
- `.NET Standard 2.1` / `2.0` / `1.6` / `1.5` / `1.4` / `1.3` / `1.2` / `1.1`

---

## 快速开始

### 1. 安装 NuGet 包

```bash
dotnet add package Shimakaze.NetHost
dotnet add package Shimakaze.HostFxr
```

### 2. 定位 hostfxr 并加载

```csharp
using Shimakaze;

// 获取 hostfxr 库路径（nethost 提供）
string hostfxrPath = NetHost.GetHostFxrPath();

// 加载 hostfxr
using var hostfxr = new HostFXR(hostfxrPath);
```

### 3. 初始化运行时并加载程序集

```csharp
// 通过 runtimeconfig.json 初始化运行时
var parameters = new InitializeParameters
{
    HostPath = "MyApp.exe",
    DotnetRoot = @"C:\Program Files\dotnet"
};

hostfxr.InitializeForRuntimeConfig(
    "MyApp.runtimeconfig.json",
    parameters,
    out HostFXRHandle context);

// 加载程序集并获取入口委托
context.LoadAssemblyAndGetFunctionPointer(
    "MyAssembly.dll",
    "MyNamespace.MyClass",
    "MyMethod",
    null,
    out nint delegatePtr);

// 关闭上下文
context.Close();
```

### 4. 运行 .NET 应用程序

```csharp
hostfxr.InitializeForDotnetCommandLine(
    new[] { "dotnet", "MyApp.dll" },
    parameters,
    out HostFXRHandle context);

int exitCode = context.RunApp();
context.Close();
```

---

## API 概览

### `NetHost`

| 方法 | 说明 |
|------|------|
| `NetHost.GetHostFxrPath()` | 调用 `nethost.get_hostfxr_path`，返回 `hostfxr` 库完整路径。 |

### `HostFXR`

| 方法 | 说明 |
|------|------|
| `Main` / `MainStartupinfo` / `MainBundleStartupinfo` | 直接启动 `hostfxr_main` 系列入口。 |
| `InitializeForDotnetCommandLine` | 解析命令行参数并初始化运行时上下文。 |
| `InitializeForRuntimeConfig` | 通过 `runtimeconfig.json` 初始化运行时上下文。 |
| `SetErrorWriter` / `GetDotnetEnvironmentInfo` | 设置错误输出器 / 枚举已安装的 SDK 与框架。 |
| `GetRuntimePropertyValue` / `SetRuntimePropertyValue` / `GetRuntimeProperties` | 读写运行时属性。 |
| `RunApp` | 执行当前上下文对应的 .NET 应用程序。 |
| `Close` | 关闭 `hostfxr` 上下文。 |
| `Dispose` | 释放 `hostfxr` 库句柄。 |

### `HostFXRHandle`

| 方法 | 说明 |
|------|------|
| `LoadAssemblyAndGetFunctionPointer` | 加载程序集并获取类型方法委托。 |
| `GetFunctionPointer` | 获取已加载程序集中类型的方法指针。 |
| `LoadAssembly` | 加载程序集到运行时上下文。 |
| `LoadAssemblyBytes` | 从内存字节加载程序集（可选 PDB）。 |
| `GetRuntimePropertyValue` / `SetRuntimePropertyValue` / `GetRuntimeProperties` | 上下文属性读写。 |
| `RunApp` | 运行上下文应用。 |
| `Close` / `Dispose` | 关闭上下文。 |

### `DelegateType`

对应 `hostfxr_delegate_type`，包括：

- `LoadAssemblyAndGetFunctionPointer`
- `GetFunctionPointer`
- `LoadAssembly`
- `LoadAssemblyBytes`
- `ComActivation`
- `WinrtActivation`
- `ComRegister` / `ComUnregister`
- `LoadInMemoryAssembly`

---

## NativeAOT 支持

`Shimakaze.NetHost` 随 NuGet 包附带 `NetHost.targets`，它会自动：

1. 解析当前 Runtime Identifier（`RuntimeIdentifier` / `DefaultAppHostRuntimeIdentifier` / `NETCoreSdkRuntimeIdentifier`）。
2. 从 SDK targeting pack 或 NuGet 全局包目录定位 `Microsoft.NETCore.App.Host.<RID>`。
3. 将 `libnethost.a` / `libnethost.lib` 作为 `NativeLibrary` 传入 NativeAOT 链接器。
4. 声明 `DirectPInvoke` 为 `nethost`，确保 P/Invoke 可被静态解析。
5. 在 Linux / macOS 上附加 `-lstdc++` 链接参数。
6. 把动态库 `nethost.dll` / `libnethost.so` / `libnethost.dylib` 复制到输出目录，以便非 AOT 运行时使用。

> 使用 `Shimakaze.NetHost` 时，无需手动下载或配置 `nethost` 原生库。

---

## 构建与打包

本项目使用 .NET SDK 10 + GitVersion.MsBuild 6.x 进行版本管理，Central Package Management 已启用。

```bash
# 还原
dotnet restore --graph --artifacts-path artifacts

# 构建 Release
dotnet build --graph --artifacts-path artifacts --configuration Release --no-restore

# 打包（包含符号包）
dotnet pack --graph --artifacts-path artifacts --configuration Release --no-restore --no-build --include-symbols
```

CI/CD 配置位于 `.github/workflows/build.yaml`：

- 在 `master` 分支的 push / pull_request 时触发构建；
- 在 `v*` 标签时发布 GitHub Release 并推送 NuGet 包。

---

## 参考项目

- [AustinWise/DotNetHostingAndNativeAot](https://github.com/AustinWise/DotNetHostingAndNativeAot)。

## 相关文档

- [Write a custom .NET host to control the .NET runtime from your native code](https://learn.microsoft.com/dotnet/core/tutorials/netcore-hosting)
- [Native code interop with Native AOT](https://learn.microsoft.com/dotnet/core/deploying/native-aot/interop)

---

## 许可证

本项目采用 [MIT License](./LICENSE) 授权。

Copyright © 2025 frg2089 <frg2089@outlook.com>
