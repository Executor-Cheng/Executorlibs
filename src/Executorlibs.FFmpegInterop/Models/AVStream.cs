using System.Runtime.InteropServices;

namespace Executorlibs.FFmpegInterop.Models
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct AVStream
    {
        [FieldOffset(0)]
        private fixed byte _data[224];

        [FieldOffset(0)]
        public int Index;

        [FieldOffset(24)]
        public long StartTime;

        [FieldOffset(32)]
        public long Duration;

        [FieldOffset(40)]
        public long FrameCount;

        [FieldOffset(208)]
        public AVCodecParameters* CodecParameters;
    }
}
