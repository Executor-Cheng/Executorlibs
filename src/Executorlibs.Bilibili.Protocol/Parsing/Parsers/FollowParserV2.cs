using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Bilibili.Protocol.Models.Enums;
using Google.Protobuf;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    /// <summary>
    /// 处理用户关注直播间消息的 <see cref="IBilibiliJsonMessageParser{TMessage}"/>
    /// </summary>
    public sealed class FollowParserV2 : InteractV2Parser<IFollowMessage, FollowMessage>
    {
        /// <inheritdoc/>
        public override bool CanParse(JsonElement root)
        {
            if (root.TryGetProperty("data", out JsonElement data) &&
                data.TryGetProperty("pb", out JsonElement pbNode))
            {
                var pb = pbNode.GetBytesFromBase64();
                using var reader = new CodedInputStream(pb);
                try
                {
                    while (true)
                    {
                        switch (reader.ReadTag())
                        {
                            case 0:
                                {
                                    break;
                                }
                            case 5:
                                {
                                    return ((InteractType)reader.ReadUInt32() & InteractType.Follow) != InteractType.None;
                                }
                        }
                    }
                }
                catch
                {

                }
            }
            return false;
        }
    }
}
