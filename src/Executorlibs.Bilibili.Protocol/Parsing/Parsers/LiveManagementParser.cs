using System;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    public abstract class LiveManagementParser<TMessage, TImpl> : BilibiliMappableJsonMessageParser<TMessage, TImpl> where TMessage : ILiveManagementMessage
                                                                                                                     where TImpl : LiveManagementMessage, TMessage, new()
    {
        protected override TImpl CreateMessage(JsonElement rawdata)
        {
            var message = base.CreateMessage(rawdata);
            message.Message = rawdata.TryGetProperty("msg", out JsonElement msgToken) ? msgToken.GetString() : null;
            message.Time = DateTime.Now;
            return message;
        }
    }
}
