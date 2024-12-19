using Executorlibs.TarsProtocol.IO;

namespace Executorlibs.TarsProtocol.Models
{
    public interface IReadableTarsType
    {
        void ReadFrom(ref TarsReader reader);
    }
}
