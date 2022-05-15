using Executorlibs.TarProtocol.IO;
using Executorlibs.TarProtocol.Models;
using Executorlibs.TarProtocol.Models.Primitives;

namespace Executorlibs.Huya.Protocol.Models
{
    public class PushMessageV2 : ITarType
    {
        public string GroupId = null!;

        public WsMsgItem[] Msgs = null!;

        public void ReadFrom(ref TarReader reader)
        {
            GroupId = reader.Read<TarString>();
            Msgs = reader.Read<TarList<WsMsgItem>>();
        }

        public void WriteTo(ref TarWriter writer)
        {
            new TarString(0, GroupId).WriteTo(ref writer);
            new TarList<WsMsgItem>(1, Msgs).WriteTo(ref writer);
        }
    }
}
