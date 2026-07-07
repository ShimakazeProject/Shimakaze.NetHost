using Shimakaze.Marshals;

#pragma warning disable IDE0130
namespace System.Runtime.InteropServices;
#pragma warning restore IDE0130

internal static class NativeLibrary
{
    public static nint Load(string libraryPath)
    {
        if (OS.IsWindows)
            return LoadLibrary(libraryPath);
        else
            return dlopen(libraryPath, 2);

        [DllImport("kernel32", SetLastError = true)]
        static extern nint LoadLibrary(string lpFileName);
        [DllImport("libdl", SetLastError = true)]
        static extern nint dlopen(string fileName, int flags);
    }

    public static void Free(nint handle)
    {
        if (OS.IsWindows)
            FreeLibrary(handle);
        else
            dlclose(handle);

        [DllImport("kernel32", SetLastError = true)]
        static extern bool FreeLibrary(nint hModule);
        [DllImport("libdl", SetLastError = true)]
        static extern int dlclose(nint handle);
    }

    public static nint GetExport(nint handle, string name)
    {
#if NET471_OR_GREATER || NETCOREAPP || NETSTANDARD
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
#else
        if (Environment.OSVersion.Platform is PlatformID.Win32NT)
#endif
            return GetProcAddress(handle, name);
        else
            return dlsym(handle, name);

        [DllImport("kernel32", SetLastError = true)]
        static extern nint GetProcAddress(nint hModule, string lpProcName);
        [DllImport("libdl", SetLastError = true)]
        static extern nint dlsym(nint handle, string symbol);
    }
}
