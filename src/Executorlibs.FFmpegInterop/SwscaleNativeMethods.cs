using System.Runtime.InteropServices;
using Executorlibs.FFmpegInterop.Models;

#pragma warning disable CA1401 // P/Invokes should not be visible
namespace Executorlibs.FFmpegInterop
{
    public static unsafe class SwscaleNativeMethods
    {
        [DllImport("swscale-6.dll", EntryPoint = "sws_getContext", CallingConvention = CallingConvention.Cdecl)]
        public static extern SwsContext* GetContext(int srcWidth, int srcHeight, int srcPixelFormat, int dstWidth, int dstHeight, int dstPixelFormat, int flags, SwsFilter* srcFilter, SwsFilter* dstFilter, double* param);

        [DllImport("swscale-6.dll", EntryPoint = "sws_freeContext", CallingConvention = CallingConvention.Cdecl)]
        public static extern void FreeContext(SwsContext* context);

        [DllImport("swscale-6.dll", EntryPoint = "sws_scale", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Scale(SwsContext* context, byte** srcSlice, int* srcStride, int srcSliceY, int srcSliceHeight, byte** dstSlice, int* dstStride);
    }
}
