using System.Text.Json;

namespace Executorlibs.NeteaseMusic.Models
{
    public class ArtistInfo
    {
        public long Id { get; }

        public string Name { get; }

        public ArtistInfo(long id, string name)
        {
            Id = id;
            Name = name;
        }
        
        public static ArtistInfo Parse(JsonElement node)
        {
            return new ArtistInfo(node.GetProperty("id").GetInt64(), node.GetProperty("name").GetString()!);
        }
    }
}
