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
        public static async ValueTask ReceiveFullyAsync(this WebSocket ws, Memory<byte> buffer, CancellationToken token = default)
        {
            while (true)
            {
                ValueWebSocketReceiveResult result = await ws.ReceiveAsync(buffer, token);
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

        public static async ValueTask<byte[]> ReceiveFullyAsync(this WebSocket webSocket, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            byte[] buffer = new byte[1024];
            using MemoryStream ms = new MemoryStream(1024);
            ValueWebSocketReceiveResult result;
            do
            {
                result = await webSocket.ReceiveAsync(buffer.AsMemory(), token);
                ms.Write(buffer, 0, result.Count);
            }
            while (!result.EndOfMessage);
            ms.Write(buffer, 0, result.Count);
            return ms.ToArray();
        }
#else
        public static async ValueTask ReceiveFullyAsync(this WebSocket ws, Memory<byte> buffer, CancellationToken token = default)
        {
            if (MemoryMarshal.TryGetArray(buffer, out ArraySegment<byte> segment))
            {
                while (true)
                {
                    WebSocketReceiveResult result = await ws.ReceiveAsync(segment, token);
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

        public static async ValueTask<byte[]> ReceiveFullyAsync(this WebSocket webSocket, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            byte[] buffer = new byte[1024];
            using MemoryStream ms = new MemoryStream(1024);
            WebSocketReceiveResult result;
            do
            {
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), token);
                ms.Write(buffer, 0, result.Count);
            }
            while (!result.EndOfMessage);
            ms.Write(buffer, 0, result.Count);
            return ms.ToArray();
        }

        public static ValueTask SendAsync(this WebSocket webSocket, ReadOnlyMemory<byte> memory, WebSocketMessageType messageType, bool endOfMessage, CancellationToken token)
        {
            if (MemoryMarshal.TryGetArray(memory, out ArraySegment<byte> segment))
            {
                return new ValueTask(webSocket.SendAsync(segment, messageType, endOfMessage, token));
            }
            return new ValueTask(Task.FromException(new NotSupportedException()));
        }
#endif
    }
}
