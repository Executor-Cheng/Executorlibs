using System;
using Executorlibs.Shared.Protocol.Models.General;
#if !NETSTANDARD2_0
using Executorlibs.MessageFramework.Models.General;
#endif

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
#if !NETSTANDARD2_0
        long IMessage.Id => throw new NotSupportedException();
#endif
    }

    public class PopularityMessage : ProtocolMessage, IPopularityMessage
    {
        public ulong Popularity { get; set; }

        public override long Id { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
    }
}
