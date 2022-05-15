using System;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.General;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    public interface IUnknownMessageParser : IBilibiliMessageParser<IUnknownMessage>
    {

    }

    /// <summary>
    /// 处理未知消息的 <see cref="IBilibiliMessageParser{TMessage}"/>
    /// </summary>
    public sealed class UnknownMessageParser : UnknownMessageParser<UnknownMessage>
    {
        /// <summary>
        /// 初始化 <see cref="UnknownMessageParser"/> 类的新实例
        /// </summary>
        public UnknownMessageParser()
        {

        }
    }

    /// <summary>
    /// 处理未知消息的 <see cref="IBilibiliMessageParser{TMessage}"/>
    /// </summary>
    public class UnknownMessageParser<TImpl> : BilibiliMessageParser<IUnknownMessage>, IUnknownMessageParser where TImpl : UnknownMessage, IUnknownMessage, new()
    {
        /// <summary>
        /// 初始化 <see cref="UnknownMessageParser{TImpl}"/> 类的新实例
        /// </summary>
        public UnknownMessageParser()
        {

        }

        public override bool CanParse(in JsonElement root)
        {
            return true;
        }

        public override IUnknownMessage Parse(in JsonElement root)
        {
            return new TImpl() { Rawdata = root, Time = DateTime.Now };
        }
    }
}
