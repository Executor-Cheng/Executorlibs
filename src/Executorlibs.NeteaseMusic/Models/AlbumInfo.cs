using System;
using System.Text.Json;
using Executorlibs.Shared;

namespace Executorlibs.NeteaseMusic.Models
{
    public class AlbumInfo
    {
        public long Id { get; }

        public string Name { get; }

        public DateTime PublishTime { get; }

        public int Count { get; }

        public AlbumInfo(long id, string name, DateTime publishTime, int count)
        {
            Id = id;
            Name = name;
            PublishTime = publishTime;
            Count = count;
        }

        public static AlbumInfo Parse(JsonElement node)
        {
            long publishTime = node.TryGetProperty("publishTime", out JsonElement publishTimeNode) ? publishTimeNode.GetInt64() : 0;
            int count = node.TryGetProperty("size", out JsonElement sizeNode) ? sizeNode.GetInt32() : 0;
            return new AlbumInfo(node.GetProperty("id").GetInt64(), node.GetProperty("name").GetString()!, Utils.UnixTime2DateTime(publishTime), count);
        }
    }
}
