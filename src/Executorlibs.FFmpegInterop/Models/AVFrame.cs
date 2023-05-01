using System.Runtime.InteropServices;

namespace Executorlibs.FFmpegInterop.Models
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct AVFrame
    {
        [FieldOffset(0)]
        public byte** Data;

        [FieldOffset(64)]
        public fixed int Linesize[8];

        [FieldOffset(96)]
        public byte** ExtendedData;

        [FieldOffset(104)]
        public int Width;

        [FieldOffset(108)]
        public int Height;

        [FieldOffset(112)]
        public int SampleCount;

        [FieldOffset(116)]
        public int Format;

        [FieldOffset(120)]
        public int KeyFrame;

        [FieldOffset(136)]
        public int Pts;

        [FieldOffset(160)]
        public int CodedPictureNumber;

        [FieldOffset(384)]
        public int PacketSize;
    }
}
