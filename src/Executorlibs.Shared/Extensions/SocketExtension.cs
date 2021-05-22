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
    }
}
