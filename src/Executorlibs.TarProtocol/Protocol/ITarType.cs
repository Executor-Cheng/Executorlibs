using Executorlibs.TarProtocol.IO;

namespace Executorlibs.TarProtocol.Protocol
{
    public interface ITarType
    {
        void ReadFrom(TarStream stream);
        
        void WriteTo(TarStream stream);
    }
}
