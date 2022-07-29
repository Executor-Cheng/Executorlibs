using System;
using System.Text.Json;

namespace Executorlibs.NeteaseMusic.Models
{
    public class DownloadSongInfo
    {
        public long Id { get; }

        public int Bitrate { get; }

        public Quality Quality { get; }

        public string Url { get; }

        public string Type { get; }

        public DateTime ExpireTime { get; }

        public DownloadSongInfo(long id, int bitrate, string url, string type)
        {
            Id = id;
            Bitrate = bitrate;
            Quality = bitrate switch // For self uploaded musics, bitrate may not be one of the Quality Enum values
            {
                <= (int)Quality.Unknown => Quality.Unknown,
                <= (int)Quality.LowQuality => Quality.LowQuality,
                <= (int)Quality.MediumQuality => Quality.MediumQuality,
                <= (int)Quality.HighQuality => Quality.HighQuality,
                _ => Quality.SuperQuality
            };
            Url = url;
            Type = type;
            ExpireTime = DateTime.Now.AddMinutes(20);
        }


        public static DownloadSongInfo Parse(JsonElement node)
        {
            return new DownloadSongInfo(node.GetProperty("id").GetInt64(), node.GetProperty("br").GetInt32(), node.GetProperty("url").GetString()!, node.GetProperty("type").GetString()!);
        }
    }
}
