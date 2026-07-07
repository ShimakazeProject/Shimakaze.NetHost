using System.Runtime.InteropServices;

namespace Shimakaze.Native;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct hostfxr_handle
{
    public nint Value;
}

internal enum hostfxr_delegate_type
{
    hdt_com_activation,
    hdt_load_in_memory_assembly,
    hdt_winrt_activation,
    hdt_com_register,
    hdt_com_unregister,
    hdt_load_assembly_and_get_function_pointer,
    hdt_get_function_pointer,
    hdt_load_assembly,
    hdt_load_assembly_bytes,
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct hostfxr_initialize_parameters
{
    public size_t size;
    public unsafe char_t* host_path;
    public unsafe char_t* dotnet_root;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct hostfxr_dotnet_environment_sdk_info
{
    public size_t size;
    public unsafe char_t* version;
    public unsafe char_t* path;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct hostfxr_dotnet_environment_framework_info
{
    public size_t size;
    public unsafe char_t* name;
    public unsafe char_t* version;
    public unsafe char_t* path;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct hostfxr_dotnet_environment_info
{
    public size_t size;

    public unsafe char_t* hostfxr_version;
    public unsafe char_t* hostfxr_commit_hash;

    public size_t sdk_count;
    public unsafe hostfxr_dotnet_environment_sdk_info* sdks;

    public size_t framework_count;
    public unsafe hostfxr_dotnet_environment_framework_info* frameworks;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct hostfxr_framework_result
{
    public size_t size;
    public unsafe char_t* name;
    public unsafe char_t* requested_version;
    public unsafe char_t* resolved_version;
    public unsafe char_t* resolved_path;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct hostfxr_resolve_frameworks_result
{
    public size_t size;

    public size_t resolved_count;
    public unsafe hostfxr_framework_result* resolved_frameworks;

    public size_t unresolved_count;
    public unsafe hostfxr_framework_result* unresolved_frameworks;
}
