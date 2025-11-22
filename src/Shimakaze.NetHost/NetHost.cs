using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Shimakaze;

internal static partial class NetHost
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
#if !NET7_0_OR_GREATER
    private static unsafe int GetHostFxrPath(Span<byte> buffer, ref nint buffer_size, nint parameters)
    {
        fixed (nint* buffer_size_native = &buffer_size)
        fixed (byte* buffer_native = buffer)
            return __PInvoke(buffer_native, buffer_size_native, parameters);

        [DllImport("nethost", EntryPoint = "get_hostfxr_path", ExactSpelling = true)]
        static extern unsafe int __PInvoke(byte* buffer_native, nint* buffer_size_native, nint parameters_native);
    }
#else
    [LibraryImport("nethost", EntryPoint = "get_hostfxr_path")]
    private static partial int GetHostFxrPath(Span<byte> buffer, ref nint buffer_size, nint parameters);
#endif

    public static string GetHostFxrPath()
    {
        nint buffer_size = 0;
        var result = GetHostFxrPath(null, ref buffer_size, 0);
        Debug.Assert(result is unchecked((int)0x80008098));

        nint byteSize = buffer_size;
        Encoding encoding = Encoding.UTF8;
#if !NETFRAMEWORK
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
#endif
        {
            byteSize *= 2;
            encoding = Encoding.Unicode;
        }

        Span<byte> buffer = Alloc<byte>(unchecked((int)byteSize));
        result = GetHostFxrPath(buffer, ref buffer_size, 0);
        Debug.Assert(result is 0);

        var charSize = encoding.GetCharCount(buffer);
        Span<char> path = Alloc<char>(charSize);
        encoding.GetChars(buffer, path);

        return path.TrimEnd('\0').ToString();
    }

    private static T[] Alloc<T>(int length)
    {
#if NET5_0_OR_GREATER
        return GC.AllocateUninitializedArray<T>(length);
#else
        return new T[length];
#endif
    }
}
