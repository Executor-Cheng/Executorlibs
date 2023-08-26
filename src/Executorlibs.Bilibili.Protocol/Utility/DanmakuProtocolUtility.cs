using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Executorlibs.Bilibili.Protocol.Clients;

namespace Executorlibs.Bilibili.Protocol.Utility
{
    public static class DanmakuProtocolUtility
    {
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
