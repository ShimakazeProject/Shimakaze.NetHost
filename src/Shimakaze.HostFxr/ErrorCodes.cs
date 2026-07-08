
namespace Shimakaze;

public static class ErrorCodes
{
    // Success
    public const int Success = 0;
    public const int Success_HostAlreadyInitialized = 0x00000001;
    public const int Success_DifferentRuntimeProperties = 0x00000002;

    // Failure
    public const int InvalidArgFailure = unchecked((int)0x80008081);
    public const int CoreHostLibLoadFailure = unchecked((int)0x80008082);
    public const int CoreHostLibMissingFailure = unchecked((int)0x80008083);
    public const int CoreHostEntryPointFailure = unchecked((int)0x80008084);
    public const int CoreHostCurHostFindFailure = unchecked((int)0x80008085);
    // unused = unchecked((int)0x80008086);
    public const int CoreClrResolveFailure = unchecked((int)0x80008087);
    public const int CoreClrBindFailure = unchecked((int)0x80008088);
    public const int CoreClrInitFailure = unchecked((int)0x80008089);
    public const int CoreClrExeFailure = unchecked((int)0x8000808a);
    public const int ResolverInitFailure = unchecked((int)0x8000808b);
    public const int ResolverResolveFailure = unchecked((int)0x8000808c);
    public const int LibHostCurExeFindFailure = unchecked((int)0x8000808d);
    public const int LibHostInitFailure = unchecked((int)0x8000808e);
    // unused = unchecked((int)0x8000808f);
    public const int LibHostExecModeFailure = unchecked((int)0x80008090);
    public const int LibHostSdkFindFailure = unchecked((int)0x80008091);
    public const int LibHostInvalidArgs = unchecked((int)0x80008092);
    public const int InvalidConfigFile = unchecked((int)0x80008093);
    public const int AppArgNotRunnable = unchecked((int)0x80008094);
    public const int AppHostExeNotBoundFailure = unchecked((int)0x80008095);
    public const int FrameworkMissingFailure = unchecked((int)0x80008096);
    public const int HostApiFailed = unchecked((int)0x80008097);
    public const int HostApiBufferTooSmall = unchecked((int)0x80008098);
    public const int LibHostUnknownCommand = unchecked((int)0x80008099);
    public const int LibHostAppRootFindFailure = unchecked((int)0x8000809a);
    public const int SdkResolverResolveFailure = unchecked((int)0x8000809b);
    public const int FrameworkCompatFailure = unchecked((int)0x8000809c);
    public const int FrameworkCompatRetry = unchecked((int)0x8000809d);
    public const int AppHostExeNotBundle = unchecked((int)0x8000809e); // unused (not defined in error_codes.h but described in host-error-codes.md)
    public const int BundleExtractionFailure = unchecked((int)0x8000809f);
    public const int BundleExtractionIOError = unchecked((int)0x800080a0);
    public const int LibHostDuplicateProperty = unchecked((int)0x800080a1);
    public const int HostApiUnsupportedVersion = unchecked((int)0x800080a2);
    public const int HostInvalidState = unchecked((int)0x800080a3);
    public const int HostPropertyNotFound = unchecked((int)0x800080a4);
    public const int CoreHostIncompatibleConfig = unchecked((int)0x800080a5);
    public const int HostApiUnsupportedScenario = unchecked((int)0x800080a6);
    public const int HostFeatureDisabled = unchecked((int)0x800080a7);
}