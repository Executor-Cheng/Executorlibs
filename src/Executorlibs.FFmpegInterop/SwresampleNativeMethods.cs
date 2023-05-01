using System.Runtime.InteropServices;
using Executorlibs.FFmpegInterop.Models;

#pragma warning disable CA1401 // P/Invokes should not be visible
namespace Executorlibs.FFmpegInterop
{
    public static unsafe class SwresampleNativeMethods
    {
        [DllImport("swresample-4.dll", EntryPoint = "swr_alloc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void* AllocContext();

        [DllImport("swresample-4.dll", EntryPoint = "swr_alloc_set_opts", CallingConvention = CallingConvention.Cdecl)]
        public static extern void* AllocSetOptions(void* s, ulong outChannelLayout, AVSampleFormat outSampleFormat, int outSampleRate,
                                                            ulong inChannelLayout, AVSampleFormat inSampleFormat, int inSampleRate,
                                                            int logOffset, void* logContext);

        [DllImport("swresample-4.dll", EntryPoint = "swr_init", CallingConvention = CallingConvention.Cdecl)]
        public static extern int InitContext(void* s);

        [DllImport("swresample-4.dll", EntryPoint = "swr_free", CallingConvention = CallingConvention.Cdecl)]
        public static extern void FreeContext(void** s);
    }
}
