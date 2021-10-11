using System;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.General;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    /// <summary>
    /// 处理简单B站消息的 <see cref="IBilibiliMessageParser{TMessage}"/>
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    /// <typeparam name="TImplementation"></typeparam>
    public abstract class SimpleBilibiliMessageParser<TInterface, TImplementation> : BilibiliMappableMessageParser<TInterface> where TInterface : IBilibiliMessage
                                                                                                                        where TImplementation : BilibiliMessage, TInterface, new()
    {
        /// <summary>
        /// 将给定的 <paramref name="root"/> 处理为 <typeparamref name="TInterface"/> 实例
        /// </summary>
        /// <param name="root">消息数据</param>
        public override TInterface Parse(in JsonElement root)
        {
            return new TImplementation
            {
                Time = DateTime.Now,
                Rawdata = root
            };
        }
    }
}
