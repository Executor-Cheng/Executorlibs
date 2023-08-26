using System;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.General;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    public interface IUnknownJsonMessageParser : IBilibiliJsonMessageParser<IUnknownJsonMessage>
    {

    }

    /// <summary>
    /// 处理未知消息的 <see cref="IBilibiliJsonMessageParser{TMessage}"/>
    /// </summary>
    public class UnknownJsonMessageParser<TMessage> : BilibiliJsonMessageParser<IUnknownJsonMessage, TMessage>, IUnknownJsonMessageParser where TMessage : UnknownJsonMessage, new()
    {
        /// <summary>
        /// 初始化 <see cref="UnknownJsonMessageParser{TImpl}"/> 类的新实例
        /// </summary>
        public UnknownJsonMessageParser()
        {

        }

        public override bool CanParse(JsonElement root)
        {
            return true;
        }

        protected override TMessage CreateMessage(JsonElement rawdata)
        {
            return new TMessage()
            {
                Rawdata = rawdata,
                Time = DateTime.Now
            };
        }
    }

    /// <summary>
    /// 处理未知消息的 <see cref="IBilibiliJsonMessageParser{TMessage}"/>
    /// </summary>
    public sealed class UnknownJsonMessageParser : UnknownJsonMessageParser<UnknownJsonMessage>
    {
        /// <summary>
        /// 初始化 <see cref="UnknownJsonMessageParser"/> 类的新实例
        /// </summary>
        public UnknownJsonMessageParser()
        {

        }
    }
}
