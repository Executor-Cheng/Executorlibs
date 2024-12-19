using System.Runtime.InteropServices;
using Executorlibs.FFmpegInterop.Models;

#pragma warning disable CA1401 // P/Invokes should not be visible
namespace Executorlibs.FFmpegInterop
{
    public static class SwscaleNativeMethods
    {
        [DllImport("swscale-6.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "sws_getContext")]
        public unsafe static extern SwsContext* GetContext(int srcWidth, int srcHeight, int srcPixelFormat, int dstWidth, int dstHeight, int dstPixelFormat, int flags, SwsFilter* srcFilter, SwsFilter* dstFilter, double* param);

        [DllImport("swscale-6.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "sws_freeContext")]
        public unsafe static extern void FreeContext(SwsContext* context);

        [DllImport("swscale-6.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "sws_scale")]
        public unsafe static extern int Scale(SwsContext* context, byte** srcSlice, int* srcStride, int srcSliceY, int srcSliceHeight, byte** dstSlice, int* dstStride);
    }
}
