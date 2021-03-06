using System;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    public class RoomChangeParser : RoomChangeParser<IRoomChangeMessage, RoomChangeMessage>
    {
        /// <summary>
        /// 初始化 <see cref="RoomChangeParser"/> 类的新实例
        /// </summary>
        public RoomChangeParser() { }
    }

    public class RoomChangeParser<TMessage, TImpl> : BilibiliMappableMessageParser<TMessage> where TMessage : IRoomChangeMessage
                                                                                      where TImpl : RoomChangeMessage, TMessage, new()
    {
        private const string Command = "ROOM_CHANGE";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="RoomChangeParser{TMessage, TImpl}"/> 类的新实例
        /// </summary>
        public RoomChangeParser() { }

        /// <summary>
        /// 将给定的 <paramref name="root"/> 处理为 <typeparamref name="TMessage"/> 实例
        /// </summary>
        /// <param name="root">消息数据</param>
        public override TMessage Parse(in JsonElement root)
        {
            JsonElement data = root.GetProperty("data");
            return new TImpl
            {
                Title = data.GetProperty("title").GetString()!,
                AreaId = data.GetProperty("parent_area_id").GetInt32(),
                SubAreaId = data.GetProperty("area_id").GetInt32(),
                Time = DateTime.Now,
                Rawdata = root,
            };
        }
    }
}
