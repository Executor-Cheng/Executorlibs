using System.Globalization;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Bilibili.Protocol.Models.Enums;
using Executorlibs.Shared;
using Executorlibs.Shared.Extensions;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    /// <summary>
    /// 处理普通弹幕的 <see cref="IBilibiliMessageParser{TMessage}"/>
    /// </summary>
    public sealed class DanmakuParser : DanmakuParser<IDanmakuMessage, DanmakuMessage>
    {

    }

    /// <summary>
    /// 处理普通弹幕的 <see cref="IBilibiliMessageParser{TMessage}"/>
    /// </summary>
    public class DanmakuParser<TMessage, TImpl> : BilibiliMappableMessageParser<TMessage> where TMessage : IDanmakuMessage
                                                                                   where TImpl : DanmakuMessage, TMessage, new()
    {
        private const string Command = "DANMU_MSG";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="DanmakuParser{TMessage, TImpl}"/> 类的新实例
        /// </summary>
        public DanmakuParser() { }

        /// <summary>
        /// 将给定的 <paramref name="root"/> 处理为 <typeparamref name="TMessage"/> 实例
        /// </summary>
        /// <param name="root">消息数据</param>
        public override TMessage Parse(in JsonElement root)
        {
            JsonElement info = root.GetProperty("info"),
                        info_0 = info[0],
                        info_2 = info[2],
                        info_3 = info[3],
                        info_4 = info[4],
                        info_5 = info[5];
            int id = int.Parse(info[9].GetProperty("ct").GetString()!, NumberStyles.HexNumber);
            return new TImpl
            {
                Time = Utils.UnixTime2DateTime(info_0[4].GetInt64()),
                IsLotteryDanmaku = info_0[9].GetInt32() > 0,
                Comment = info[1].GetString()!,
                UserId = info_2[0].GetInt64(),
                UserName = info_2[1].GetString()!,
                IsAdmin = info_2[2].GetInt32() == 1,
                LordType = info_2[3].GetInt32() == 1 ? (info_2[4].GetInt32() == 1 ? LordType.Yearly : LordType.Monthly) : LordType.None,
                Medal = info_3.HasValues() ? new Medal
                {
                    Level = info_3[0].GetInt32(),
                    Name = info_3[1].GetString()!,
                    Master = info_3[2].GetString()!,
                    MasterId = info_3[12].GetInt32(),
                    RoomId = info_3[3].GetInt32(),
                    Color = info_3[12].GetInt32(),
                    // Badge = null 以后再整
                } : null,
                Level = info_4[0].GetInt32(),
                Rank = info_4[3].ValueKind == JsonValueKind.Number ? info_4[3].GetInt32() : (int?)null,
                Title = info_5.HasValues() ? Title.Parse(info_5[1].GetString()) : null,
                GuardType = (GuardType)info[7].GetInt32(),
                Id = id,
                Token = id,
                Mode = (DanmakuMode)info_0[1].GetInt32(),
                Rawdata = root
            };
        }
    }
}
