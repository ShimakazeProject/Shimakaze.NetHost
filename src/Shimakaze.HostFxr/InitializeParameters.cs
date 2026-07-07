using System;

using Shimakaze.Marshals;
using Shimakaze.Native;

namespace Shimakaze;

public sealed class InitializeParameters
{
    public string? HostPath { get; set; }
    public string? DotnetRoot { get; set; }

    internal unsafe IDisposable Fixed(out hostfxr_initialize_parameters parameters)
    {
        var _1 = StringMarshal.Fixed(HostPath, out var host_path);
        var _2 = StringMarshal.Fixed(DotnetRoot, out var dotnet_root);
        parameters = new()
        {
            size = sizeof(hostfxr_initialize_parameters),
            host_path = host_path,
            dotnet_root = dotnet_root,
        };

        return new CombineDisposable(_1, _2);
    }
}
