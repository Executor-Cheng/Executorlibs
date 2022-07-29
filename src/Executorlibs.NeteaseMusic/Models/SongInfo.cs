using System;
using System.Linq;
using System.Text.Json;

namespace Executorlibs.NeteaseMusic.Models
{
    public class SongInfo
    {
        public long Id { get; }

        public string Name { get; }

        public ArtistInfo[] Artists { get; }

        public AlbumInfo Album { get; }

        public TimeSpan Duration { get; }

        public bool CanPlay { get; set; }

        public bool NeedPaymentToDownload { get; } // Fee == 8 | > 0

        public SongInfo(long id, string name, ArtistInfo[] artists, AlbumInfo album, TimeSpan duration, bool needPaymentToDownload)
        {
            Id = id;
            Name = name;
            Artists = artists;
            Album = album;
            Duration = duration;
            NeedPaymentToDownload = needPaymentToDownload;
        }

        public static SongInfo Parse(JsonElement node)
        {
            if (!node.TryGetProperty("album", out JsonElement albumNode))
            {
                albumNode = node.GetProperty("al");
            }
            if (!node.TryGetProperty("duration", out JsonElement durationNode))
            {
                durationNode = node.GetProperty("dt");
            }
            return new SongInfo(node.GetProperty("id").GetInt64(), node.GetProperty("name").GetString()!, node.EnumerateArray().Select(ArtistInfo.Parse).ToArray(), AlbumInfo.Parse(albumNode), TimeSpan.FromMilliseconds(durationNode.GetDouble()), node.GetProperty("fee").GetInt32() != 0);
        }
    }
}
