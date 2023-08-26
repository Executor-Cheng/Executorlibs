using System;
using System.Linq;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    public abstract class CommonLotteryResultParser<TMessage, TImpl> : BilibiliMappableJsonMessageParser<TMessage, TImpl> where TMessage : ICommonLotteryResultMessage
                                                                                                                          where TImpl : CommonLotteryResultMessage, TMessage, new()
    {
        /// <summary>
        /// 初始化 <see cref="CommonLotteryResultParser{TMessage, TImpl}"/> 类的新实例
        /// </summary>
        public CommonLotteryResultParser() { }

        /// <summary>
        /// 将给定的 <paramref name="rawdata"/> 处理为 <typeparamref name="TMessage"/> 实例
        /// </summary>
        /// <param name="rawdata">消息数据</param>
        protected override TImpl CreateMessage(JsonElement rawdata)
        {
            var message = base.CreateMessage(rawdata);
            var data = rawdata.GetProperty("data");
            message.Id = data.GetProperty("id").GetUInt32();
            message.AwardName = data.GetProperty("award_name").GetString()!;
            message.AwardNum = data.GetProperty("award_num").GetUInt32();
            message.AwardUsers = data.GetProperty("award_users").EnumerateArray().Select(p => (p.GetProperty("uname").GetString()!, p.GetProperty("uid").GetUInt64())).ToArray();
            message.Time = DateTime.Now;
            return message;
        }
    }
}
