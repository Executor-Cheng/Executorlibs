using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Bilibili.Protocol.Models.Enums;
using Google.Protobuf;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    public sealed class EnterParserV2 : InteractV2Parser<IEnterMessage, EnterMessage>
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
                        switch (reader.ReadTag() >> 3)
                        {
                            case 0:
                                {
                                    return false;
                                }
                            case 5:
                                {
                                    return (InteractType)reader.ReadUInt32() == InteractType.Enter;
                                }
                            default:
                                {
                                    reader.SkipLastField();
                                    break;
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
