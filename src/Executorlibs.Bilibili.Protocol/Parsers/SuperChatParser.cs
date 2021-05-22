using System.Globalization;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Bilibili.Protocol.Models.Enums;
using Executorlibs.Shared;
using Executorlibs.Shared.Extensions;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    /// <summary>
    /// 处理醒目留言的 <see cref="IBilibiliMessageParser{TMessage}"/>
    /// </summary>
    public class SuperChatParser : DefaultSuperChatParser<ISuperChatMessage, SuperChatMessage>
    {
        /// <summary>
        /// 初始化 <see cref="SuperChatParser"/> 类的新实例
        /// </summary>
        public SuperChatParser() { }
    }

    public class DefaultSuperChatParser<TMessage, TImpl> : BilibiliMappableParser<TMessage> where TMessage : ISuperChatMessage
                                                                                            where TImpl : SuperChatMessage, TMessage, new()
    {
        private const string Command = "SUPER_CHAT_MESSAGE";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="DefaultSuperChatParser{TMessage, TImpl}"/> 类的新实例
        /// </summary>
        public DefaultSuperChatParser() { }

        /// <summary>
        /// 将给定的 <paramref name="root"/> 处理为 <typeparamref name="TMessage"/> 实例
        /// </summary>
        /// <param name="root">消息数据</param>
        public override TMessage Parse(in JsonElement root)
        {
            JsonElement data = root.GetProperty("data"),
                               user = data.GetProperty("user_info"),
                               medal = data.GetProperty("medal_info"),
                               id = data.GetProperty("id"),
                               medalColor = medal.ValueKind == JsonValueKind.Object ? medal.GetProperty("medal_color") : default,
                               userId = data.GetProperty("uid");
            return new TImpl
            {
                Time = Utils.UnixTime2DateTime(data.GetProperty("start_time").GetInt32()),
                Comment = data.GetProperty("message").GetString()!,
                UserId = userId.ValueKind == JsonValueKind.Number ? userId.GetInt32() : int.Parse(userId.GetString()!),
                UserName = user.GetProperty("uname").GetString()!,
                IsAdmin = user.GetProperty("manager").GetInt32() == 1,
                LordType = user.GetProperty("is_vip").GetInt32() == 1 ? (user.GetProperty("is_svip").GetInt32() == 1 ? LordType.Yearly : LordType.Monthly) : LordType.None,
                Medal = medal.HasValues() ? new Medal
                {
                    Level = medal.GetProperty("medal_level").GetInt32(),
                    Name = medal.GetProperty("medal_name").GetString()!,
                    Master = medal.GetProperty("anchor_uname").GetString()!,
                    MasterId = medal.GetProperty("target_id").GetInt32(),
                    RoomId = medal.GetProperty("anchor_roomid").GetInt32(),
                    Color = medalColor.ValueKind == JsonValueKind.Number ? medalColor.GetInt32() : int.Parse(medalColor.GetString()![1..], NumberStyles.HexNumber),
                    // Badge = 
                } : null,
                Level = user.GetProperty("user_level").GetInt32(),
                Title = Title.Parse(user.GetProperty("title").GetString()),
                GuardType = (GuardType)user.GetProperty("guard_level").GetInt32(),
                GiftSeedPrice = (int)(data.GetProperty("price").GetDouble() * 1000),
                //KeepTime = TimeSpan.FromSeconds(data.GetProperty("time").GetInt32()), 时间可能会差一秒, 改为本地判断
                Id = id.ValueKind == JsonValueKind.Number ? id.GetInt32() : int.Parse(id.GetString()!),
                Token = int.Parse(data.GetProperty("token").GetString()!, NumberStyles.HexNumber),
                Rawdata = root
            };
        }
    }
}
