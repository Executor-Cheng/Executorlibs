using Executorlibs.TarProtocol.IO;

namespace Executorlibs.TarProtocol.Models
{
    public interface IReadableTarType
    {
        void ReadFrom(ref TarReader reader);
    }
}
