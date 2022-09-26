using System.Runtime.InteropServices;

namespace Executorlibs.FFmpegInterop.Models
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct AVIOContext 
    {
        [FieldOffset(0)]
        private fixed byte _data[208];

        [FieldOffset(8)]
        public byte* Buffer;
    }
}
