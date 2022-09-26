using System.Runtime.InteropServices;
using Executorlibs.FFmpegInterop.Models;

#pragma warning disable CA1401 // P/Invokes should not be visible
namespace Executorlibs.FFmpegInterop
{
    public static unsafe class AVFormatNativeMethods
    {
        [DllImport("avformat-59.dll", EntryPoint = "avformat_alloc_context", CallingConvention = CallingConvention.StdCall)]
        public static extern AVFormatContext* AllocateContext();

        [DllImport("avformat-59.dll", EntryPoint = "avformat_free_context", CallingConvention = CallingConvention.StdCall)]
        public static extern void FreeContext(AVFormatContext* context);

        [DllImport("avformat-59.dll", EntryPoint = "avio_alloc_context", CallingConvention = CallingConvention.StdCall)]
        public static extern AVIOContext* AllocateIOContext(void* buffer, int bufferSize, int writeFlag, void* opaque,
                                                            delegate* unmanaged[Cdecl]<void*, byte*, int, int> readFunction,
                                                            delegate* unmanaged[Cdecl]<void*, byte*, int, int> writeFunction,
                                                            delegate* unmanaged[Cdecl]<void*, long, int, long> seekFunction);

        [DllImport("avformat-59.dll", EntryPoint = "avio_context_free", CallingConvention = CallingConvention.StdCall)]
        public static extern void FreeIOContext(AVIOContext** context);

        [DllImport("avformat-59.dll", EntryPoint = "avformat_open_input", CallingConvention = CallingConvention.StdCall)]
        public static extern int OpenInput(AVFormatContext** context, byte* url, AVInputFormat* format, AVDictionary** options);

        [DllImport("avformat-59.dll", EntryPoint = "avformat_find_stream_info", CallingConvention = CallingConvention.StdCall)]
        public static extern int FindStreamInfo(AVFormatContext* context, AVDictionary** options);

        [DllImport("avformat-59.dll", EntryPoint = "av_find_best_stream", CallingConvention = CallingConvention.StdCall)]
        public static extern int FindBestStream(AVFormatContext* context, AVMediaType type, int wantedStreamCount, int relatedStream, AVCodec** decoder, int flags);

        [DllImport("avformat-59.dll", EntryPoint = "av_read_frame", CallingConvention = CallingConvention.StdCall)]
        public static extern int ReadFrame(AVFormatContext* context, AVPacket* packet);
    }
}
