#if !NETSTANDARD2_0
using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using Executorlibs.Bilibili.Protocol.Utility;

namespace Executorlibs.Bilibili.Protocol.Clients
{
    public class BrotliPayloadDecoder : PayloadDecoder
    {
        protected BrotliDecoder _decoder;

        protected ReadOnlyMemory<byte> _rawdata;

        protected bool _done;

        public override bool TryOpen(byte[] rawdata)
        {
            ref var protocol = ref DanmakuProtocolUtility.AsProtocol(rawdata);
            if (protocol.CompressedFlag == (BitConverter.IsLittleEndian ? 0x0000000500030010UL : 0x0500000003001000UL))
            {
                _decoder = default;
                _rawdata = rawdata.AsMemory((int)DanmakuProtocol.Size, (int)(protocol.PacketLength - DanmakuProtocol.Size));
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
            if (_done)
            {
                decodedRawdata = null;
                return false;
            }
            do
            {
                var rawdata = _rawdata.Span;
                var decompressBuffer = _decompressBuffer;
                var decompressSpan = decompressBuffer.AsSpan();
                var status = _decoder.Decompress(rawdata, decompressSpan.Slice(0, (int)DanmakuProtocol.Size), out int consumed, out int written);
                if ((status | (OperationStatus)(written >> 4)) == OperationStatus.DestinationTooSmall)
                {
                    // written 必定在 [0,16] 内
                    // 仅当 written = 16, 其 >> 4 = 1
                    // 仅 status = Done (0) 且 written = 16 (即没有后续数据)
                    // 或 status = DestinationTooSmall (1)
                    // 接收到的压缩负载才有效
                    // 其它情况按以上的 bitwise operation 都不会 = 1
                    ref var protocol = ref DanmakuProtocolUtility.AsProtocol(decompressSpan);
                    protocol.ChangeEndian();
                    uint packetLength = protocol.PacketLength;
                    if (packetLength > 65535)
                    {
                        break;
                    }
                    if (packetLength > 16)
                    {
                        if (packetLength > (uint)decompressBuffer.Length)
                        {
                            _decompressBuffer = decompressBuffer = new byte[packetLength];
                            decompressSpan = decompressBuffer;
                            ref var newProtocol = ref DanmakuProtocolUtility.AsProtocol(decompressBuffer);
                            newProtocol = protocol;
                        }
                        status = _decoder.Decompress(rawdata.Slice(consumed), decompressSpan.Slice((int)DanmakuProtocol.Size, (int)(packetLength - DanmakuProtocol.Size)), out written, out _);
                        consumed += written;
                        if (status > OperationStatus.DestinationTooSmall)
                        {
                            break;
                        }
                    }
                    _done = status == OperationStatus.Done;
                    _rawdata = _rawdata.Slice(consumed);
                    decodedRawdata = decompressBuffer;
                    return true;
                }
            }
            while (false);
            throw new InvalidDataException();
        }

        public override void Close()
        {
            _decoder.Dispose();
            _rawdata = default;
            _done = false;
        }
    }
}
#endif
