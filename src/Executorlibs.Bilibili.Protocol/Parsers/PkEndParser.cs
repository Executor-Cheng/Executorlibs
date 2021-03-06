using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    public class PkEndParser : PkEndParser<IPkEndMessage, PkEndMessage>
    {
        /// <summary>
        /// 初始化 <see cref="PkEndParser"/> 类的新实例
        /// </summary>
        public PkEndParser() { }
    }

    public class PkEndParser<TMessage, TImpl> : PkBaseParser<TMessage, TImpl> where TMessage : IPkEndMessage
                                                                              where TImpl : PkEndMessage, TMessage, new()
    {
        private const string Command = "PK_BATTLE_END";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="PkEndParser{TMessage, TImpl}"/> 类的新实例
        /// </summary>
        public PkEndParser() { }

        /// <inheritdoc/>
        public override TMessage Parse(in JsonElement root)
        {
            TImpl message = (TImpl)base.Parse(root);
            message.EndTime = message.Time;

            JsonElement data = root.GetProperty("data"),
                        red = data.GetProperty("init_info"),
                        blue = data.GetProperty("match_info");

            int redRoomId = red.GetProperty("room_id").GetInt32(),
                blueRoomId = blue.GetProperty("room_id").GetInt32();

            if (redRoomId > blueRoomId) // 规定红方的房间号必须比蓝方的房间号小
            {
                redRoomId ^= blueRoomId;
                blueRoomId ^= redRoomId;
                redRoomId ^= blueRoomId;
                data = red;
                red = blue;
                blue = data;
            }

            message.RedRoomId = redRoomId;
            message.RedScore = red.GetProperty("votes").GetInt64();
            message.RedMatchResult = red.GetProperty("winner_type").GetInt32();
            message.RedBestUserName = red.GetProperty("best_uname").GetString()!;

            message.BlueRoomId = blueRoomId;
            message.BlueScore = blue.GetProperty("votes").GetInt64();
            message.BlueMatchResult = blue.GetProperty("winner_type").GetInt32();
            message.BlueBestUserName = blue.GetProperty("best_uname").GetString()!;

            return message;
        }
    }
}
