using System.Runtime.InteropServices;

namespace Executorlibs.FFmpegInterop.Models
{
    [StructLayout(LayoutKind.Explicit, Size = 104)]
    public unsafe struct AVPacket
    {
        [FieldOffset(8)]
        public long Pts;

        [FieldOffset(16)]
        public long Dts;

        [FieldOffset(24)]
        public byte* Data;

        [FieldOffset(32)]
        public int Size;

        [FieldOffset(36)]
        public int StreamIndex;

        [FieldOffset(96)]
        public AVRelational TimeBase;
    }
}
