using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Shared;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    public abstract class PkBaseParser<TMessage, TImpl> : BilibiliMappableParser<TMessage> where TMessage : IPkBaseMessage
                                                                                           where TImpl : PkBaseMessage, TMessage, new()
    {
        /// <summary>
        /// 将给定的 <paramref name="root"/> 处理为 <typeparamref name="TMessage"/> 实例
        /// </summary>
        /// <param name="root">消息数据</param>
        public override TMessage Parse(in JsonElement root)
        {
            TImpl message = new TImpl();
            JsonElement idToken = root.GetProperty("pk_id");
            message.Id = idToken.ValueKind == JsonValueKind.Number ? idToken.GetInt32() : int.Parse(idToken.GetString()!); // 该死的后端真的该鲨了祭天
            message.Time = Utils.UnixTime2DateTime(root.GetProperty("timestamp").GetInt32());
            return message;
        }
    }
}
