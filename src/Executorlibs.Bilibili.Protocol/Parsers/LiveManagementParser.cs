using System;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    public abstract class LiveManagementParser<TMessage, TImpl> : BilibiliMappableParser<TMessage> where TMessage : ILiveManagementMessage
                                                                                                   where TImpl : LiveManagementMessage, TMessage, new()
    {
        /// <summary>
        /// 将给定的 <paramref name="root"/> 处理为 <typeparamref name="TMessage"/> 实例
        /// </summary>
        /// <param name="root">消息数据</param>
        public override TMessage Parse(in JsonElement root)
        {
            return new TImpl
            {
                Message = root.TryGetProperty("msg", out JsonElement msgToken) ? msgToken.GetString() : null,
                Time = DateTime.Now,
                Rawdata = root
            };
        }
    }
}
