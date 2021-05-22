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
    public interface IBilibiliMessage : IProtocolMessage<JsonElement>
    {

    }

    /// <summary>
    /// 实现 <see cref="IBilibiliMessage"/> 的抽象类
    /// </summary>
    public abstract class BilibiliMessage : ProtocolMessage<JsonElement>, IBilibiliMessage
    {

    }
}
