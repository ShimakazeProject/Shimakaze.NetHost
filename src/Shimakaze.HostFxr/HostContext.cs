using System.Runtime.InteropServices;

namespace Shimakaze;

internal sealed class HostContext(HostFxr fxr, nint invalidHandleValue) : SafeHandle(invalidHandleValue, true)
{
#if NET7_0_OR_GREATER
    public override bool IsInvalid => handle is 0;
#else
    public override bool IsInvalid => handle is (nint)0; 
#endif

    public unsafe int RunApp() => fxr.RunApp(handle);

    public unsafe int GetRuntimeDelegate(DelegateType type, out nint @delegate) => fxr.GetRuntimeDelegate(handle, type, out @delegate);

    public new unsafe int Close() => fxr.Close(handle);

    protected override bool ReleaseHandle() => Close() == 0;
}