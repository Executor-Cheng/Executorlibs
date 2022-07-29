using System.Runtime.InteropServices;

namespace Executorlibs.FFmpegInterop.Models
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct AVCodec
    {
        [FieldOffset(0)]
        private fixed byte _data[224];

        [FieldOffset(20)]
        public int Id;
    }

}
