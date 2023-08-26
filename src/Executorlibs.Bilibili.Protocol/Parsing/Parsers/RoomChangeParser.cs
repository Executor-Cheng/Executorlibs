using System;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    public class RoomChangeParser : RoomChangeParser<IRoomChangeMessage, RoomChangeMessage>
    {
        /// <summary>
        /// 初始化 <see cref="RoomChangeParser"/> 类的新实例
        /// </summary>
        public RoomChangeParser() { }
    }

    public class RoomChangeParser<TMessage, TImpl> : BilibiliMappableJsonMessageParser<TMessage, TImpl> where TMessage : IRoomChangeMessage
                                                                                                        where TImpl : RoomChangeMessage, TMessage, new()
    {
        private const string Command = "ROOM_CHANGE";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="RoomChangeParser{TMessage, TImpl}"/> 类的新实例
        /// </summary>
        public RoomChangeParser() { }

        protected override TImpl CreateMessage(JsonElement rawdata)
        {
            var message = base.CreateMessage(rawdata);
            var data = rawdata.GetProperty("data");
            message.Title = data.GetProperty("title").GetString()!;
            message.AreaId = data.GetProperty("parent_area_id").GetUInt32();
            message.SubAreaId = data.GetProperty("area_id").GetUInt32();
            message.Time = DateTime.Now;
            return message;
        }
    }
}
