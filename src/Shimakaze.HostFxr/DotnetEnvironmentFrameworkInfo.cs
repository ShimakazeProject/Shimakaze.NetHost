using System;

using Shimakaze.Marshals;
using Shimakaze.Native;

namespace Shimakaze;

public sealed class DotNetEnvironmentFrameworkInfo
{
    public string? Name { get; set; }
    public string? Version { get; set; }
    public string? Path { get; set; }

    internal unsafe IDisposable Fixed(out hostfxr_dotnet_environment_framework_info parameters)
    {
        var _1 = StringMarshal.Fixed(Name, out var name);
        var _2 = StringMarshal.Fixed(Version, out var version);
        var _3 = StringMarshal.Fixed(Path, out var path);
        parameters = new()
        {
            size = sizeof(hostfxr_dotnet_environment_framework_info),
            name = name,
            version = version,
            path = path,
        };

        return new CombineDisposable(_1, _2, _3);
    }

    internal static unsafe DotNetEnvironmentFrameworkInfo From(in hostfxr_dotnet_environment_framework_info info) => new()
    {
        Name = StringMarshal.From(info.name, ushort.MaxValue),
        Version = StringMarshal.From(info.version, ushort.MaxValue),
        Path = StringMarshal.From(info.path, ushort.MaxValue),
    };
}
