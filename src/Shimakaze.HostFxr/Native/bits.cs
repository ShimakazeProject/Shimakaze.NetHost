using System.Runtime.InteropServices;

namespace Shimakaze.Native;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal readonly struct int32_t(int value)
{
    public readonly int Value = value;

    public static implicit operator int(int32_t v) => v.Value;
    public static implicit operator int32_t(int v) => new(v);
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal readonly struct int64_t(long value)
{
    public readonly long Value = value;

    public static implicit operator long(int64_t v) => v.Value;
    public static implicit operator int64_t(long v) => new(v);
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal readonly struct size_t(nint value)
{
    public readonly nint Value = value;

    public static implicit operator nint(size_t v) => v.Value;
    public static implicit operator size_t(nint v) => new(v);

    public static implicit operator size_t(int v) => new(v);
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal readonly struct char_t(byte value)
{
    public readonly byte Value = value;
}
