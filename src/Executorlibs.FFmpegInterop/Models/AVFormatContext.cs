using System.Runtime.InteropServices;

namespace Executorlibs.FFmpegInterop.Models
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct AVFormatContext
    {
        [FieldOffset(0)]
        private fixed byte _data[472];

        [FieldOffset(32)]
        public AVIOContext* Pb;

        [FieldOffset(44)]
        public int StreamCount;

        [FieldOffset(48)]
        public AVStream** Streams;

        [FieldOffset(64)]
        public long StartTime;

        [FieldOffset(72)]
        public long Duration;

        [FieldOffset(80)]
        public long Bitrate;
    }
}
