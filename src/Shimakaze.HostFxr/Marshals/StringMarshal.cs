using System;
using System.Runtime.InteropServices;
using System.Text;

using Shimakaze.Native;

namespace Shimakaze.Marshals;

internal static class StringMarshal
{
    public static unsafe IDisposable Alloc(int bufferSize, out char_t* ptr)
    {
        int elementSize = OS.IsWindows ? 2 : 1;
        NativeMemoryHandle handle = new(bufferSize, elementSize);
        ptr = (char_t*)handle.Handle;
        return handle;
    }

    public static unsafe IDisposable Alloc(int count, int bufferSize, out char_t** ptr)
    {
        IDisposable[] handles = NativeMemoryHandle.Alloc<IDisposable>(count);
        NativeMemoryHandle handle = new(count, sizeof(char_t*));
        ptr = (char_t**)handle.Handle;

        for (int i = 0; i < count; i++)
            handles[i] = Alloc(bufferSize, out ptr[i]);

        return new CombineDisposable([.. handles, handle]);
    }

    public static unsafe IDisposable Fixed(string? str, out char_t* ptr)
    {
        if (str is null)
        {
            ptr = null;
            return CombineDisposable.Default;
        }

        Encoding encoding = OS.IsWindows ? Encoding.Unicode : Encoding.UTF8;
        var size = encoding.GetByteCount(str);
        size += (encoding == Encoding.Unicode ? 2 : 1);
        NativeMemoryHandle handle = new(size, sizeof(char_t));
        ptr = (char_t*)handle.Handle;

#if NET || NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        encoding.GetBytes(str, handle.AsSpan<byte>());
#elif NETSTANDARD1_3_OR_GREATER || NET20_OR_GREATER || NETCOREAPP
        fixed (char* sz = str)
            encoding.GetBytes(sz, str.Length, (byte*)ptr, size);
#else
        {
            var tmparr = encoding.GetBytes(str);
            Marshal.Copy(tmparr, 0, (nint)ptr, tmparr.Length);
        }
#endif

        // Keep end with 0
        ptr[size - 1] = default;
        if (encoding == Encoding.Unicode)
            ptr[size - 2] = default;

        return handle;
    }
    public static unsafe IDisposable Fixed(string?[]? arr, out char_t** ptr)
    {
        if (arr is null)
        {
            ptr = null;
            return CombineDisposable.Default;
        }

        IDisposable[] handles = NativeMemoryHandle.Alloc<IDisposable>(arr.Length);
        NativeMemoryHandle handle = new(arr.Length, sizeof(char_t*));
        ptr = (char_t**)handle.Handle;

        for (int i = 0; i < arr.Length; i++)
            handles[i] = Fixed(arr[i], out ptr[i]);

        return new CombineDisposable([.. handles, handle]);
    }

    public static unsafe string? From(char_t* buffer, int maxSize)
    {
        if (buffer is null)
            return null;

        if (OS.IsWindows)
        {
            var ptr = (char*)buffer;
            int i = 0;
            for (; i < maxSize; i++)
            {
                if (ptr[i] is '\0')
                    break;
            }

            return new(ptr, 0, i);
        }
        else
        {
            var ptr = (byte*)buffer;
            int i = 0;
            for (; i < maxSize; i++)
            {
                if (ptr[i] is 0)
                    break;
            }
#if NET46_OR_GREATER || NETSTANDARD1_3_OR_GREATER || NET || NETCOREAPP
            return Encoding.UTF8.GetString(ptr, i);
#elif NET20_OR_GREATER
            var size = Encoding.UTF8.GetCharCount(ptr, i);
            char* tmp = stackalloc char[size];
            Encoding.UTF8.GetChars(ptr, i, tmp, size);
            return new(tmp, 0, size);
#else
            var arr = new byte[i];
            Marshal.Copy((nint)ptr, arr, 0, i);
            return Encoding.UTF8.GetString(arr, 0, i);
#endif
        }
    }
    public static unsafe string?[] From(char_t** buffer, int count, int maxSize)
    {
        var arr = new string?[count];
        for (int i = 0; i < arr.Length; i++)
            arr[i] = From(buffer[i], maxSize);

        return arr;
    }
}
