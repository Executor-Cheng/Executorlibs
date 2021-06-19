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
        /// <summary>
        /// 用户Id
        /// </summary>
        object UserId { get; }
    }

    public abstract class UserMessage : ProtocolMessage, IUserMessage
    {
        public string UserName { get; set; } = null!;

        public object UserId { get; set; } = null!;
    }

    /// <summary>
    /// 全平台通用的用户基本信息接口
    /// </summary>
    /// <typeparam name="TRawdata">原始数据类型</typeparam>
    /// <typeparam name="TUserId">用户Id的类型</typeparam>
    public interface IUserMessage<TUserId, TRawdata> : IUserMessage, IProtocolMessage<TRawdata>
    {
#if !NETSTANDARD2_0
        object IUserMessage.UserId => UserId!;
#endif

        /// <summary>
        /// 用户Id
        /// </summary>
        new TUserId UserId { get; }
    }

    public abstract class UserMessage<TUserId, TRawdata> : UserMessage, IUserMessage<TUserId, TRawdata>
    {
        public new TUserId UserId { get; set; } = default!;

        public TRawdata Rawdata { get; set; } = default!;

        object IUserMessage.UserId => UserId!;
    }
}
