using System;
using System.IO;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
#if NETSTANDARD2_0
using System.Runtime.InteropServices;
#endif

namespace Executorlibs.Shared.Extensions
{
    public static class WebSocketExtensions
    {
#if !NETSTANDARD2_0
        public static async Task ReceiveFullyAsync(this WebSocket ws, Memory<byte> buffer, CancellationToken token = default)
        {
            while (true)
            {
                var result = await ws.ReceiveAsync(buffer, token);
                if (result.Count == buffer.Length)
                {
                    return;
                }
                if (result.EndOfMessage)
                {
                    throw new EndOfStreamException();
                }
                buffer = buffer[result.Count..];
            }
        }

        public static async Task<byte[]> ReceiveFullyAsync(this WebSocket webSocket, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            var buffer = new byte[1024];
            var ms = new MemoryStream(1024);
            while (true)
            {
                var result = await webSocket.ReceiveAsync(buffer.AsMemory(), token);
                ms.Write(buffer, 0, result.Count);
                if (result.EndOfMessage)
                {
                    return ms.ToArray();
                }
            }
        }
#else
        public static async Task ReceiveFullyAsync(this WebSocket ws, Memory<byte> buffer, CancellationToken token = default)
        {
            if (MemoryMarshal.TryGetArray(buffer, out ArraySegment<byte> segment))
            {
                while (true)
                {
                    var result = await ws.ReceiveAsync(segment, token);
                    if (result.Count == segment.Count)
                    {
                        return;
                    }
                    if (result.EndOfMessage)
                    {
                        throw new EndOfStreamException();
                    }
                    segment = new ArraySegment<byte>(segment.Array, segment.Offset + result.Count, segment.Count - result.Count);
                }
            }
            throw new NotSupportedException();
        }

        public static async Task<byte[]> ReceiveFullyAsync(this WebSocket webSocket, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            var buffer = new byte[1024];
            var ms = new MemoryStream(1024);
            while (true)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), token);
                ms.Write(buffer, 0, result.Count);
                if (result.EndOfMessage)
                {
                    return ms.ToArray();
                }
            }
        }

        public static Task SendAsync(this WebSocket webSocket, ReadOnlyMemory<byte> memory, WebSocketMessageType messageType, bool endOfMessage, CancellationToken token)
        {
            if (MemoryMarshal.TryGetArray(memory, out ArraySegment<byte> segment))
            {
                return webSocket.SendAsync(segment, messageType, endOfMessage, token);
            }
            return Task.FromException(new NotSupportedException());
        }
#endif
    }
}
