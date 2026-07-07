using System.Runtime.InteropServices;

using Shimakaze.Marshals;

namespace Shimakaze.Native;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
internal struct load_assembly_and_get_function_pointer_fn
{
    [FieldOffset(0)]
    public nint Value;

    [FieldOffset(0)]
    private readonly unsafe delegate* unmanaged[Stdcall]<
        char_t* /* assembly_path */,
        char_t* /* type_name */,
        char_t* /* method_name */,
        char_t* /* delegate_type_name */,
        void* /* reserved */,
        out void* /* delegate */,
        int> _windows;

    [FieldOffset(0)]
    private readonly unsafe delegate* unmanaged[Cdecl]<
        char_t* /* assembly_path */,
        char_t* /* type_name */,
        char_t* /* method_name */,
        char_t* /* delegate_type_name */,
        void* /* reserved */,
        out void* /* delegate */,
        int> _unix;

    public readonly unsafe int Invoke(char_t* assembly_path, char_t* type_name, char_t* method_name, char_t* delegate_type_name, void* reserved, out void* @delegate)
        => OS.IsWindows
            ? _windows(assembly_path, type_name, method_name, delegate_type_name, reserved, out @delegate)
            : _unix(assembly_path, type_name, method_name, delegate_type_name, reserved, out @delegate);

    public static implicit operator load_assembly_and_get_function_pointer_fn(nint ptr) => new() { Value = ptr };
}

[StructLayout(LayoutKind.Explicit, Pack = 1)]
internal struct component_entry_point_fn
{
    [FieldOffset(0)]
    public nint Value;

    [FieldOffset(0)]
    private readonly unsafe delegate* unmanaged[Stdcall]<
        void* /* arg */,
        int32_t /* arg_size_in_bytes */,
        int> _windows;

    [FieldOffset(0)]
    private readonly unsafe delegate* unmanaged[Cdecl]<
        void* /* arg */,
        int32_t /* arg_size_in_bytes */,
        int> _unix;

    public readonly unsafe int Invoke(void* arg, int arg_size_in_bytes)
        => OS.IsWindows
            ? _windows(arg, arg_size_in_bytes)
            : _unix(arg, arg_size_in_bytes);

    public static implicit operator component_entry_point_fn(nint ptr) => new() { Value = ptr };
}

[StructLayout(LayoutKind.Explicit, Pack = 1)]
internal struct get_function_pointer_fn
{
    [FieldOffset(0)]
    public nint Value;

    [FieldOffset(0)]
    private readonly unsafe delegate* unmanaged[Stdcall]<
        char_t* /* type_name */,
        char_t* /* method_name */,
        char_t* /* delegate_type_name */,
        void* /* load_context */,
        void* /* reserved */,
        out void* /* delegate */,
        int> _windows;

    [FieldOffset(0)]
    private readonly unsafe delegate* unmanaged[Cdecl]<
        char_t* /* type_name */,
        char_t* /* method_name */,
        char_t* /* delegate_type_name */,
        void* /* load_context */,
        void* /* reserved */,
        out void* /* delegate */,
        int> _unix;

    public readonly unsafe int Invoke(char_t* type_name, char_t* method_name, char_t* delegate_type_name, void* load_context, void* reserved, out void* @delegate)
        => OS.IsWindows
            ? _windows(type_name, method_name, delegate_type_name, load_context, reserved, out @delegate)
            : _unix(type_name, method_name, delegate_type_name, load_context, reserved, out @delegate);

    public static implicit operator get_function_pointer_fn(nint ptr) => new() { Value = ptr };
}

[StructLayout(LayoutKind.Explicit, Pack = 1)]
internal struct load_assembly_fn
{
    [FieldOffset(0)]
    public nint Value;

    [FieldOffset(0)]
    private readonly unsafe delegate* unmanaged[Stdcall]<
        char_t* /* assembly_path */,
        void* /* load_context */,
        void* /* reserved */,
        int> _windows;

    [FieldOffset(0)]
    private readonly unsafe delegate* unmanaged[Cdecl]<
        char_t* /* assembly_path */,
        void* /* load_context */,
        void* /* reserved */,
        int> _unix;

    public readonly unsafe int Invoke(char_t* assembly_path, void* load_context, void* reserved)
        => OS.IsWindows
            ? _windows(assembly_path, load_context, reserved)
            : _unix(assembly_path, load_context, reserved);

    public static implicit operator load_assembly_fn(nint ptr) => new() { Value = ptr };
}

[StructLayout(LayoutKind.Explicit, Pack = 1)]
internal struct load_assembly_bytes_fn
{
    [FieldOffset(0)]
    public nint Value;

    [FieldOffset(0)]
    private readonly unsafe delegate* unmanaged[Stdcall]<
        void* /* assembly_bytes */,
        size_t /* assembly_bytes_len */,
        void* /* symbols_bytes */,
        size_t /* symbols_bytes_len */,
        void* /* load_context */,
        void* /* reserved */,
        int> _windows;

    [FieldOffset(0)]
    private readonly unsafe delegate* unmanaged[Cdecl]<
        void* /* assembly_bytes */,
        size_t /* assembly_bytes_len */,
        void* /* symbols_bytes */,
        size_t /* symbols_bytes_len */,
        void* /* load_context */,
        void* /* reserved */,
        int> _unix;

    public readonly unsafe int Invoke(void* assembly_bytes, size_t assembly_bytes_len, void* symbols_bytes, size_t symbols_bytes_len, void* load_context, void* reserved)
        => OS.IsWindows
            ? _windows(assembly_bytes, assembly_bytes_len, symbols_bytes, symbols_bytes_len, load_context, reserved)
            : _unix(assembly_bytes, assembly_bytes_len, symbols_bytes, symbols_bytes_len, load_context, reserved);

    public static implicit operator load_assembly_bytes_fn(nint ptr) => new() { Value = ptr };
}