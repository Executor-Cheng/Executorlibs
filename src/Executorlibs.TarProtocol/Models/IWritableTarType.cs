using Executorlibs.TarProtocol.IO;

namespace Executorlibs.TarProtocol.Models
{
    public interface IWritableTarType
    {
        void WriteTo(ref TarWriter writer);
    }
}
