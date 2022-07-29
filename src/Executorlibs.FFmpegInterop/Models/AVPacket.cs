using System.Runtime.InteropServices;

namespace Executorlibs.FFmpegInterop.Models
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct AVPacket
    {
        [FieldOffset(0)]
        private fixed byte _data[104];

        [FieldOffset(24)]
        public byte* Data;

        [FieldOffset(32)]
        public int Size;

        [FieldOffset(36)]
        public int StreamIndex;
    }
}
