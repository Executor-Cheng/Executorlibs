using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using Executorlibs.Bilibili.Protocol.Utility;

namespace Executorlibs.Bilibili.Protocol.Clients
{
    public class DeflatePayloadDecoder : PayloadDecoder
    {
        protected DeflateStream? _deflater;

        public DeflatePayloadDecoder()
        {

        }

        public override bool TryOpen(byte[] rawdata)
        {
            ref var protocol = ref DanmakuProtocolUtility.AsProtocol(rawdata);
            if (protocol.CompressedFlag == (BitConverter.IsLittleEndian ? 0x0000000500020010UL : 0x0500000002001000UL))
            {
                var ms = new MemoryStream(rawdata, (int)(DanmakuProtocol.Size + 2), (int)(protocol.PacketLength - DanmakuProtocol.Size - 2)); // skip 0x78 0xDA
                _deflater = new DeflateStream(ms, CompressionMode.Decompress);
                return true;
            }
            return false;
        }
#if NETSTANDARD2_0
        public override bool TryProcess(out byte[]? decodedRawdata)
#else
        public override bool TryProcess([NotNullWhen(true)] out byte[]? decodedRawdata)
#endif
        {
            var deflater = _deflater ?? throw new InvalidOperationException("必须先执行打开操作");
            var decompressBuffer = _decompressBuffer;
            int consumed = deflater.Read(decompressBuffer, 0, 16);
            if (consumed == 0)
            {
                decodedRawdata = null;
                return false;
            }
            if (consumed != 16)
            {
                throw new InvalidDataException("无法读取协议头");
            }
            ref var protocol = ref DanmakuProtocolUtility.AsProtocol(decompressBuffer);
            protocol.ChangeEndian();
            uint packetLength = protocol.PacketLength;
            uint payloadLength = packetLength - DanmakuProtocol.Size;
            if (packetLength > 65535)
            {
                throw new InvalidDataException($"包长度过大:{packetLength}");
            }
            if (packetLength > (uint)decompressBuffer.Length)
            {
                _decompressBuffer = decompressBuffer = new byte[packetLength];
                ref var newProtocol = ref DanmakuProtocolUtility.AsProtocol(decompressBuffer);
                newProtocol = protocol;
            }
            if ((uint)deflater.Read(decompressBuffer, 16, (int)payloadLength) != payloadLength)
            {
                throw new InvalidDataException("无法读取消息数据");
            }
            decodedRawdata = decompressBuffer;
            return true;
        }

        public override void Close()
        {
            var deflate = _deflater ?? throw new InvalidOperationException("必须先执行打开操作");
            deflate.Dispose();
            _deflater = null;
        }
    }
}
