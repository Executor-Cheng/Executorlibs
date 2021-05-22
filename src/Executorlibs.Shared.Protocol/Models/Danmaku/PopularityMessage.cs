using System;
using Executorlibs.MessageFramework.Models.General;
using Executorlibs.Shared.Protocol.Models.General;

namespace Executorlibs.Shared.Protocol.Models.Danmaku
{
    /// <summary>
    /// 全平台通用的当前房间人气信息
    /// </summary>
    public interface IPopularityMessage : IProtocolMessage
    {
        /// <summary>
        /// 人气值
        /// </summary>
        ulong Popularity { get; }

        int IProtocolMessage.RoomId => throw new NotSupportedException();

        long IMessage.Id => throw new NotSupportedException();
    }

    public class PopularityMessage : IPopularityMessage
    {
        public ulong Popularity { get; set; }

        public DateTime Time { get; set; }
    }
}
