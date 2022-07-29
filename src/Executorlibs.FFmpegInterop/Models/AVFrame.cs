using System.Runtime.InteropServices;

namespace Executorlibs.FFmpegInterop.Models
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct AVFrame
    {
        [FieldOffset(0)]
        private fixed byte _data[448];

        [FieldOffset(0)]
        public fixed long Data[8];
        //#if TARGET_64BIT
        //        public fixed long Data[8];
        //#else
        //        public fixed int Data[8];
        //#endif

        [FieldOffset(64)]
        public fixed int Linesize[8];

        [FieldOffset(96)]
        public byte** ExtendedData;

        [FieldOffset(112)]
        public int SampleCount;

        [FieldOffset(116)]
        public AVSampleFormat Format;
    }
}
