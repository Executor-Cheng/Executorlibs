using System.Runtime.InteropServices;

namespace Executorlibs.FFmpegInterop.Models
{
    [StructLayout(LayoutKind.Explicit)]
    public struct AVFrame
    {
        [FieldOffset(0)]
        public unsafe byte** Data;

        [FieldOffset(64)]
        public unsafe fixed int Linesize[8];

        [FieldOffset(96)]
        public unsafe byte** ExtendedData;

        [FieldOffset(104)]
        public int Width;

        [FieldOffset(108)]
        public int Height;

        [FieldOffset(112)]
        public int SampleCount;

        [FieldOffset(116)]
        public AVSampleFormat Format;

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
