using System;
using System.Runtime.InteropServices;

namespace Shimakaze.Marshals;

internal static class OS
{
#if NET
    public static bool IsWindows => OperatingSystem.IsWindows();
#elif NET471_OR_GREATER || NETSTANDARD || NETCOREAPP
    public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#else
    public static bool IsWindows => Environment.OSVersion.Platform is PlatformID.Win32S or PlatformID.Win32Windows or PlatformID.Win32NT or PlatformID.WinCE;
#endif
}
