using System.Text.Json;

namespace Executorlibs.NeteaseMusic.Models
{
    public class PlaylistInfo
    {
        public long Id { get; }

        public string? Name { get; }

        public PlaylistInfo(long id, string? name)
        {
            Id = id;
            Name = name;
        }

        public static PlaylistInfo Parse(JsonElement node)
        {
            return new PlaylistInfo(node.GetProperty("id").GetInt64(), null);
        }
    }
}
