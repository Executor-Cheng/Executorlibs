using System.Globalization;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Bilibili.Protocol.Models.Enums;
using Executorlibs.Shared;
using Executorlibs.Shared.Extensions;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    /// <summary>
    /// 处理醒目留言的 <see cref="IBilibiliJsonMessageParser{TMessage}"/>
    /// </summary>
    public class SuperChatParser : SuperChatParser<ISuperChatMessage, SuperChatMessage>
    {
        /// <summary>
        /// 初始化 <see cref="SuperChatParser"/> 类的新实例
        /// </summary>
        public SuperChatParser() { }
    }

    public class SuperChatParser<TMessage, TImpl> : BilibiliMappableJsonMessageParser<TMessage, TImpl> where TMessage : ISuperChatMessage
                                                                                                       where TImpl : SuperChatMessage, TMessage, new()
    {
        private const string Command = "SUPER_CHAT_MESSAGE";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="SuperChatParser{TMessage, TImpl}"/> 类的新实例
        /// </summary>
        public SuperChatParser() { }

        protected override TImpl CreateMessage(JsonElement rawdata)
        {
            var message = base.CreateMessage(rawdata);
            JsonElement data = rawdata.GetProperty("data"),
                               user = data.GetProperty("user_info"),
                               medal = data.GetProperty("medal_info"),
                               id = data.GetProperty("id"),
                               medalColor = medal.ValueKind == JsonValueKind.Object ? medal.GetProperty("medal_color") : default,
                               userId = data.GetProperty("uid");
            message.Time = Utils.UnixTime2DateTime(data.GetProperty("start_time").GetInt32());
            message.Comment = data.GetProperty("message").GetString()!;
            message.UserId = userId.ValueKind == JsonValueKind.Number ? userId.GetUInt64() : ulong.Parse(userId.GetString()!);
            message.UserName = user.GetProperty("uname").GetString()!;
            message.IsAdmin = user.GetProperty("manager").GetInt32() == 1;
            message.LordType = user.GetProperty("is_vip").GetInt32() == 1 ? (user.GetProperty("is_svip").GetInt32() == 1 ? LordType.Yearly : LordType.Monthly) : LordType.None;
            message.Medal = medal.HasValues() ? new Medal
            {
                Level = medal.GetProperty("medal_level").GetUInt32(),
                Name = medal.GetProperty("medal_name").GetString()!,
                Master = medal.GetProperty("anchor_uname").GetString()!,
                MasterId = medal.GetProperty("target_id").GetUInt64(),
                RoomId = medal.GetProperty("anchor_roomid").GetUInt32(),
#if NETSTANDARD2_0
                Color = medalColor.ValueKind == JsonValueKind.Number ? medalColor.GetUInt32() : uint.Parse(medalColor.GetString()!.Substring(1), NumberStyles.HexNumber),
#else
                Color = medalColor.ValueKind == JsonValueKind.Number ? medalColor.GetUInt32() : uint.Parse(medalColor.GetString()![1..], NumberStyles.HexNumber),
#endif
                // Badge = 
            } : null;
            message.Level = user.GetProperty("user_level").GetUInt32();
            message.Title = Title.Parse(user.GetProperty("title").GetString());
            message.GuardType = (GuardType)user.GetProperty("guard_level").GetInt32();
            message.GiftSeedPrice = (uint)(data.GetProperty("price").GetDouble() * 1000);
            //KeepTime = TimeSpan.FromSeconds(data.GetProperty("time").GetInt32()), 时间可能会差一秒, 改为本地判断
            message.Id = id.ValueKind == JsonValueKind.Number ? id.GetUInt32() : uint.Parse(id.GetString()!);
            message.Token = uint.Parse(data.GetProperty("token").GetString()!, NumberStyles.HexNumber);
            return message;
        }
    }
}
