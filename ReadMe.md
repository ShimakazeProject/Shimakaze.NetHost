# Shimakaze.NetHost

```powershell
dotnet add package Shimakaze.NetHost
dotnet add package Shimakaze.HostFxr
```

```csharp
using Shimakaze;

// call nethost.dll get_hostfxr_path
var hostfxrPath = NetHost.GetHostFxrPath();

// load hostfxr.dll
HostFxr hostfxr = new(hostfxrPath);

// call hostfxr_initialize_for_dotnet_command_line
hostfxr.Initialize([@"xxx.dll"], null, out var context);

// call hostfxr_run_app
context.RunApp();
```

## References
- https://github.com/AustinWise/DotNetHostingAndNativeAot
