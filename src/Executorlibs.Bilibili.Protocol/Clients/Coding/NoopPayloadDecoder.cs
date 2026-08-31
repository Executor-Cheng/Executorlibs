using System.Diagnostics.CodeAnalysis;

namespace Executorlibs.Bilibili.Protocol.Clients.Coding
{
    public class NoopPayloadDecoder : PayloadDecoder
    {
        public static readonly PayloadDecoder Singleton = new NoopPayloadDecoder();

        public override ushort ProtocolVersion => 1;

        public override void Close()
        {
            
        }

        public override bool TryOpen(byte[] rawdata)
        {
            return false;
        }

        public override bool TryProcess([NotNullWhen(true)]out byte[]? decodedRawdata)
        {
            decodedRawdata = null;
            return false;
        }
    }
}
