using System.Runtime.InteropServices;

namespace Executorlibs.FFmpegInterop.Models
{
    [StructLayout(LayoutKind.Explicit, Size = 224)]
    public struct AVStream
    {
        [FieldOffset(0)]
        public int Index;

        [FieldOffset(16)]
        public AVRelational TimeBase;

        [FieldOffset(24)]
        public long StartTime;

        [FieldOffset(32)]
        public long Duration;

        [FieldOffset(40)]
        public long FrameCount;

        [FieldOffset(208)]
        public unsafe AVCodecParameters* CodecParameters;
    }
}
