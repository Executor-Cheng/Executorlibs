using System.Runtime.InteropServices;

namespace Executorlibs.FFmpegInterop.Models
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct AVCodecParameters
    {
        [FieldOffset(0)]
        private fixed byte _data[144];

        [FieldOffset(0)]
        public int CodecType;

        [FieldOffset(4)]
        public int CodecId;

        [FieldOffset(28)]
        public AVSampleFormat Format;

        [FieldOffset(32)]
        public long Bitrate;

        [FieldOffset(104)]
        public ulong ChannelLayout;

        [FieldOffset(112)]
        public int Channels;

        [FieldOffset(116)]
        public int SampleRate;

        [FieldOffset(124)]
        public int FrameSize;
    }
}
