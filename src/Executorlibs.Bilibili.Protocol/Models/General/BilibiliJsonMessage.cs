using System;
using System.Text.Json;
using Executorlibs.MessageFramework.Models.General;
using Executorlibs.Shared.Protocol.Models.General;

namespace Executorlibs.Bilibili.Protocol.Models.General
{
    /// <summary>
    /// 表示由B站发出的消息
    /// </summary>
    /// <remarks>
    /// 继承自 <see cref="IMessage{JsonElement}"/>
    /// </remarks>
    public interface IBilibiliJsonMessage : IBilibiliMessage, IProtocolMessage<JsonElement>
    {

    }

    /// <summary>
    /// 实现 <see cref="IBilibiliJsonMessage"/> 的抽象类
    /// </summary>
    public abstract class BilibiliJsonMessage : Message<JsonElement>, IBilibiliJsonMessage
    {
        public ulong Id { get; set; }

        public uint RoomId { get; set; }

        public DateTime Time { get; set; }

        protected override JsonElement DeepClone()
        {
            if (Rawdata.ValueKind == JsonValueKind.Undefined)
            {
                return default;
            }
            return Rawdata.Clone();
        }
    }
}
