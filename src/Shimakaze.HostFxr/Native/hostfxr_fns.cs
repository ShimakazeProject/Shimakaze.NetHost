using System.Runtime.InteropServices;

namespace Shimakaze.Native;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct hostfxr_main_fn
{
    public unsafe delegate* unmanaged[Cdecl]<int /* argc */, char_t** /* argv */, int32_t> Value;

    public static unsafe implicit operator hostfxr_main_fn(nint ptr) => new() { Value = (delegate* unmanaged[Cdecl]<int, char_t**, int32_t>)ptr };
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct hostfxr_main_startupinfo_fn
{
    public unsafe delegate* unmanaged[Cdecl]<
        int /* argc */,
        char_t** /* argv */,
        char_t* /* host_path */,
        char_t* /* dotnet_root */,
        char_t* /* app_path */,
        int32_t> Value;

    public static unsafe implicit operator hostfxr_main_startupinfo_fn(nint ptr) => new() { Value = (delegate* unmanaged[Cdecl]<int, char_t**, char_t*, char_t*, char_t*, int32_t>)ptr };
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct hostfxr_main_bundle_startupinfo_fn
{
    public unsafe delegate* unmanaged[Cdecl]<
        int /* argc */,
        char_t** /* argv */,
        char_t* /* host_path */,
        char_t* /* dotnet_root */,
        char_t* /* app_path */,
        int64_t /* bundle_header_offset */,
        int32_t> Value;

    public static unsafe implicit operator hostfxr_main_bundle_startupinfo_fn(nint ptr) => new() { Value = (delegate* unmanaged[Cdecl]<int, char_t**, char_t*, char_t*, char_t*, int64_t, int32_t>)ptr };
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct hostfxr_set_error_writer_fn
{
    public unsafe delegate* unmanaged[Cdecl]<hostfxr_error_writer_fn /* error_writer */, hostfxr_error_writer_fn> Value;

    public static unsafe implicit operator hostfxr_set_error_writer_fn(nint ptr) => new() { Value = (delegate* unmanaged[Cdecl]<hostfxr_error_writer_fn, hostfxr_error_writer_fn>)ptr };
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct hostfxr_error_writer_fn
{
    public unsafe delegate* unmanaged[Cdecl]<char_t* /* message */, void> Value;

    public static unsafe implicit operator hostfxr_error_writer_fn(nint ptr) => new() { Value = (delegate* unmanaged[Cdecl]<char_t*, void>)ptr };

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void Delegate(char_t* message);
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct hostfxr_initialize_for_dotnet_command_line_fn
{
    public unsafe delegate* unmanaged[Cdecl]<
        int /* argc */,
        char_t** /* argv */,
        hostfxr_initialize_parameters* /* parameters */,
        out hostfxr_handle /* host_context_handle */,
        int32_t> Value;

    public static unsafe implicit operator hostfxr_initialize_for_dotnet_command_line_fn(nint ptr) => new() { Value = (delegate* unmanaged[Cdecl]<int, char_t**, hostfxr_initialize_parameters*, out hostfxr_handle, int32_t>)ptr };
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct hostfxr_initialize_for_runtime_config_fn
{
    public unsafe delegate* unmanaged[Cdecl]<
        char_t* /* runtime_config_path */,
        hostfxr_initialize_parameters* /* parameters */,
        out hostfxr_handle /* host_context_handle */,
        int32_t> Value;

    public static unsafe implicit operator hostfxr_initialize_for_runtime_config_fn(nint ptr) => new() { Value = (delegate* unmanaged[Cdecl]<char_t*, hostfxr_initialize_parameters*, out hostfxr_handle, int32_t>)ptr };
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct hostfxr_get_runtime_property_value_fn
{
    public unsafe delegate* unmanaged[Cdecl]<
        hostfxr_handle /* host_context_handle */,
        char_t* /* name */,
        char_t** /* value */,
        int32_t> Value;

    public static unsafe implicit operator hostfxr_get_runtime_property_value_fn(nint ptr) => new() { Value = (delegate* unmanaged[Cdecl]<hostfxr_handle, char_t*, char_t**, int32_t>)ptr };
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct hostfxr_set_runtime_property_value_fn
{
    public unsafe delegate* unmanaged[Cdecl]<
        hostfxr_handle /* host_context_handle */,
        char_t* /* name */,
        char_t* /* value */,
        int32_t> Value;

    public static unsafe implicit operator hostfxr_set_runtime_property_value_fn(nint ptr) => new() { Value = (delegate* unmanaged[Cdecl]<hostfxr_handle, char_t*, char_t*, int32_t>)ptr };
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct hostfxr_get_runtime_properties_fn
{
    public unsafe delegate* unmanaged[Cdecl]<
        hostfxr_handle /* host_context_handle */,
        ref size_t /* count */,
        char_t** /* keys */,
        char_t** /* values */,
        int32_t> Value;

    public static unsafe implicit operator hostfxr_get_runtime_properties_fn(nint ptr) => new() { Value = (delegate* unmanaged[Cdecl]<hostfxr_handle, ref size_t, char_t**, char_t**, int32_t>)ptr };
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct hostfxr_run_app_fn
{
    public unsafe delegate* unmanaged[Cdecl]<hostfxr_handle /* host_context_handle */, int32_t> Value;

    public static unsafe implicit operator hostfxr_run_app_fn(nint ptr) => new() { Value = (delegate* unmanaged[Cdecl]<hostfxr_handle, int32_t>)ptr };
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct hostfxr_get_runtime_delegate_fn
{
    public unsafe delegate* unmanaged[Cdecl]<
        hostfxr_handle /* host_context_handle */,
        hostfxr_delegate_type /* type */,
        out void* /* delegate */,
        int32_t> Value;

    public static unsafe implicit operator hostfxr_get_runtime_delegate_fn(nint ptr) => new() { Value = (delegate* unmanaged[Cdecl]<hostfxr_handle, hostfxr_delegate_type, out void*, int32_t>)ptr };
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct hostfxr_close_fn
{
    public unsafe delegate* unmanaged[Cdecl]<hostfxr_handle /* host_context_handle */, int32_t> Value;

    public static unsafe implicit operator hostfxr_close_fn(nint ptr) => new() { Value = (delegate* unmanaged[Cdecl]<hostfxr_handle, int32_t>)ptr };
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct hostfxr_get_dotnet_environment_info_result_fn
{
    public unsafe delegate* unmanaged[Cdecl]<
        hostfxr_dotnet_environment_info* /* info */,
        void* /* result_context */,
        void> Value;

    public static unsafe implicit operator hostfxr_get_dotnet_environment_info_result_fn(nint ptr) => new() { Value = (delegate* unmanaged[Cdecl]<hostfxr_dotnet_environment_info*, void*, void>)ptr };

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void Delegate(hostfxr_dotnet_environment_info* info, void* resultContext);
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct hostfxr_get_dotnet_environment_info_fn
{
    public unsafe delegate* unmanaged[Cdecl]<
        char_t* /* dotnet_root */,
        void* /* reserved */,
        hostfxr_get_dotnet_environment_info_result_fn /* result */,
        void* /* result_context */,
        int32_t> Value;

    public static unsafe implicit operator hostfxr_get_dotnet_environment_info_fn(nint ptr) => new() { Value = (delegate* unmanaged[Cdecl]<char_t*, void*, hostfxr_get_dotnet_environment_info_result_fn, void*, int32_t>)ptr };
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct hostfxr_resolve_frameworks_result_fn
{
    public unsafe delegate* unmanaged[Cdecl]<
        hostfxr_resolve_frameworks_result* /* result */,
        void* /* result_context */,
        void> Value;

    public static unsafe implicit operator hostfxr_resolve_frameworks_result_fn(nint ptr) => new() { Value = (delegate* unmanaged[Cdecl]<hostfxr_resolve_frameworks_result*, void*, void>)ptr };
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct hostfxr_resolve_frameworks_for_runtime_config_fn
{
    public unsafe delegate* unmanaged[Cdecl]<
        char_t* /* runtime_config_path */,
        hostfxr_initialize_parameters* /* parameters */,
        hostfxr_resolve_frameworks_result_fn* /* callback */,
        void* /* result_context */,
        int32_t> Value;

    public static unsafe implicit operator hostfxr_resolve_frameworks_for_runtime_config_fn(nint ptr) => new() { Value = (delegate* unmanaged[Cdecl]<char_t*, hostfxr_initialize_parameters*, hostfxr_resolve_frameworks_result_fn*, void*, int32_t>)ptr };
}
