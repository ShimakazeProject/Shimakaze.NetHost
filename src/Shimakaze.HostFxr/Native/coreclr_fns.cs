using System.Runtime.InteropServices;

using Shimakaze.Marshals;

namespace Shimakaze.Native;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct load_assembly_and_get_function_pointer_fn
{
    public unsafe delegate* unmanaged<
        char_t* /* assembly_path */,
        char_t* /* type_name */,
        char_t* /* method_name */,
        char_t* /* delegate_type_name */,
        void* /* reserved */,
        out void* /* delegate */,
        int> Value;

    public static unsafe implicit operator load_assembly_and_get_function_pointer_fn(nint ptr) => new() { Value = (delegate* unmanaged<char_t*, char_t*, char_t*, char_t*, void*, out void*, int>)ptr };
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct component_entry_point_fn
{
    public unsafe delegate* unmanaged<
        void* /* arg */,
        int32_t /* arg_size_in_bytes */,
        int> Value;

    public static unsafe implicit operator component_entry_point_fn(nint ptr) => new() { Value = (delegate* unmanaged<void*, int32_t, int>)ptr };
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct get_function_pointer_fn
{
    public unsafe delegate* unmanaged<
        char_t* /* type_name */,
        char_t* /* method_name */,
        char_t* /* delegate_type_name */,
        void* /* load_context */,
        void* /* reserved */,
        out void* /* delegate */,
        int> Value;

    public static unsafe implicit operator get_function_pointer_fn(nint ptr) => new() { Value = (delegate* unmanaged<char_t*, char_t*, char_t*, void*, void*, out void*, int>)ptr };
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct load_assembly_fn
{
    public unsafe delegate* unmanaged<
        char_t* /* assembly_path */,
        void* /* load_context */,
        void* /* reserved */,
        int> Value;

    public static unsafe implicit operator load_assembly_fn(nint ptr) => new() { Value = (delegate* unmanaged<char_t*, void*, void*, int>)ptr };
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct load_assembly_bytes_fn
{
    public unsafe delegate* unmanaged<
        void* /* assembly_bytes */,
        size_t /* assembly_bytes_len */,
        void* /* symbols_bytes */,
        size_t /* symbols_bytes_len */,
        void* /* load_context */,
        void* /* reserved */,
        int> Value;

    public static unsafe implicit operator load_assembly_bytes_fn(nint ptr) => new() { Value = (delegate* unmanaged<void*, size_t, void*, size_t, void*, void*, int>)ptr };
}