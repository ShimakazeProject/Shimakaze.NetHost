using System;
using System.Runtime.InteropServices;

namespace Shimakaze;

internal static class HostFxrExtensions
{
    extension(HostFxr hostfxr)
    {
        public unsafe int Initialize(string[] args, in InitializeParameters? parameters, out HostContext context)
        {
            char** argv = (char**)NativeMemory.Alloc((nuint)args.Length, (nuint)sizeof(char*));
            try
            {
                for (var i = 0; i < args.Length; i++)
                {
                    argv[i] = (char*)NativeMemory.Alloc((nuint)args[i].Length + 1, sizeof(char));
                    fixed (char* ptr = args[i])
                        NativeMemory.Copy(ptr, argv[i], (nuint)args[i].Length * sizeof(char));

                    argv[i][args[i].Length] = '\0';
                }

                nint hContext;
                var result = parameters switch
                {
                    InitializeParameters param => hostfxr.InitializeForDotnetCommandLine(args.Length, argv, &param, out hContext),
                    _ => hostfxr.InitializeForDotnetCommandLine(args.Length, argv, null, out hContext)
                };

                context = new(hostfxr, hContext);
                return result;
            }
            finally
            {
                for (var i = 0; i < args.Length; i++)
                    NativeMemory.Free(argv[i]);

                NativeMemory.Free(argv);
            }
        }

        public unsafe int Initialize(string runtime_config_path, in InitializeParameters? parameters, out HostContext context)
        {
            fixed (char* ptr = runtime_config_path)
            {
                nint hContext;
                var result = parameters switch
                {
                    InitializeParameters param => hostfxr.InitializeForRuntimeConfig(ptr, &param, out hContext),
                    _ => hostfxr.InitializeForRuntimeConfig(ptr, null, out hContext)
                };

                context = new(hostfxr, hContext);
                return result;
            }
        }
    }
}

#if !NET7_0_OR_GREATER
file static class NativeMemory
{
    public static unsafe void Free(void* ptr) => Marshal.FreeHGlobal((nint)ptr);
    public static unsafe void* Alloc(nuint elementCount, nuint elementSize) => (void*)Marshal.AllocHGlobal(unchecked((int)(elementCount * elementSize)));
    public static unsafe void Copy(void* source, void* destination, nuint byteCount) => Buffer.MemoryCopy(source, destination, byteCount, byteCount);
}
#endif