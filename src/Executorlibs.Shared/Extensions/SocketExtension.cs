using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

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
        public static async ValueTask ReceiveFullyAsync(this Socket socket, Memory<byte> memory, CancellationToken token = default)
        {
            while (true)
            {
                int n = await socket.ReceiveAsync(memory, SocketFlags.None, token);
                if (n < 1)
                {
                    throw new SocketException(10054);
                }
                else if (n < memory.Length)
                {
                    memory = memory[n..];
                }
                else
                {
                    break;
                }
            }
        }
#else
        public static async ValueTask ReceiveFullyAsync(this Socket socket, Memory<byte> memory, CancellationToken token = default)
        {
            if (System.Runtime.InteropServices.MemoryMarshal.TryGetArray(memory, out ArraySegment<byte> segment))
            {
                int offset = segment.Offset, count = segment.Count;
                while (true)
                {
                    int n = await Task.Factory.FromAsync(socket.BeginReceive(segment.Array, offset, count, SocketFlags.None, null, null), socket.EndReceive);
                    if (n < 1)
                    {
                        throw new SocketException(10054);
                    }
                    else if (n < count)
                    {
                        offset += n;
                        count -= n;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            throw new NotSupportedException();
        }
#endif
    }
}
