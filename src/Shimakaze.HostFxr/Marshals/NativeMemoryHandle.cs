using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Shimakaze.Marshals;

internal sealed class NativeMemoryHandle(int count, int size) : IDisposable
{
    private bool _disposedValue;
#if NET6_0_OR_GREATER
    private readonly unsafe void* _ptr = NativeMemory.Alloc((nuint)count, (nuint)size);
    public unsafe nint Handle => (nint)_ptr;
#else
    public nint Handle { get; } = Marshal.AllocHGlobal(size * count);
#endif

#if NET || NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public unsafe Span<T> AsSpan<T>() => new((void*)Handle, size * count);
#endif

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP || NET
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static T[] Alloc<T>(int length)
    {
#if NET5_0_OR_GREATER
        return GC.AllocateUninitializedArray<T>(length);
#else
        return new T[length];
#endif
    }

    private unsafe void DisposeCore()
    {
        if (_disposedValue)
            return;

#if NET6_0_OR_GREATER
        NativeMemory.Free(_ptr);
#else
        Marshal.FreeHGlobal(Handle);
#endif
        _disposedValue = true;
    }

    ~NativeMemoryHandle()
    {
        DisposeCore();
    }

    public void Dispose()
    {
        DisposeCore();
        GC.SuppressFinalize(this);
    }
}
