using Executorlibs.TarsProtocol.IO;

namespace Executorlibs.TarsProtocol.Models
{
    public interface IWritableTarsType
    {
        void WriteTo(ref TarsWriter writer);
    }
}
