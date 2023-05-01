using System;
using System.Runtime.InteropServices;

namespace Executorlibs.FFmpegInterop.Models
{
    public unsafe struct ManagedAVCodecContext : IDisposable
    {
        private readonly AVCodecContext* _context;

        public ManagedAVCodecContext(AVCodecContext* context)
        {
            _context = context;
        }

        public static implicit operator AVCodecContext*(ManagedAVCodecContext context)
        {
            return context._context;
        }

        public void Dispose()
        {
            AVCodecNativeMethods.CloseContext(_context);
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct AVCodecContext
    {
        [FieldOffset(12)]
        public int CodecType;

        [FieldOffset(24)]
        public int CodecId;

        [FieldOffset(56)]
        public long Bitrate;

        [FieldOffset(100)]
        public AVRelational TimeBase;

        [FieldOffset(116)]
        public int Width;

        [FieldOffset(120)]
        public int Height;

        [FieldOffset(136)]
        public int PixelFormat;

        [FieldOffset(352)]
        public int SampleRate;

        [FieldOffset(356)]
        public int Channels;

        [FieldOffset(360)]
        public AVSampleFormat SampleFormat;

        [FieldOffset(368)]
        public int FrameNumber;

        [FieldOffset(384)]
        public ulong ChannelLayout;

        [FieldOffset(712)]
        public AVRelational Framerate;
    }
}
