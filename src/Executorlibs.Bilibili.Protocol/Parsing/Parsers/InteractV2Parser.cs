using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Bilibili.Protocol.Models.Enums;
using Executorlibs.Shared;
using Google.Protobuf;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    public abstract class InteractV2Parser<TMessage, TImpl> : BilibiliMappableJsonMessageParser<TMessage, TImpl> where TMessage : IInteractMessage
                                                                                                                 where TImpl : InteractMessage, TMessage, new()
    {
        /// <summary>
        /// 本消息的cmd值
        /// </summary>
        protected const string Command = "INTERACT_WORD_V2";

        /// <inheritdoc/>
        public override string Key => Command;

        protected override TImpl CreateMessage(JsonElement rawdata)
        {
            var message = base.CreateMessage(rawdata);
            var data = rawdata.GetProperty("data");
            var pb = data.GetProperty("pb").GetBytesFromBase64();
            using var reader = new CodedInputStream(pb);
            while (true)
            {
                switch (reader.ReadTag() >> 3)
                {
                    case 0:
                        {
                            return message;
                        }
                    case 1:
                        {
                            message.UserId = reader.ReadUInt64();
                            break;
                        }
                    case 2:
                        {
                            message.UserName = reader.ReadString();
                            break;
                        }
                    case 4:
                        {
                            int cnt = reader.ReadLength();
                            while (cnt-- != 0)
                            {
                                message.UserType |= (InteractUserType)(1 << (int)reader.ReadUInt32());
                            }
                            break;
                        }
                    case 5:
                        {
                            message.Type = (InteractType)reader.ReadUInt32();
                            break;
                        }
                    case 7:
                        {
                            message.Time = Utils.UnixTime2DateTime((int)reader.ReadUInt32());
                            break;
                        }
                    default:
                        {
                            reader.SkipLastField();
                            break;
                        }
                }
            }
        }
    }
}
