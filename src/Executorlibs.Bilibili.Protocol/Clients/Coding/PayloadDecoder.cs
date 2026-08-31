using System;
using System.Diagnostics.CodeAnalysis;

namespace Executorlibs.Bilibili.Protocol.Clients.Coding
{
    public class PayloadDecoderFactory
    {
        public virtual PayloadDecoder Create(ushort version)
        {
            return version switch
            {
                1 => NoopPayloadDecoder.Singleton,
                2 => new DeflatePayloadDecoder(),
#if !NETSTANDARD2_0
                3 => new BrotliPayloadDecoder(),
#endif
                _ => throw new NotSupportedException()
            };
        }
    }

    /// <summary>
    /// 弹幕负载解码器
    /// </summary>
    public abstract class PayloadDecoder
    {
        public abstract ushort ProtocolVersion { get; }

        protected byte[] _decompressBuffer;

        protected PayloadDecoder() : this(new byte[4096])
        {

        }

        protected PayloadDecoder(byte[] decompressBuffer)
        {
            _decompressBuffer = decompressBuffer;
        }

        public abstract bool TryOpen(byte[] rawdata);

        /// <summary>
        /// 尝试解码数据
        /// </summary>
        /// <remarks>
        /// 解码成功时, 应返回一个完整的弹幕数据包, 即包头和包体
        /// </remarks>
        /// <param name="decodedRawdata">解码后的数据</param>
        /// <returns><see langword="true"/> 表示解码成功, caller 应当继续尝试; <see langword="false"/> 表示解码完成</returns>
        public abstract bool TryProcess([NotNullWhen(true)]out byte[]? decodedRawdata);

        public abstract void Close();
    }
}
