using System.Runtime.InteropServices;
using Executorlibs.FFmpegInterop.Models;

#pragma warning disable CA1401 // P/Invokes should not be visible
namespace Executorlibs.FFmpegInterop
{
    public static unsafe class AVFormatNativeMethods
    {
        [DllImport("avformat-59.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "avformat_alloc_context")]
        public unsafe static extern AVFormatContext* AllocateContext();

        [DllImport("avformat-59.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "avformat_free_context")]
        public unsafe static extern void FreeContext(AVFormatContext* context);

        [DllImport("avformat-59.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "avio_alloc_context")]
        public unsafe static extern AVIOContext* AllocateIOContext(void* buffer, int bufferSize, int writeFlag, void* opaque, delegate* unmanaged[Cdecl]<void*, byte*, int, int> readFunction, delegate* unmanaged[Cdecl]<void*, byte*, int, int> writeFunction, delegate* unmanaged[Cdecl]<void*, long, int, long> seekFunction);

        [DllImport("avformat-59.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "avio_context_free")]
        public unsafe static extern void FreeIOContext(AVIOContext** context);

        [DllImport("avformat-59.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "avformat_open_input")]
        public unsafe static extern int OpenInput(AVFormatContext** context, byte* url, AVInputFormat* format, AVDictionary** options);

        [DllImport("avformat-59.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "avformat_find_stream_info")]
        public unsafe static extern int FindStreamInfo(AVFormatContext* context, AVDictionary** options);

        [DllImport("avformat-59.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "av_find_best_stream")]
        public unsafe static extern int FindBestStream(AVFormatContext* context, AVMediaType type, int wantedStreamCount, int relatedStream, AVCodec** decoder, int flags);

        [DllImport("avformat-59.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "av_read_frame")]
        public unsafe static extern int ReadFrame(AVFormatContext* context, AVPacket* packet);

        [DllImport("avformat-59.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "avformat_write_header")]
        public unsafe static extern int WriteHeader(AVFormatContext* context, AVDictionary* options);

        [DllImport("avformat-59.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "av_write_trailer")]
        public unsafe static extern int WriteTrailer(AVFormatContext* context);

        [DllImport("avformat-59.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "av_seek_frame")]
        public unsafe static extern int SeekFrame(AVFormatContext* context, int streamIndex, long timestamp, int flags);
    }
}
