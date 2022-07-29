using System.Runtime.InteropServices;
using Executorlibs.FFmpegInterop.Models;

#pragma warning disable CA1401 // P/Invokes should not be visible
namespace Executorlibs.FFmpegInterop
{
    public static unsafe class AVUtilNativeMethods
    {
        [DllImport("avutil-57.dll", EntryPoint = "av_malloc", CallingConvention = CallingConvention.StdCall)]
        public static extern void* Allocate(ulong size);

        [DllImport("avutil-57.dll", EntryPoint = "av_free", CallingConvention = CallingConvention.StdCall)]
        public static extern void Free(void* ptr);

        [DllImport("avutil-57.dll", EntryPoint = "av_frame_alloc", CallingConvention = CallingConvention.StdCall)]
        public static extern AVFrame* AllocateFrame();

        [DllImport("avutil-57.dll", EntryPoint = "av_frame_free", CallingConvention = CallingConvention.StdCall)]
        public static extern void FreeFrame(AVFrame** frame);

        [DllImport("avutil-57.dll", EntryPoint = "av_frame_unref", CallingConvention = CallingConvention.StdCall)]
        public static extern void UnreferenceFrame(AVFrame* frame);

        [DllImport("avutil-57.dll", EntryPoint = "av_get_bytes_per_sample", CallingConvention = CallingConvention.StdCall)]
        public static extern int GetBytesPerSample(AVSampleFormat format);

        [DllImport("avutil-57.dll", EntryPoint = "av_get_channel_layout_nb_channels", CallingConvention = CallingConvention.StdCall)]
        public static extern int GetChannelsByChannelLayout(ulong channelLayout);

        [DllImport("avutil-57.dll", EntryPoint = "av_samples_get_buffer_size", CallingConvention = CallingConvention.StdCall)]
        public static extern int GetSamplesBufferSize(int* linesize, int channels, int samples, AVSampleFormat sampleFormat, int align);
    }
}
