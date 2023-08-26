using System.Globalization;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Bilibili.Protocol.Models.Enums;
using Executorlibs.Shared;
using Executorlibs.Shared.Extensions;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    /// <summary>
    /// 处理普通弹幕的 <see cref="IBilibiliJsonMessageParser{TMessage}"/>
    /// </summary>
    public abstract class DanmakuParserBase<TMessage, TImpl> : BilibiliJsonMessageParser<TMessage, TImpl> where TMessage : IDanmakuMessage
                                                                                                          where TImpl : DanmakuMessage, TMessage, new()
    {
        protected const string Command = "DANMU_MSG";

        protected override TImpl CreateMessage(JsonElement rawdata)
        {
            var message = base.CreateMessage(rawdata);
            JsonElement info = rawdata.GetProperty("info"),
                        info_0 = info[0],
                        info_2 = info[2],
                        info_3 = info[3],
                        info_4 = info[4],
                        info_5 = info[5];
            uint id = uint.Parse(info[9].GetProperty("ct").GetString()!, NumberStyles.HexNumber);
            message.Time = Utils.UnixTime2DateTime(info_0[4].GetInt64());
            message.IsLotteryDanmaku = info_0[9].GetInt32() > 0;
            message.Comment = info[1].GetString()!;
            message.UserId = info_2[0].GetUInt64();
            message.UserName = info_2[1].GetString()!;
            message.IsAdmin = info_2[2].GetInt32() == 1;
            message.LordType = info_2[3].GetInt32() == 1 ? (info_2[4].GetInt32() == 1 ? LordType.Yearly : LordType.Monthly) : LordType.None;
            message.Medal = info_3.HasValues() ? new Medal
            {
                Level = info_3[0].GetUInt32(),
                Name = info_3[1].GetString()!,
                Master = info_3[2].GetString()!,
                MasterId = info_3[12].GetUInt64(),
                RoomId = info_3[3].GetUInt32(),
                Color = info_3[12].GetUInt32(),
                // Badge = null 以后再整
            } : null;
            message.Level = info_4[0].GetUInt32();
            message.Rank = info_4[3].ValueKind == JsonValueKind.Number ? info_4[3].GetUInt32() : default(uint?);
            message.Title = info_5.HasValues() ? Title.Parse(info_5[1].GetString()) : null;
            message.GuardType = (GuardType)info[7].GetInt32();
            message.Id = id;
            message.Token = id;
            message.Mode = (DanmakuMode)info_0[1].GetInt32();
            return message;
        }
    }
}
