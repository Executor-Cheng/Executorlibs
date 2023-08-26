using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Executorlibs.Bilibili.Protocol.Clients
{
    /// <summary>
    /// 表示 Bilibili 直播平台的弹幕协议头
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = (int)Size)]
    public struct DanmakuProtocol
    {
        /// <summary>
        /// 结构体大小
        /// </summary>
        public const uint Size = 16;

        /// <summary>
        /// 消息总长度 (协议头 + 数据长度)
        /// </summary>
        [FieldOffset(0)]
        public uint PacketLength;

        /// <summary>
        /// 消息头长度 (固定为16[sizeof(DanmakuProtocol)])
        /// </summary>
        [FieldOffset(4)]
        public ushort HeaderLength;

        /// <summary>
        /// 压缩标志
        /// </summary>
        [FieldOffset(4)]
        public ulong CompressedFlag;

        /// <summary>
        /// 消息版本号
        /// </summary>
        [FieldOffset(6)]
        public ushort Version;

        /// <summary>
        /// 消息类型
        /// </summary>
        [FieldOffset(8)]
        public uint Action;

        /// <summary>
        /// 参数, 固定为1
        /// </summary>
        [FieldOffset(12)]
        public uint Parameter;

        /// <summary>
        /// 在本机字节序为小端时, 翻转本结构体内所有成员的字节序
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ChangeEndian()
        {
            if (BitConverter.IsLittleEndian) // JIT 在本机字节序为大端下会对此优化成 nop
            {
                PacketLength = BinaryPrimitives.ReverseEndianness(PacketLength);
                HeaderLength = BinaryPrimitives.ReverseEndianness(HeaderLength);
                Version = BinaryPrimitives.ReverseEndianness(Version);
                Action = BinaryPrimitives.ReverseEndianness(Action);
                Parameter = BinaryPrimitives.ReverseEndianness(Parameter);
            }
        }
    }
}
