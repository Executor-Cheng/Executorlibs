using System;
using System.Linq;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    public abstract class CommonLotteryResultParser<TMessage, TImpl> : BilibiliMappableParser<TMessage> where TMessage : ICommonLotteryResultMessage
                                                                                                        where TImpl : CommonLotteryResultMessage, TMessage, new()
    {
        /// <summary>
        /// 初始化 <see cref="CommonLotteryResultParser{TMessage, TImpl}"/> 类的新实例
        /// </summary>
        public CommonLotteryResultParser() { }

        /// <summary>
        /// 将给定的 <paramref name="root"/> 处理为 <typeparamref name="TMessage"/> 实例
        /// </summary>
        /// <param name="root">消息数据</param>
        public override TMessage Parse(in JsonElement root)
        {
            JsonElement data = root.GetProperty("data");
            return new TImpl
            {
                Id = data.GetProperty("id").GetInt32(),
                AwardName = data.GetProperty("award_name").GetString()!,
                AwardNum = data.GetProperty("award_num").GetInt32(),
                AwardUsers = data.GetProperty("award_users").EnumerateArray().Select(p => (p.GetProperty("uname").GetString()!, p.GetProperty("uid").GetInt32())).ToArray(),
                Time = DateTime.Now,
                Rawdata = root
            };
        }
    }
}
