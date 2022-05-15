using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Executorlibs.TarProtocol.Models;

namespace Executorlibs.TarProtocol.IO
{
#if false
    public unsafe struct TarWriter : IDisposable
    {
        private const int BufferSize = 1024;

        private readonly GCHandle _handle;

        private readonly byte* _buffer;

        private readonly Stream? _underlyingStream;

        private readonly int _length;

        private readonly int _origin;

        private int _position;

        private byte[] ByteArray
        {
            get
            {
                object? array = _handle.Target;
                return Unsafe.As<object, byte[]>(ref array!);
            }
        }

        public TarWriter(Stream stream) : this(stream, BufferSize)
        {
            
        }

        public TarWriter(Stream stream, int bufferSize)
        {
            byte[] buffer = GC.AllocateUninitializedArray<byte>(bufferSize, true);
            _handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            _buffer = (byte*)Unsafe.AsPointer(ref MemoryMarshal.GetArrayDataReference(buffer));
            _underlyingStream = stream;
            _length = bufferSize;
            _origin = 0;
            _position = 0;
        }

        public TarWriter(byte[] buffer) : this(buffer, 0, buffer.Length)
        {
            
        }

        public TarWriter(byte[] buffer, int offset, int length)
        {
            _handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            _buffer = (byte*)Unsafe.AsPointer(ref MemoryMarshal.GetArrayDataReference(buffer)) + offset;
            _underlyingStream = null;
            _length = length;
            _origin = offset;
            _position = 0;
        }

        public void WriteHeader(TarHeader header)
        {
            if (_position + 2 > _length)
            {
                WriteHeaderSlowPath(header);
                return;
            }
            BinaryPrimitives.WriteUInt16LittleEndian(new Span<byte>(_buffer + _position, 2), header._rawTag);
            _position += header.ActualSize;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void WriteHeaderSlowPath(TarHeader header)
        {
            WriteRawByte((byte)header._rawTag);
            if (header.HasTag8)
            {
                WriteRawByte(header.Tag8);
            }
        }

        public void WriteRawByte(byte value)
        {
            if (_position == _length)
            {
                Flush();
            }
            *(_buffer + _position++) = value;
        }

        public void WriteRawBigEndian16(ushort value)
        {
            const int length = sizeof(ushort);
            if (_position + _length > _length)
            {
                WriteRawBigEndian16SlowPath(value);
                return;
            }
            BinaryPrimitives.WriteUInt16BigEndian(new Span<byte>(_buffer + _position, length), value);
            _position += length;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void WriteRawBigEndian16SlowPath(ushort value)
        {
            WriteRawByte((byte)(value >> 8));
            WriteRawByte((byte)value);
        }

        public void WriteRawBigEndian32(uint value)
        {
            const int length = sizeof(uint);
            if (_position + _length > _length)
            {
                WriteRawBigEndian32SlowPath(value);
                return;
            }
            BinaryPrimitives.WriteUInt32BigEndian(new Span<byte>(_buffer + _position, length), value);
            _position += length;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void WriteRawBigEndian32SlowPath(uint value)
        {
            WriteRawByte((byte)(value >> 24));
            WriteRawByte((byte)(value >> 16));
            WriteRawByte((byte)(value >> 8));
            WriteRawByte((byte)value);
        }

        public void WriteRawBigEndian64(ulong value)
        {
            const int length = sizeof(ulong);
            if (_position + _length > _length)
            {
                WriteRawBigEndian64SlowPath(value);
                return;
            }
            BinaryPrimitives.WriteUInt64BigEndian(new Span<byte>(_buffer + _position, length), value);
            _position += length;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void WriteRawBigEndian64SlowPath(ulong value)
        {
            WriteRawByte((byte)(value >> 56));
            WriteRawByte((byte)(value >> 48));
            WriteRawByte((byte)(value >> 40));
            WriteRawByte((byte)(value >> 32));
            WriteRawByte((byte)(value >> 24));
            WriteRawByte((byte)(value >> 16));
            WriteRawByte((byte)(value >> 8));
            WriteRawByte((byte)value);
        }

        public void WriteRawBigEndianSingle(float value)
        {
            WriteRawBigEndian32((uint)BitConverter.SingleToInt32Bits(value));
        }

        public void WriteRawBigEndianDouble(double value)
        {
            WriteRawBigEndian64((ulong)BitConverter.DoubleToInt64Bits(value));
        }

        public void WriteBytes(ArraySegment<byte> buffer)
        {
            WriteBytes(new Span<byte>(buffer.Array, buffer.Offset, buffer.Count));
        }

        public void WriteBytes(byte[] buffer)
        {
            WriteBytes(new Span<byte>(buffer, 0, buffer.Length));
        }

        public void WriteBytes(byte[] buffer, int length)
        {
            WriteBytes(buffer, 0, length);
        }

        public void WriteBytes(byte[] buffer, int offset, int length)
        {
            WriteBytes(new Span<byte>(buffer, offset, length));
        }

        public void WriteBytes(ReadOnlySpan<byte> value)
        {
            if (_length - _position >= value.Length)
            {
                value.CopyTo(new Span<byte>(_buffer + _position, value.Length));
                _position += value.Length;
                return;
            }
            int bytesWritten = 0;
            while (_length - _position < value.Length - bytesWritten)
            {
                int length = _length - _position;
                value.Slice(bytesWritten, length).CopyTo(new Span<byte>(_buffer + _position, length));
                bytesWritten += length;
                _position += length;
                Flush();
            }
            int remainderLength = value.Length - bytesWritten;
            value.Slice(bytesWritten, remainderLength).CopyTo(new Span<byte>(_buffer + _position, remainderLength));
            _position += remainderLength;
        }

        public void Flush()
        {
            if (_underlyingStream == null)
            {
                throw new InvalidOperationException();
            }
            _underlyingStream.Write(new ReadOnlySpan<byte>(_buffer, _position));
            _position = 0;
        }

        public void Dispose()
        {
            if (_handle.IsAllocated)
            {
                _handle.Free();
            }
        }
    }
#else
    public ref struct TarWriter
    {
        private const int BufferSize = 1024;

        private readonly Span<byte> _span;

        private readonly Stream? _underlyingStream;

        private int _position;

        private readonly int Length => _span.Length;

        private readonly ref byte First => ref MemoryMarshal.GetReference(_span);

        public TarWriter(Stream? stream) : this(stream, BufferSize)
        {

        }

        public TarWriter(Stream? stream, int bufferSize) : this(GC.AllocateUninitializedArray<byte>(bufferSize), 0, bufferSize, stream)
        {

        }

        public TarWriter(byte[] buffer) : this(buffer, 0, buffer.Length)
        {

        }

        public TarWriter(byte[] buffer, int offset, int length) : this(buffer, offset, length, null)
        {

        }

        private TarWriter(byte[] buffer, int offset, int length, Stream? underlyingStream)
        {
            _span = new Span<byte>(buffer, offset, length);
            _underlyingStream = underlyingStream;
            _position = 0;
        }

        public void WriteHeader(TarHeader header)
        {
            if (_position + 4 > Length)
            {
                WriteHeaderSlowPath(header);
                return;
            };
            BinaryPrimitives.WriteUInt32LittleEndian(MemoryMarshal.CreateSpan(ref Unsafe.Add(ref First, _position), 4), header._rawTag);
            _position += header.ActualSize;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void WriteHeaderSlowPath(TarHeader header)
        {
            WriteRawByte((byte)header._rawTag);
            if (header.HasTag8)
            {
                WriteRawByte(header.Tag8);
            }
        }

        public void WriteRawByte(byte value)
        {
            if (_position == Length)
            {
                Flush();
            }
            Unsafe.Add(ref First, _position++) = value;
        }

        public void WriteRawBigEndian16(ushort value)
        {
            const int length = sizeof(ushort);
            if (_position + Length > Length)
            {
                WriteRawBigEndian16SlowPath(value);
                return;
            }
            BinaryPrimitives.WriteUInt16BigEndian(MemoryMarshal.CreateSpan(ref Unsafe.Add(ref First, _position), length), value);
            _position += length;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void WriteRawBigEndian16SlowPath(ushort value)
        {
            WriteRawByte((byte)(value >> 8));
            WriteRawByte((byte)value);
        }

        public void WriteRawBigEndian32(uint value)
        {
            const int length = sizeof(uint);
            if (_position + Length > Length)
            {
                WriteRawBigEndian32SlowPath(value);
                return;
            }
            BinaryPrimitives.WriteUInt32BigEndian(MemoryMarshal.CreateSpan(ref Unsafe.Add(ref First, _position), length), value);
            _position += length;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void WriteRawBigEndian32SlowPath(uint value)
        {
            WriteRawByte((byte)(value >> 24));
            WriteRawByte((byte)(value >> 16));
            WriteRawByte((byte)(value >> 8));
            WriteRawByte((byte)value);
        }

        public void WriteRawBigEndian64(ulong value)
        {
            const int length = sizeof(ulong);
            if (_position + this.Length > this.Length)
            {
                WriteRawBigEndian64SlowPath(value);
                return;
            }
            BinaryPrimitives.WriteUInt64BigEndian(MemoryMarshal.CreateSpan(ref Unsafe.Add(ref First, _position), length), value);
            _position += length;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void WriteRawBigEndian64SlowPath(ulong value)
        {
            WriteRawByte((byte)(value >> 56));
            WriteRawByte((byte)(value >> 48));
            WriteRawByte((byte)(value >> 40));
            WriteRawByte((byte)(value >> 32));
            WriteRawByte((byte)(value >> 24));
            WriteRawByte((byte)(value >> 16));
            WriteRawByte((byte)(value >> 8));
            WriteRawByte((byte)value);
        }

        public void WriteRawBigEndianSingle(float value)
        {
            WriteRawBigEndian32((uint)BitConverter.SingleToInt32Bits(value));
        }

        public void WriteRawBigEndianDouble(double value)
        {
            WriteRawBigEndian64((ulong)BitConverter.DoubleToInt64Bits(value));
        }

        public void WriteBytes(ArraySegment<byte> buffer)
        {
            WriteBytes(new ReadOnlySpan<byte>(buffer.Array, buffer.Offset, buffer.Count));
        }

        public void WriteBytes(byte[] buffer)
        {
            WriteBytes(new ReadOnlySpan<byte>(buffer, 0, buffer.Length));
        }

        public void WriteBytes(byte[] buffer, int length)
        {
            WriteBytes(buffer, 0, length);
        }

        public void WriteBytes(byte[] buffer, int offset, int length)
        {
            WriteBytes(new ReadOnlySpan<byte>(buffer, offset, length));
        }

        public void WriteBytes(ReadOnlySpan<byte> value)
        {
            if (Length - _position >= value.Length)
            {
                value.CopyTo(MemoryMarshal.CreateSpan(ref Unsafe.Add(ref First, _position), value.Length));
                _position += value.Length;
                return;
            }
            int bytesWritten = 0;
            while (Length - _position < value.Length - bytesWritten)
            {
                int length = this.Length - _position;
                MemoryMarshal.CreateSpan(ref Unsafe.Add(ref MemoryMarshal.GetReference(value), bytesWritten), length).CopyTo(MemoryMarshal.CreateSpan(ref Unsafe.Add(ref First, _position), length));
                bytesWritten += length;
                _position += length;
                Flush();
            }
            int remainderLength = value.Length - bytesWritten;
            MemoryMarshal.CreateSpan(ref Unsafe.Add(ref MemoryMarshal.GetReference(value), bytesWritten), remainderLength).CopyTo(MemoryMarshal.CreateSpan(ref Unsafe.Add(ref First, _position), remainderLength));
            _position += remainderLength;
        }

        public void Flush()
        {
            if (_underlyingStream == null)
            {
                throw new InvalidOperationException();
            }
            _underlyingStream.Write(MemoryMarshal.CreateReadOnlySpan(ref First, _position));
            _position = 0;
        }
    }
#endif
}
