using System.Runtime.InteropServices;

namespace Executorlibs.FFmpegInterop.Models
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct AVCodecContext
    {
        [FieldOffset(0)]
        private fixed byte _data[920];

        [FieldOffset(56)]
        public long Bitrate;

        [FieldOffset(352)]
        public int SampleRate;

        [FieldOffset(356)]
        public int Channels;

        [FieldOffset(360)]
        public AVSampleFormat SampleFormat;

        [FieldOffset(384)]
        public ulong ChannelLayout;
    }
}
