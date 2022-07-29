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
        public static ValueTask ReceiveFullyAsync(this Socket socket, byte[] buffer, CancellationToken token = default)
            => socket.ReceiveFullyAsync(new Memory<byte>(buffer, 0, buffer.Length), token);

        public static ValueTask ReceiveFullyAsync(this Socket socket, byte[] buffer, int offset, int size, CancellationToken token = default)
        {
            if (offset + size > buffer.Length)
            {
                throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
            }
            Memory<byte> memory = new Memory<byte>(buffer, offset, size);
            return socket.ReceiveFullyAsync(memory, token);
        }

#if !NETSTANDARD2_0
#pragma warning disable CA2012 // Use ValueTasks correctly
        public static ValueTask ReceiveFullyAsync(this Socket socket, Memory<byte> memory, CancellationToken token = default)
        {
            ValueTask<int> vt = socket.ReceiveAsync(memory, SocketFlags.None, token);
            if (vt.IsCompletedSuccessfully)
            {
                int recved = vt.Result;
                if (recved == memory.Length)
                {
                    return default;
                }
                vt = new ValueTask<int>(recved);
            }
            return Await(socket, memory, vt, token);

            static async ValueTask Await(Socket socket, Memory<byte> memory, ValueTask<int> recvTask, CancellationToken token)
            {
                while (true)
                {
                    int n = await recvTask;
                    if (n < 1)
                    {
                        throw new SocketException((int)SocketError.ConnectionReset);
                    }
                    else if (n == memory.Length)
                    {
                        return;
                    }
                    memory = memory[n..];
                    recvTask = socket.ReceiveAsync(memory, SocketFlags.None, token);
                }
            }
        }
#else
        public static async ValueTask ReceiveFullyAsync(this Socket socket, Memory<byte> memory, CancellationToken token = default)
        {
            if (MemoryMarshal.TryGetArray(memory, out ArraySegment<byte> segment))
            {
                int offset = segment.Offset, count = segment.Count;
                while (true)
                {
                    int n = await Task.Factory.FromAsync(socket.BeginReceive(segment.Array, offset, count, SocketFlags.None, null, null), socket.EndReceive);
                    if (n < 1)
                    {
                        throw new SocketException(10054);
                    }
                    else if (n == count)
                    {
                        return;
                    }
                    offset += n;
                    count -= n;
                }
            }
            throw new NotSupportedException();
        }

        public static ValueTask<int> SendAsync(this Socket socket, ReadOnlyMemory<byte> memory, SocketFlags socketFlags, CancellationToken token = default)
        {
            if (MemoryMarshal.TryGetArray(memory, out ArraySegment<byte> segment))
            {
                return new ValueTask<int>(Task.Factory.FromAsync(socket.BeginSend(segment.Array, segment.Offset, segment.Count, socketFlags, null, null), socket.EndSend));
            }
            throw new NotSupportedException();
        }
#endif
    }
}
