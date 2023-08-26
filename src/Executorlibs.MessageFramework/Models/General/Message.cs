namespace Executorlibs.MessageFramework.Models.General
{
    /// <summary>
    /// 消息的基接口
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// 是否拦截消息传递
        /// </summary>
        bool BlockRemainingHandlers { get; set; }
    }

    /// <summary>
    /// 实现消息的基本消息的抽象类
    /// </summary>
    public abstract class Message : IMessage
    {
        /// <inheritdoc/>
        public abstract bool BlockRemainingHandlers { get; set; }
    }

    /// <summary>
    /// 表示不具有原始数据的基本消息
    /// </summary>
    public class DefaultMessage : Message
    {
        public override bool BlockRemainingHandlers { get; set; }
    }

    /// <summary>
    /// 具有原始数据的消息基接口
    /// </summary>
    /// <typeparam name="TRawdata">原始消息数据类型</typeparam>
    /// <remarks>
    /// 继承自 <see cref="IMessage"/>
    /// </remarks>
    public interface IMessage<TRawdata> : IMessage
    {
        bool RawdataPersisted { get; set; }

        /// <summary>
        /// 原始数据
        /// </summary>
        TRawdata? Rawdata { get; }

        void PersistRawdata();
    }

    /// <summary>
    /// 实现具有原始数据消息的抽象类
    /// </summary>
    /// <typeparam name="TRawdata">原始数据类型</typeparam>
    public abstract class Message<TRawdata> : Message, IMessage<TRawdata> // 建议传消息只使用接口（方便继承）
    {
        protected const uint BlockFlag = 0x1;

        protected const uint RawdataPersistedFlag = 0x2;

        protected volatile uint _flag;

        public sealed override bool BlockRemainingHandlers
        {
            get => (_flag & BlockFlag) != 0;
            set => SetFlag(BlockFlag, value);
        }

        public bool RawdataPersisted
        {
            get => (_flag & RawdataPersistedFlag) != 0;
            set => SetFlag(RawdataPersistedFlag, value);
        }

        /// <inheritdoc/>
        public TRawdata? Rawdata { get; set; } // Parser 应当设定它为 non-default

        public void PersistRawdata()
        {
            if (!RawdataPersisted)
            {
                Rawdata = DeepClone();
                SetFlag(RawdataPersistedFlag);
            }
        }

        protected abstract TRawdata? DeepClone();

#if !NET8_0
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected static byte Bool2Byte(bool x)
        {
            return System.Runtime.CompilerServices.Unsafe.As<bool, byte>(ref x);
        }
#endif

        protected void SetFlag(uint flag)
        {
            _flag |= flag;
        }

        protected void SetFlag(uint flag, bool status)
        {
#if NET8_0
            uint x = status ? 1u : 0;
            x = (uint)-(int)x;
#else
            uint x = Bool2Byte(status);
            x--;
#endif
            _flag ^= (x ^ _flag) & flag;
        }
    }
}
