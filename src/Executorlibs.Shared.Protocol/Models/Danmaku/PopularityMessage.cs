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
    }

    /// <summary>
    /// 全平台通用的当前房间人气信息
    /// </summary>
    public interface IPopularityMessage<TRawdata> : IProtocolMessage<TRawdata>
    {

    }


    public class PopularityMessage<TRawdata> : ProtocolMessage<TRawdata>, IPopularityMessage<TRawdata>
    {
        public ulong Popularity { get; set; }
    }
}
