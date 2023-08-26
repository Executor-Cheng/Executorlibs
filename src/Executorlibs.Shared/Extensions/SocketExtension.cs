using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
#if NETSTANDARD2_0
using System.Runtime.InteropServices;
#endif

namespace Executorlibs.Shared.Extensions
{
    public static class SocketExtension
    {
        public static ValueTask ReceiveFullyAsync(this Socket socket, byte[] buffer, SocketFlags flags, CancellationToken token = default)
        {
            return socket.ReceiveFullyAsync(new Memory<byte>(buffer, 0, buffer.Length), flags, token);
        }

        public static ValueTask ReceiveFullyAsync(this Socket socket, byte[] buffer, int offset, int size, SocketFlags flags, CancellationToken token = default)
        {
            var memory = new Memory<byte>(buffer, offset, size);
            return socket.ReceiveFullyAsync(memory, flags, token);
        }

#if !NETSTANDARD2_0
#pragma warning disable CA2012 // Use ValueTasks correctly
        public static ValueTask ReceiveFullyAsync(this Socket socket, Memory<byte> memory, SocketFlags flags, CancellationToken token = default)
        {
            var vt = socket.ReceiveAsync(memory, flags, token);
            if (vt.IsCompletedSuccessfully)
            {
                int recved = vt.Result;
                if (recved == memory.Length)
                {
                    return default;
                }
                vt = new ValueTask<int>(recved);
            }
            return Await(socket, memory, vt, flags, token);

            static async ValueTask Await(Socket socket, Memory<byte> memory, ValueTask<int> recvTask, SocketFlags flags, CancellationToken token)
            {
                while (true)
                {
                    int n = await recvTask;
                    if (n == memory.Length)
                    {
                        return;
                    }
                    if (n < 1)
                    {
                        throw new SocketException((int)SocketError.ConnectionReset);
                    }
                    memory = memory[n..];
                    recvTask = socket.ReceiveAsync(memory, flags, token);
                }
            }
        }
#else
        public static async ValueTask ReceiveFullyAsync(this Socket socket, Memory<byte> memory, SocketFlags flags, CancellationToken token = default)
        {
            if (MemoryMarshal.TryGetArray(memory, out ArraySegment<byte> segment))
            {
                while (true)
                {
                    int n = await SocketTaskExtensions.ReceiveAsync(socket, segment, flags);
                    if (n == segment.Count)
                    {
                        return;
                    }
                    if (n < 1)
                    {
                        throw new SocketException((int)SocketError.ConnectionReset);
                    }
                    token.ThrowIfCancellationRequested();
                    segment = new ArraySegment<byte>(segment.Array, segment.Offset + n, segment.Count - n);
                }
            }
            throw new NotSupportedException();
        }

        public static ValueTask<int> SendAsync(this Socket socket, ReadOnlyMemory<byte> memory, SocketFlags flags, CancellationToken token = default)
        {
            if (MemoryMarshal.TryGetArray(memory, out ArraySegment<byte> segment))
            {
                return new ValueTask<int>(SocketTaskExtensions.SendAsync(socket, segment, flags));
            }
            throw new NotSupportedException();
        }
#endif
    }
}
