using System;
using System.Runtime.InteropServices;

using Shimakaze.Marshals;
using Shimakaze.Native;

namespace Shimakaze;

public sealed class HostFXRHandle : IDisposable
{
    private readonly HostFXR _hostfxr;
    private bool _disposedValue;

    private load_assembly_and_get_function_pointer_fn _load_assembly_and_get_function_pointer;
    private get_function_pointer_fn _get_function_pointer;
    private load_assembly_fn _load_assembly;
    private load_assembly_bytes_fn _load_assembly_bytes;

    internal hostfxr_handle Handle { get; }

    public bool IsNull => Handle.Value is 0;

    internal HostFXRHandle(HostFXR hostfxr, hostfxr_handle handle)
    {
        _hostfxr = hostfxr;
        Handle = handle;
    }

    public unsafe void LoadAssemblyAndGetFunctionPointer(string assemblyPath, string typeName, string? methodName, string? delegateTypeName, out nint @delegate)
    {
        if (_load_assembly_and_get_function_pointer.Value is null)
        {
            GetRuntimeDelegate(DelegateType.LoadAssemblyAndGetFunctionPointer, out nint handle);
            _load_assembly_and_get_function_pointer = handle;
        }
        using var _1 = StringMarshal.Fixed(assemblyPath, out var assembly_path);
        using var _2 = StringMarshal.Fixed(typeName, out var type_name);
        using var _3 = StringMarshal.Fixed(methodName, out var method_name);
        using var _4 = StringMarshal.Fixed(delegateTypeName, out var delegate_type_name);
        if (delegateTypeName is HostFXR.UNMANAGEDCALLERSONLY_METHOD)
            delegate_type_name = (char_t*)-1;

        Marshal.ThrowExceptionForHR(_load_assembly_and_get_function_pointer.Value(assembly_path, type_name, method_name, delegate_type_name, null, out void* ptr));
        @delegate = (nint)ptr;
    }

    public unsafe void GetFunctionPointer(string typeName, string? methodName, string? delegateTypeName, out nint @delegate)
    {
        if (_get_function_pointer.Value is null)
        {
            GetRuntimeDelegate(DelegateType.GetFunctionPointer, out nint handle);
            _get_function_pointer = handle;
        }

        using var _1 = StringMarshal.Fixed(typeName, out var type_name);
        using var _2 = StringMarshal.Fixed(methodName, out var method_name);
        using var _3 = StringMarshal.Fixed(delegateTypeName, out var delegate_type_name);
        if (delegateTypeName is HostFXR.UNMANAGEDCALLERSONLY_METHOD)
            delegate_type_name = (char_t*)-1;

        Marshal.ThrowExceptionForHR(_get_function_pointer.Value(type_name, method_name, delegate_type_name, null, null, out void* ptr));
        @delegate = (nint)ptr;
    }

    public unsafe void LoadAssembly(string assemblyPath)
    {
        if (_load_assembly.Value is null)
        {
            GetRuntimeDelegate(DelegateType.LoadAssembly, out nint handle);
            _load_assembly = handle;
        }
        using var _1 = StringMarshal.Fixed(assemblyPath, out var assembly_path);
        Marshal.ThrowExceptionForHR(_load_assembly.Value(assembly_path, null, null));
    }

    public unsafe void LoadAssemblyBytes(byte[] assemblyBytes, byte[]? symbolsBytes)
    {
        if (_load_assembly_bytes.Value is null)
        {
            GetRuntimeDelegate(DelegateType.LoadAssemblyBytes, out nint handle);
            _load_assembly_bytes = handle;
        }

        fixed (void* assembly_bytes = assemblyBytes)
        fixed (void* symbols_bytes = symbolsBytes)
            Marshal.ThrowExceptionForHR(_load_assembly_bytes.Value(assembly_bytes, assemblyBytes.Length, symbols_bytes, symbolsBytes?.Length ?? 0, null, null));
    }
    public unsafe void LoadAssemblyBytes(void* assembly_bytes, int assembly_bytes_len, void* symbols_bytes, int symbols_bytes_len)
    {
        if (_load_assembly_bytes.Value is null)
        {
            GetRuntimeDelegate(DelegateType.LoadAssemblyBytes, out nint handle);
            _load_assembly_bytes = handle;
        }

        Marshal.ThrowExceptionForHR(_load_assembly_bytes.Value(assembly_bytes, assembly_bytes_len, symbols_bytes, symbols_bytes_len, null, null));
    }

#if NET || NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public unsafe void LoadAssemblyBytes(ReadOnlySpan<byte> assemblyBytes, ReadOnlySpan<byte> symbolsBytes)
    {
        if (_load_assembly_bytes.Value is null)
        {
            GetRuntimeDelegate(DelegateType.LoadAssemblyBytes, out nint handle);
            _load_assembly_bytes = handle;
        }

        fixed (void* assembly_bytes = assemblyBytes)
        fixed (void* symbols_bytes = symbolsBytes)
            Marshal.ThrowExceptionForHR(_load_assembly_bytes.Value(assembly_bytes, assemblyBytes.Length, symbols_bytes, symbolsBytes.Length, null, null));
    }
#endif

    public void GetRuntimePropertyValue(string name, out string? value, int bufferSize = 256) => _hostfxr.GetRuntimePropertyValue(this, name, out value, bufferSize);
    public void SetRuntimePropertyValue(string name, string? value) => _hostfxr.SetRuntimePropertyValue(this, name, value);
    public void GetRuntimeProperties(out string?[] keys, out string?[] values, int bufferSize = 256) => _hostfxr.GetRuntimeProperties(this, out keys, out values, bufferSize);
    public void RunApp() => _hostfxr.RunApp(this);
    public void GetRuntimeDelegate(DelegateType type, out nint @delegate) => _hostfxr.GetRuntimeDelegate(this, type, out @delegate);

    public void Close()
    {
        if (_disposedValue)
            return;

        _hostfxr.Close(this);
        _disposedValue = true;
        return;
    }

    ~HostFXRHandle()
    {
        Close();
    }

    public void Dispose()
    {
        Close();
        GC.SuppressFinalize(this);
    }
}
