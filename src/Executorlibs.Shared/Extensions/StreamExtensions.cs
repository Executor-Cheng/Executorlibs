using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Executorlibs.Shared.Extensions
{
    public static class StreamExtensions
    {
        public static ValueTask ReadFullyAsync(this Stream stream, byte[] buffer, CancellationToken token = default)
            => stream.ReadFullyAsync(buffer, 0, buffer.Length, token);

        public static async ValueTask ReadFullyAsync(this Stream stream, byte[] buffer, int offset, int size, CancellationToken token = default)
        {
            if (offset + size > buffer.Length)
            {
                throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
            }
            while (size > 0)
            {
#if !NETSTANDARD2_0
                int n = await stream.ReadAsync(new Memory<byte>(buffer, offset, size), token);
#else
                int n = await stream.ReadAsync(buffer, offset, size, token);
#endif
                if (n < 1)
                {
                    throw new EndOfStreamException();
                }
                offset += n;
                size -= n;
            }
        }
    }
}
