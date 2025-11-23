using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Shimakaze;

internal sealed class TString(nint handle) : SafeHandle(handle, true)
{
#if NETFRAMEWORK
    private static Encoding Encoding => field ??= Encoding.Unicode;
#else
    private static Encoding Encoding => field ??= RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
        ? Encoding.Unicode
        : Encoding.UTF8;
#endif

    public override bool IsInvalid => handle is (nint)0;

    protected override unsafe bool ReleaseHandle()
    {
        NativeMemory.Free((void*)handle);
        return true;
    }

    public static unsafe implicit operator TString(string? str)
    {
        if (str is null)
            return new(0);

        unchecked
        {
            var size = Encoding.GetByteCount(str) + 1;
            if (!Encoding.IsSingleByte)
                size++;

            byte* ptr = (byte*)NativeMemory.Alloc((nuint)size, sizeof(byte));

            Encoding.GetBytes(str, new Span<byte>(ptr, size));

            ptr[size - 1] = 0;
            if (!Encoding.IsSingleByte)
                ptr[size - 2] = 0;

            return new((nint)ptr);
        }
    }

    public static unsafe implicit operator byte*(TString? s) => s is null ? null : (byte*)s.handle;
}

#if !NET7_0_OR_GREATER
file static class NativeMemory
{
    public static unsafe void Free(void* ptr) => Marshal.FreeHGlobal((nint)ptr);
    public static unsafe void* Alloc(nuint elementCount, nuint elementSize) => (void*)Marshal.AllocHGlobal(unchecked((int)(elementCount * elementSize)));
}
#endif