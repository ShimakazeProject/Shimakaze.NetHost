using System;

using Shimakaze.Marshals;
using Shimakaze.Native;

namespace Shimakaze;

public sealed class DotNetEnvironmentInfo
{
    public string? HostfxrVersion { get; set; }
    public string? HostfxrCommitHash { get; set; }
    public DotNetEnvironmentSdkInfo[] Sdks { get; set; } = [];
    public DotNetEnvironmentFrameworkInfo[] Frameworks { get; set; } = [];

    internal static unsafe DotNetEnvironmentInfo From(hostfxr_dotnet_environment_info* info)
    {
        string? hostfxrVersion = StringMarshal.From(info->hostfxr_version, ushort.MaxValue);
        string? hostfxrCommitHash = StringMarshal.From(info->hostfxr_commit_hash, ushort.MaxValue);
        var sdks = NativeMemoryHandle.Alloc<DotNetEnvironmentSdkInfo>((int)info->sdk_count.Value);
        var frameworks = NativeMemoryHandle.Alloc<DotNetEnvironmentFrameworkInfo>((int)info->framework_count.Value);
        for (int i = 0; i < sdks.Length; i++)
            sdks[i] = DotNetEnvironmentSdkInfo.From(info->sdks[i]);
        for (int i = 0; i < frameworks.Length; i++)
            frameworks[i] = DotNetEnvironmentFrameworkInfo.From(info->frameworks[i]);

        return new()
        {
            HostfxrVersion = hostfxrVersion,
            HostfxrCommitHash = hostfxrCommitHash,
            Sdks = sdks,
            Frameworks = frameworks,
        };
    }

    internal unsafe IDisposable Fixed(out hostfxr_dotnet_environment_info parameters)
    {
        var _1 = StringMarshal.Fixed(HostfxrVersion, out var hostfxr_version);
        var _2 = StringMarshal.Fixed(HostfxrCommitHash, out var hostfxr_commit_hash);
        NativeMemoryHandle hSdks = new(Sdks.Length, sizeof(hostfxr_dotnet_environment_sdk_info));
        var dsdks = NativeMemoryHandle.Alloc<IDisposable>(Sdks.Length);
        var sdks = (hostfxr_dotnet_environment_sdk_info*)hSdks.Handle;

        NativeMemoryHandle hFrameworks = new(Frameworks.Length, sizeof(hostfxr_dotnet_environment_framework_info));
        var dframeworks = NativeMemoryHandle.Alloc<IDisposable>(Frameworks.Length);
        var frameworks = (hostfxr_dotnet_environment_framework_info*)hFrameworks.Handle;
        for (int i = 0; i < Sdks.Length; i++)
            dsdks[i] = Sdks[i].Fixed(out sdks[i]);
        for (int i = 0; i < Frameworks.Length; i++)
            dframeworks[i] = Frameworks[i].Fixed(out frameworks[i]);

        parameters = new()
        {
            size = sizeof(hostfxr_dotnet_environment_info),
            hostfxr_version = hostfxr_version,
            hostfxr_commit_hash = hostfxr_commit_hash,

            sdk_count = Sdks.Length,
            sdks = sdks,

            framework_count = Frameworks.Length,
            frameworks = frameworks,
        };

        return new CombineDisposable([.. dframeworks, hFrameworks, .. dsdks, hSdks, _2, _1]);
    }
}