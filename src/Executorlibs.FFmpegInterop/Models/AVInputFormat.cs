using System.Runtime.InteropServices;

namespace Executorlibs.FFmpegInterop.Models
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct AVInputFormat
    {
        [FieldOffset(0)]
        private fixed byte _data[152];
    }
}
