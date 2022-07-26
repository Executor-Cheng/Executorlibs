using System;

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
    }
}
