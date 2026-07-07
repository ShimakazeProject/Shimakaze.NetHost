using System;

using Shimakaze.Marshals;
using Shimakaze.Native;

namespace Shimakaze;

public sealed class DotNetEnvironmentSdkInfo
{
    public string? Version { get; set; }
    public string? Path { get; set; }

    internal unsafe IDisposable Fixed(out hostfxr_dotnet_environment_sdk_info parameters)
    {
        var _1 = StringMarshal.Fixed(Version, out var version);
        var _2 = StringMarshal.Fixed(Path, out var path);
        parameters = new()
        {
            size = sizeof(hostfxr_dotnet_environment_sdk_info),
            version = version,
            path = path,
        };

        return new CombineDisposable(_1, _2);
    }

    internal static unsafe DotNetEnvironmentSdkInfo From(in hostfxr_dotnet_environment_sdk_info info) => new()
    {
        Version = StringMarshal.From(info.version, ushort.MaxValue),
        Path = StringMarshal.From(info.path, ushort.MaxValue),
    };
}
