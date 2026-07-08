using System;
using System.Runtime.InteropServices;

using Shimakaze.Marshals;
using Shimakaze.Native;

namespace Shimakaze;

public sealed unsafe class HostFXR : IDisposable
{
    public const string UNMANAGEDCALLERSONLY_METHOD = "UNMANAGEDCALLERSONLY_METHOD";
    private bool _disposedValue;
    private readonly nint _hModule;
    private readonly bool _leaveOpen;

    private hostfxr_main_fn _main;
    private hostfxr_main_startupinfo_fn _main_startupinfo;
    private hostfxr_main_bundle_startupinfo_fn _main_bundle_startupinfo;
    private hostfxr_set_error_writer_fn _set_error_writer;
    private hostfxr_initialize_for_dotnet_command_line_fn _initialize_for_dotnet_command_line;
    private hostfxr_initialize_for_runtime_config_fn _initialize_for_runtime_config;
    private hostfxr_get_runtime_property_value_fn _get_runtime_property_value;
    private hostfxr_set_runtime_property_value_fn _set_runtime_property_value;
    private hostfxr_get_runtime_properties_fn _get_runtime_properties;
    private hostfxr_run_app_fn _run_app;
    private hostfxr_get_runtime_delegate_fn _get_runtime_delegate;
    private hostfxr_close_fn _close;
    private hostfxr_get_dotnet_environment_info_fn _get_dotnet_environment_info;

    private HostFXR(nint hModule, bool leaveOpen)
    {
        _hModule = hModule;
        _leaveOpen = leaveOpen;
    }

    public HostFXR(nint hModule)
        : this(hModule, false)
    {
    }

    public HostFXR(string hostfxrPath)
        : this(NativeLibrary.Load(hostfxrPath), true)
    {
    }

    public void Main(string[] args)
    {
        if (_main.Value is null)
            _main = NativeLibrary.GetExport(_hModule, "hostfxr_main");

        using var _1 = StringMarshal.Fixed(args, out var argv);
        Marshal.ThrowExceptionForHR(_main.Value(args.Length, argv));
    }

    public void MainStartupinfo(string[] args, string hostPath, string dotnetRoot, string appPath)
    {
        if (_main_startupinfo.Value is null)
            _main_startupinfo = NativeLibrary.GetExport(_hModule, "hostfxr_main_startupinfo");

        using var _1 = StringMarshal.Fixed(args, out var argv);
        using var _2 = StringMarshal.Fixed(hostPath, out var host_path);
        using var _3 = StringMarshal.Fixed(dotnetRoot, out var dotnet_root);
        using var _4 = StringMarshal.Fixed(appPath, out var app_path);
        Marshal.ThrowExceptionForHR(_main_startupinfo.Value(args.Length, argv, host_path, dotnet_root, app_path));
    }

    public void MainBundleStartupinfo(string[] args, string hostPath, string dotnetRoot, string appPath, long bundleHeaderOffset)
    {
        if (_main_bundle_startupinfo.Value is null)
            _main_bundle_startupinfo = NativeLibrary.GetExport(_hModule, "hostfxr_main_bundle_startupinfo");

        using var _1 = StringMarshal.Fixed(args, out var argv);
        using var _2 = StringMarshal.Fixed(hostPath, out var host_path);
        using var _3 = StringMarshal.Fixed(dotnetRoot, out var dotnet_root);
        using var _4 = StringMarshal.Fixed(appPath, out var app_path);
        Marshal.ThrowExceptionForHR(_main_bundle_startupinfo.Value(args.Length, argv, host_path, dotnet_root, app_path, bundleHeaderOffset));
    }

    public void InitializeForDotnetCommandLine(string[] args, InitializeParameters? parameters, out HostFXRHandle hostContextHandle)
    {
        if (_initialize_for_dotnet_command_line.Value is null)
            _initialize_for_dotnet_command_line = NativeLibrary.GetExport(_hModule, "hostfxr_initialize_for_dotnet_command_line");

        hostfxr_initialize_parameters* param = null;
        using var _1 = StringMarshal.Fixed(args, out var argv);
        using var _2 = parameters?.Fixed(out *param);
        var result = _initialize_for_dotnet_command_line.Value(args.Length, argv, param, out var host_context_handle);
        hostContextHandle = new(this, host_context_handle);
        Marshal.ThrowExceptionForHR(result);
    }
    public void InitializeForRuntimeConfig(string runtimeConfigPath, InitializeParameters? parameters, out HostFXRHandle hostContextHandle)
    {
        if (_initialize_for_runtime_config.Value is null)
            _initialize_for_runtime_config = NativeLibrary.GetExport(_hModule, "hostfxr_initialize_for_runtime_config");

        hostfxr_initialize_parameters* param = null;
        using var _1 = StringMarshal.Fixed(runtimeConfigPath, out var runtime_config_path);
        using var _2 = parameters?.Fixed(out *param);
        var result = _initialize_for_runtime_config.Value(runtime_config_path, param, out var host_context_handle);
        hostContextHandle = new(this, host_context_handle);
        Marshal.ThrowExceptionForHR(result);
    }
    internal void GetRuntimePropertyValue(HostFXRHandle hostContextHandle, string name, out string? value, int bufferSize = 256)
    {
        if (_get_runtime_property_value.Value is null)
            _get_runtime_property_value = NativeLibrary.GetExport(_hModule, "hostfxr_get_runtime_property_value");

        int result;
        using var _1 = StringMarshal.Fixed(name, out var arg1);
        using var _2 = StringMarshal.Alloc(bufferSize, out var buffer);
        result = _get_runtime_property_value.Value(hostContextHandle.Handle, arg1, &buffer);
        value = StringMarshal.From(buffer, bufferSize);

        Marshal.ThrowExceptionForHR(result);
    }
    internal void SetRuntimePropertyValue(HostFXRHandle hostContextHandle, string name, string? value)
    {
        if (_set_runtime_property_value.Value is null)
            _set_runtime_property_value = NativeLibrary.GetExport(_hModule, "hostfxr_set_runtime_property_value");

        using var _1 = StringMarshal.Fixed(name, out var arg1);
        using var _2 = StringMarshal.Fixed(value, out var arg2);
        Marshal.ThrowExceptionForHR(_set_runtime_property_value.Value(hostContextHandle.Handle, arg1, arg2));
    }

    internal void GetRuntimeProperties(HostFXRHandle hostContextHandle, out int count)
    {
        if (_get_runtime_properties.Value is null)
            _get_runtime_properties = NativeLibrary.GetExport(_hModule, "hostfxr_get_runtime_properties");

        size_t i = default;
        var result = _get_runtime_properties.Value(hostContextHandle.Handle, ref i, null, null);
        count = (int)i.Value;

        Marshal.ThrowExceptionForHR(result);
    }

    internal void GetRuntimeProperties(HostFXRHandle hostContextHandle, int count, out string?[] keys, out string?[] values, int bufferSize = 256)
    {
        if (_get_runtime_properties.Value is null)
            _get_runtime_properties = NativeLibrary.GetExport(_hModule, "hostfxr_get_runtime_properties");

        size_t i = count;

        using var _1 = StringMarshal.Alloc(count, bufferSize, out var arg1);
        using var _2 = StringMarshal.Alloc(count, bufferSize, out var arg2);
        var result = _get_runtime_properties.Value(hostContextHandle.Handle, ref i, arg1, arg2);
        keys = StringMarshal.From(arg1, count, bufferSize);
        values = StringMarshal.From(arg2, count, bufferSize);

        Marshal.ThrowExceptionForHR(result);
    }

    internal void GetRuntimeProperties(HostFXRHandle hostContextHandle, out string?[] keys, out string?[] values, int bufferSize = 256)
    {
        GetRuntimeProperties(hostContextHandle, out int count);
        GetRuntimeProperties(hostContextHandle, count, out keys, out values, bufferSize);
    }

    internal void RunApp(HostFXRHandle hostContextHandle)
    {
        if (_run_app.Value is null)
            _run_app = NativeLibrary.GetExport(_hModule, "hostfxr_run_app");

        Marshal.ThrowExceptionForHR(_run_app.Value(hostContextHandle.Handle));
    }

    internal void GetRuntimeDelegate(HostFXRHandle hostContextHandle, DelegateType type, out nint @delegate)
    {
        if (_get_runtime_delegate.Value is null)
            _get_runtime_delegate = NativeLibrary.GetExport(_hModule, "hostfxr_get_runtime_delegate");

        var result = _get_runtime_delegate.Value(hostContextHandle.Handle, (hostfxr_delegate_type)type, out void* ptr);
        @delegate = (nint)ptr;
        Marshal.ThrowExceptionForHR(result);
    }

    internal void Close(HostFXRHandle hostContextHandle)
    {
        if (_close.Value is null)
            _close = NativeLibrary.GetExport(_hModule, "hostfxr_close");

        Marshal.ThrowExceptionForHR(_close.Value(hostContextHandle.Handle));
    }

    public ErrorWriter SetErrorWriter(ErrorWriter errorWriter)
    {
        if (_set_error_writer.Value is null)
            _set_error_writer = NativeLibrary.GetExport(_hModule, "hostfxr_set_error_writer");

        hostfxr_error_writer_fn.Delegate tmp = (message) => errorWriter(StringMarshal.From(message, ushort.MaxValue));
        hostfxr_error_writer_fn error_writer = Marshal.GetFunctionPointerForDelegate(tmp);
        error_writer = _set_error_writer.Value(error_writer);
        return (message) =>
        {
            using var _1 = StringMarshal.Fixed(message, out var ptr);
            error_writer.Value(ptr);
        };
    }

    public void GetDotnetEnvironmentInfo(string dotnetRoot, GetDotnetEnvironmentInfoResult result, object? resultContext)
    {
        if (_get_dotnet_environment_info.Value is null)
            _get_dotnet_environment_info = NativeLibrary.GetExport(_hModule, "hostfxr_get_dotnet_environment_info");

        void* result_context = null;
        if (resultContext is not null)
        {
            var handle = GCHandle.Alloc(resultContext);
            result_context = (void*)GCHandle.ToIntPtr(handle);
        }
        using var _1 = StringMarshal.Fixed(dotnetRoot, out var dotnet_root);

        hostfxr_get_dotnet_environment_info_result_fn.Delegate tmp = (info, result_context) =>
        {
            object? resultContext = null;
            if (result_context is not null)
                resultContext = GCHandle.FromIntPtr((nint)result_context).Target;

            result(DotNetEnvironmentInfo.From(info), resultContext);
        };
        hostfxr_get_dotnet_environment_info_result_fn fnResult = Marshal.GetFunctionPointerForDelegate(tmp);
        Marshal.ThrowExceptionForHR(_get_dotnet_environment_info.Value(dotnet_root, null, fnResult, result_context));
    }

    private void DisposeCore()
    {
        if (_disposedValue)
            return;

        if (!_leaveOpen)
            NativeLibrary.Free(_hModule);
        _disposedValue = true;
    }

    ~HostFXR()
    {
        DisposeCore();
    }

    public void Dispose()
    {
        DisposeCore();
        GC.SuppressFinalize(this);
    }
}
