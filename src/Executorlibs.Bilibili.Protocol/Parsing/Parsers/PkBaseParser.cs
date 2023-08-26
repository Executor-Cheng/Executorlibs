using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Shared;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    public abstract class PkBaseParser<TMessage, TImpl> : BilibiliMappableJsonMessageParser<TMessage, TImpl> where TMessage : IPkBaseMessage
                                                                                                             where TImpl : PkBaseMessage, TMessage, new()
    {
        protected override TImpl CreateMessage(JsonElement rawdata)
        {
            var message = base.CreateMessage(rawdata);
            var idToken = rawdata.GetProperty("pk_id");
            message.Id = idToken.ValueKind == JsonValueKind.Number ? idToken.GetUInt32() : uint.Parse(idToken.GetString()!); // 该死的后端真的该鲨了祭天
            message.Time = Utils.UnixTime2DateTime(rawdata.GetProperty("timestamp").GetInt32());
            return message;
        }
    }
}
