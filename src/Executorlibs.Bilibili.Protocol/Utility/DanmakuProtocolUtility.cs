using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Clients;

#pragma warning disable IDE0037 // Use inferred member name
namespace Executorlibs.Bilibili.Protocol.Utility
{
    public static class DanmakuProtocolUtility
    {
        public static readonly ReadOnlyMemory<byte> HeartBeatPacket = new byte[16] { 0, 0, 0, 16, 0, 16, 0, 2, 0, 0, 0, 2, 0, 0, 0, 1 };

        public static uint HandleEndianessAndGetPacketLength(byte[] recvBuffer)
        {
            ref var protocol = ref AsProtocol(recvBuffer);
            protocol.ChangeEndian();
            return protocol.PacketLength;
        }

        public static byte[] CreatePayload(uint action)
        {
            byte[] buffer = new byte[16];
            ref DanmakuProtocol protocol = ref AsProtocol(buffer);
            protocol.PacketLength = (uint)buffer.Length;
            protocol.Action = action;
            protocol.HeaderLength = 16;
            protocol.Parameter = 1;
            protocol.Version = 2;
            protocol.ChangeEndian();
            return buffer;
        }

        public static byte[] CreatePayload(uint action, string body)
        {
            var buffer = new byte[16 + Encoding.UTF8.GetByteCount(body)];
            var span = buffer.AsSpan();
            ref DanmakuProtocol protocol = ref AsProtocol(span);
            protocol.PacketLength = (uint)buffer.Length;
            protocol.Action = action;
            protocol.HeaderLength = 16;
            protocol.Parameter = 1;
            protocol.Version = 2;
            protocol.ChangeEndian();
#if NETSTANDARD2_0
            Encoding.UTF8.GetBytes(body, 0, body.Length, buffer, 16);
#else
            Encoding.UTF8.GetBytes(body, span[16..]);
#endif
            return buffer;
        }

        public static byte[] CreatePayload(uint action, byte[] body)
        {
            byte[] buffer = new byte[16 + body.Length];
            var span = buffer.AsSpan();
            ref DanmakuProtocol protocol = ref AsProtocol(span);
            protocol.PacketLength = (uint)buffer.Length;
            protocol.Action = action;
            protocol.HeaderLength = 16;
            protocol.Parameter = 1;
            protocol.Version = 2;
            protocol.ChangeEndian();
#if NET5_0_OR_GREATER
            Unsafe.CopyBlock(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), 16), ref MemoryMarshal.GetArrayDataReference(body), (uint)body.Length);
#else
            Unsafe.CopyBlock(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), 16), ref MemoryMarshal.GetReference(body.AsSpan()), (uint)body.Length);
#endif
            return buffer;
        }

        public static byte[] CreateJoinRoomPayload(byte version, uint roomId, ulong userId, string buvid, string platform, string token)
        {
            byte[] json = JsonSerializer.SerializeToUtf8Bytes(new
            {
                uid = userId,
                roomid = roomId,
                protover = version,
                buvid = buvid,
                platform = platform,
                type = 2,
                key = token
            });
            return CreatePayload(7, json);
        }

        public static ref DanmakuProtocol AsProtocol(byte[] buffer)
        {
#if NET5_0_OR_GREATER
            return ref AsProtocol(ref MemoryMarshal.GetArrayDataReference(buffer));
#else
            return ref AsProtocol(ref MemoryMarshal.GetReference(buffer.AsSpan()));
#endif
        }

        public static ref DanmakuProtocol AsProtocol(ReadOnlySpan<byte> buffer)
        {
            return ref AsProtocol(ref MemoryMarshal.GetReference(buffer));
        }

        public static ref DanmakuProtocol AsProtocol(ref byte head)
        {
            return ref Unsafe.As<byte, DanmakuProtocol>(ref head);
        }

        public static ReadOnlySpan<byte> GetBodySpan(byte[] buffer)
        {
#if !NETSTANDARD
            ref var head = ref MemoryMarshal.GetArrayDataReference(buffer);
            ref var protocol = ref AsProtocol(ref head);
            return MemoryMarshal.CreateReadOnlySpan(ref Unsafe.Add(ref head, (int)DanmakuProtocol.Size), (int)(protocol.PacketLength - DanmakuProtocol.Size));
#else
            var span = buffer.AsSpan();
            ref var head = ref MemoryMarshal.GetReference(span);
            ref var protocol = ref AsProtocol(ref head);
            return span.Slice(protocol.HeaderLength, (int)(protocol.PacketLength -  DanmakuProtocol.Size));
#endif
        }

        public static ReadOnlyMemory<byte> GetBody(byte[] buffer)
        {
#if !NETSTANDARD
            ref var head = ref MemoryMarshal.GetArrayDataReference(buffer);
#else
            var span = buffer.AsSpan();
            ref var head = ref MemoryMarshal.GetReference(span);
#endif
            ref var protocol = ref AsProtocol(ref head);
            return new ReadOnlyMemory<byte>(buffer, (int)DanmakuProtocol.Size, (int)(protocol.PacketLength - DanmakuProtocol.Size));
        }
    }
}
