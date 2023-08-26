using System.Diagnostics.CodeAnalysis;

namespace Executorlibs.Bilibili.Protocol.Clients
{
    public abstract class PayloadDecoder
    {
        protected byte[] _decompressBuffer;

        protected PayloadDecoder() : this(new byte[4096])
        {

        }

        protected PayloadDecoder(byte[] decompressBuffer)
        {
            _decompressBuffer = decompressBuffer;
        }

        public abstract bool TryOpen(byte[] rawdata);

#if NETSTANDARD2_0
        public abstract bool TryProcess(out byte[]? decodedRawdata);
#else
        public abstract bool TryProcess([NotNullWhen(true)] out byte[]? decodedRawdata);
#endif
        public abstract void Close();
    }
}
