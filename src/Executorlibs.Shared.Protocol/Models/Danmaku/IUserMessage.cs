using Executorlibs.Shared.Protocol.Models.General;

namespace Executorlibs.Shared.Protocol.Models.Danmaku
{
    /// <summary>
    /// 全平台通用的用户基本信息接口
    /// </summary>
    public interface IUserMessage : IProtocolMessage
    {
        /// <summary>
        /// 用户名
        /// </summary>
        string UserName { get; }

        object UserId { get; }
    }

    /// <summary>
    /// 全平台通用的用户基本信息接口
    /// </summary>
    /// <typeparam name="TRawdata">原始数据类型</typeparam>
    /// <typeparam name="TUserId">用户Id的类型</typeparam>
    public interface IUserMessage<TRawdata, TUserId> : IUserMessage, IProtocolMessage<TRawdata>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        new TUserId UserId { get; }

#if !NETSTANDARD2_0
        object IUserMessage.UserId => UserId!;
#endif
    }

    public abstract class UserMessage<TRawdata, TUserId> : ProtocolMessage<TRawdata>, IUserMessage<TRawdata, TUserId>
    {
        public string UserName { get; set; } = null!;

        public TUserId UserId { get; set; } = default!;

#if NETSTANDARD2_0
        object IUserMessage.UserId => UserId!;
#endif
    }
}
