using System.Runtime.InteropServices;
using Executorlibs.FFmpegInterop.Models;

#pragma warning disable CA1401 // P/Invokes should not be visible
namespace Executorlibs.FFmpegInterop
{
    public static unsafe class AVCodecNativeMethods
    {
        [DllImport("avcodec-59.dll", EntryPoint = "avcodec_find_decoder", CallingConvention = CallingConvention.Cdecl)]
        public static extern AVCodec* FindDecoder(int coderId);

        [DllImport("avcodec-59.dll", EntryPoint = "avcodec_find_encoder", CallingConvention = CallingConvention.Cdecl)]
        public static extern AVCodec* FindEncoder(int coderId);

        [DllImport("avcodec-59.dll", EntryPoint = "av_parser_init", CallingConvention = CallingConvention.Cdecl)]
        public static extern void* InitParser(int coderId);

        [DllImport("avcodec-59.dll", EntryPoint = "avcodec_alloc_context3", CallingConvention = CallingConvention.Cdecl)]
        public static extern AVCodecContext* AllocContext3(AVCodec* codec);

        [DllImport("avcodec-59.dll", EntryPoint = "avcodec_close", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CloseContext(AVCodecContext* ctx);

        [DllImport("avcodec-59.dll", EntryPoint = "avcodec_free_context", CallingConvention = CallingConvention.Cdecl)]
        public static extern void FreeContext(AVCodecContext** ctx);

        [DllImport("avcodec-59.dll", EntryPoint = "avcodec_open2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Open2(AVCodecContext* avctx, AVCodec* codec, void** options);

        [DllImport("avcodec-59.dll", EntryPoint = "av_init_packet", CallingConvention = CallingConvention.Cdecl)]
        public static extern void InitPacket(void* hPacket);

        [DllImport("avcodec-59.dll", EntryPoint = "avcodec_send_packet", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SendPacket(AVCodecContext* avctx, AVPacket* avpkt);

        [DllImport("avcodec-59.dll", EntryPoint = "avcodec_send_frame", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SendFrame(AVCodecContext* avctx, AVFrame* frame);

        [DllImport("avcodec-59.dll", EntryPoint = "avcodec_receive_packet", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReceivePacket(AVCodecContext* avctx, AVPacket* avpkt);

        [DllImport("avcodec-59.dll", EntryPoint = "avcodec_receive_frame", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReceiveFrame(AVCodecContext* avctx, AVFrame* frame);

        [DllImport("avcodec-59.dll", EntryPoint = "av_parser_parse2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Parse2(void* parserContext, AVCodecContext* avctx, byte** poutbuf, int* poutbuf_size, ref byte buf, int buf_size, long pts, long dts, long pos);

        [DllImport("avcodec-59.dll", EntryPoint = "av_packet_alloc", CallingConvention = CallingConvention.Cdecl)]
        public static extern AVPacket* AllocatePacket();

        [DllImport("avcodec-59.dll", EntryPoint = "av_packet_unref", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UnreferencePacket(AVPacket* packet);

        [DllImport("avcodec-59.dll", EntryPoint = "av_packet_free", CallingConvention = CallingConvention.Cdecl)]
        public static extern void FreePacket(AVPacket** packet);

        [DllImport("avcodec-59.dll", EntryPoint = "avcodec_parameters_to_context", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CopyParametersToContext(AVCodecContext* codec, AVCodecParameters* parameters);

        [DllImport("avcodec-59.dll", EntryPoint = "avcodec_flush_buffers", CallingConvention = CallingConvention.Cdecl)]
        public static extern int FlushBuffers(AVCodecContext* codec);
    }
}
