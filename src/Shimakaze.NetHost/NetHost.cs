using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Shimakaze;

public partial class NetHost
{
    /// <summary>
    /// Get the path to the hostfxr library
    /// </summary>
    /// <param name="buffer">
    /// Buffer that will be populated with the hostfxr path, including a null terminator.
    /// </param>
    /// <param name="buffer_size">
    /// [in] Size of buffer in char_t units. <br/>
    /// [out] Size of buffer used in char_t units. If the input value is too small
    ///       or buffer is nullptr, this is populated with the minimum required size
    ///       in char_t units for a buffer to hold the hostfxr path
    /// </param>
    /// <param name="parameters">
    /// Optional. Parameters that modify the behaviour for locating the hostfxr library.
    /// If nullptr, hostfxr is located using the environment variable or global registration
    /// </param>
    /// <returns>
    /// 0 on success, otherwise failure <br/>
    /// 0x80008098 - buffer is too small (HostApiBufferTooSmall)
    /// </returns>
    /// <remarks>
    /// The full search for the hostfxr library is done on every call. To minimize the need
    /// to call this function multiple times, pass a large buffer (e.g. PATH_MAX).
    /// </remarks>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
    private static unsafe int GetHostFxrPath(Span<byte> buffer, ref nint buffer_size, nint parameters)
#else
    private static unsafe int GetHostFxrPath(byte[]? buffer, ref nint buffer_size, nint parameters)
#endif
    {
        fixed (nint* buffer_size_native = &buffer_size)
        fixed (byte* buffer_native = buffer)
            return __PInvoke(buffer_native, buffer_size_native, parameters);

        [DllImport("nethost", EntryPoint = "get_hostfxr_path", ExactSpelling = true)]
        static extern int __PInvoke(byte* buffer_native, nint* buffer_size_native, nint parameters_native);
    }

    public static unsafe string GetHostFxrPath()
    {
        nint buffer_size = 0;
        var result = GetHostFxrPath(null, ref buffer_size, 0);
        Debug.Assert(result is unchecked((int)0x80008098));

        nint byteSize = buffer_size;

        Encoding encoding;
#if NET
        if (OperatingSystem.IsWindows())
#elif NET471_OR_GREATER || NETSTANDARD || NETCOREAPP
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
#else
        if (Environment.OSVersion.Platform is PlatformID.Win32S or PlatformID.Win32Windows or PlatformID.Win32NT or PlatformID.WinCE)
#endif
            encoding = Encoding.Unicode;
        else
            encoding = Encoding.UTF8;

        if (encoding == Encoding.Unicode)
            byteSize *= 2;

        var buffer = Alloc<byte>((int)byteSize);
        result = GetHostFxrPath(buffer, ref buffer_size, 0);
        Debug.Assert(result is 0);

#if NET || NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        var path = Alloc<char>(encoding.GetCharCount(buffer));
        encoding.GetChars(buffer, path);
#elif NETSTANDARD1_3_OR_GREATER || NET20_OR_GREATER || NETCOREAPP
        var path = Alloc<char>(encoding.GetCharCount(buffer));
        fixed (byte* sz = buffer)
        fixed (char* ptr = path)
            encoding.GetChars(sz, buffer.Length, ptr, path.Length);
#else
        var path = encoding.GetChars(buffer);
#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
        return path.AsSpan().TrimEnd('\0').ToString();
#else
        for (int i = path.Length - 1; i >= 0; i--)
        {
            if (path[i] is not '\0')
                return new string(path, 0, i);
        }

        return string.Empty;
#endif
    }

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP || NET
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    private static T[] Alloc<T>(int length)
    {
#if NET5_0_OR_GREATER
        return GC.AllocateUninitializedArray<T>(length);
#else
        return new T[length];
#endif
    }
}
