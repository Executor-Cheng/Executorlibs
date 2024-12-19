using System;
using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Executorlibs.TarsProtocol.Exceptions;
using Executorlibs.TarsProtocol.Models;

namespace Executorlibs.TarsProtocol.IO
{
    public ref struct TarsReader
    {
        private readonly byte[] _buffer;

        private readonly ReadOnlySpan<byte> _span;

        private readonly int _origin;

        private nint _position;

        private readonly nint _length;

        private readonly ref byte First => ref MemoryMarshal.GetReference(_span);

        public TarsReader(ArraySegment<byte> buffer) : this(buffer.Array!, buffer.Offset, buffer.Count)
        {

        }

        public TarsReader(byte[] buffer) : this(buffer, 0, buffer.Length)
        {

        }

        public TarsReader(byte[] buffer, int offset, int length)
        {
            _buffer = buffer;
            _span = buffer.AsSpan().Slice(offset, length);
            _length = length;
            _position = 0;
            _origin = offset;
        }

        public TarsHeader PeekHeader()
        {
            if (!TryPeekHeader(out TarsHeader header))
            {
                ThrowMalformedMessage();
            }
            return header;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryPeekHeader(out TarsHeader header)
        {
            const int length = sizeof(ushort);
            if (_position + length > _length)
            {
                return TryPeekHeaderSlowPath(out header);
            }
            header = new TarsHeader(BinaryPrimitives.ReadUInt16LittleEndian(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.Add(ref First, _position), length)));
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryPeekHeaderSlowPath(out TarsHeader header)
        {
            Unsafe.SkipInit(out header);
            if (!TryPeekRawByte(out byte tagAndType))
            {
                return false;
            }
            header._rawTag = tagAndType;
            return !header.HasTag8; // 不可能有 Tag8
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryReadHeader(out TarsHeader header)
        {
            if (!TryPeekHeader(out header))
            {
                return false;
            }
            _position += header.ActualSize;
            return true;
        }

        public TarsHeader ReadHeader()
        {
            if (!TryReadHeader(out TarsHeader header))
            {
                ThrowMalformedMessage();
            }
            return header;
        }

        public ushort ParseRawLittleEndian16()
        {
            const int length = sizeof(ushort);
            if (_position + length > _length)
            {
                ThrowMalformedMessage();
            }
            ushort result = BinaryPrimitives.ReadUInt16LittleEndian(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.Add(ref First, _position), length));
            _position += length;
            return result;
        }

        public ushort ParseRawBigEndian16()
        {
            const int length = sizeof(ushort);
            if (_position + length > _length)
            {
                ThrowMalformedMessage();
            }
            ushort result = BinaryPrimitives.ReadUInt16BigEndian(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.Add(ref First, _position), length));
            _position += length;
            return result;
        }

        public uint ParseRawBigEndian32()
        {
            const int length = sizeof(uint);
            if (_position + length > _length)
            {
                ThrowMalformedMessage();
            }
            uint result = BinaryPrimitives.ReadUInt32BigEndian(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.Add(ref First, _position), length));
            _position += length;
            return result;
        }

        public ulong ParseRawBigEndian64()
        {
            const int length = sizeof(ulong);
            if (_position + length > _length)
            {
                ThrowMalformedMessage();
            }
            ulong result = BinaryPrimitives.ReadUInt64BigEndian(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.Add(ref First, _position), length));
            _position += length;
            return result;
        }

        public float ParseRawBigEndianSingle()
        {
            const int length = sizeof(float);
            if (_position + length > _length)
            {
                ThrowMalformedMessage();
            }
            float result = BinaryPrimitives.ReadSingleBigEndian(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.Add(ref First, _position), length));
            _position += length;
            return result;
        }

        public double ParseRawBigEndianDouble()
        {
            const int length = sizeof(double);
            if (_position + length > _length)
            {
                ThrowMalformedMessage();
            }
            double result = BinaryPrimitives.ReadDoubleBigEndian(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.Add(ref First, _position), length));
            _position += length;
            return result;
        }

        public ulong ReadVarInt()
        {
            TarsHeader header = ReadHeader();
            if (header.Type > TarsType.Long)
            {
                if (header.Type == TarsType.Zero)
                {
                    return 0u;
                }
                ThrowTypeMismatch(TarsType.Int, header.Type);
            }
            int length = 1 << (int)header.Type;
            if (_position + length > _length)
            {
                ThrowMalformedMessage();
            }
            ref byte current = ref Unsafe.Add(ref First, _position);
            ulong result = length switch
            {
                1 => current,
                2 => BinaryPrimitives.ReadUInt16BigEndian(MemoryMarshal.CreateSpan(ref current, sizeof(ushort))),
                3 => BinaryPrimitives.ReadUInt32BigEndian(MemoryMarshal.CreateSpan(ref current, sizeof(uint))),
                4 => BinaryPrimitives.ReadUInt64BigEndian(MemoryMarshal.CreateSpan(ref current, sizeof(ulong))),
                _ => 0
            };
            _position += length;
            return result;
        }

        public byte ReadRawByte()
        {
            if (_position == _length)
            {
                ThrowMalformedMessage();
            }
            return Unsafe.Add(ref First, _position++);
        }

        public bool TryReadRawByte(out byte value)
        {
            if (_position == _length)
            {
                value = default;
                return false;
            }
            value = Unsafe.Add(ref First, _position);
            _position++;
            return true;
        }

        public byte PeekByte()
        {
            if (_position == _length)
            {
                ThrowMalformedMessage();
            }
            return Unsafe.Add(ref First, _position);
        }

        public bool TryPeekRawByte(out byte value)
        {
            if (_position == _length)
            {
                value = default;
                return false;
            }
            value = Unsafe.Add(ref First, _position);
            return true;
        }

        public ArraySegment<byte> ReadBytes(int length)
        {
            if (_position + length > _length)
            {
                ThrowMalformedMessage();
            }
            ArraySegment<byte> segment = new ArraySegment<byte>(_buffer, (int)_position + _origin, length);
            _position += length;
            return segment;
        }

        public void SkipRawBytes(int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }
            if (_position + length > _length)
            {
                ThrowMalformedMessage();
            }
            _position += length;
        }

        public void SkipLastField(TarsType type)
        {
            if (type == TarsType.Zero)
            {
                return;
            }
            if (type <= TarsType.Long)
            {
                SkipRawBytes(1 << (int)type);
                return;
            }
            if (type <= TarsType.Double)
            {
                SkipRawBytes(4 << (int)type);
                return;
            }
            switch (type)
            {
                case TarsType.String1:
                    {
                        uint length = ReadRawByte();
                        SkipRawBytes((int)length);
                        break;
                    }
                case TarsType.String4:
                    {
                        uint length = ParseRawBigEndian32();
                        SkipRawBytes((int)length);
                        break;
                    }
                case TarsType.Map:
                    {
                        ulong length = ReadVarInt();
                        for (ulong i = 0; i < length; i++)
                        {
                            SkipGroup();
                            SkipGroup();
                        }
                        break;
                    }
                case TarsType.List:
                    {
                        ulong length = ReadVarInt();
                        for (ulong i = 0; i < length; i++)
                        {
                            SkipGroup();
                        }
                        break;
                    }
                case TarsType.StructBegin:
                    {
                        SkipGroup();
                        break;
                    }
                case TarsType.StructEnd:
                    {
                        ThrowMalformedMessage();
                        break;
                    }
                case TarsType.SimpleList:
                    {
                        ReadRawByte();
                        ulong length = ReadVarInt();
                        SkipRawBytes((int)(uint)length);
                        break;
                    }
            }
        }

        public void SkipGroup()
        {
            while (true)
            {
                TarsHeader header = ReadHeader();
                if (header.Type == TarsType.StructEnd)
                {
                    break;
                }
                SkipLastField(header.Type);
            }
        }

        public bool IsDataAvailable(int size)
        {
            return size <= _length - _position;
        }

        public void ReadRawBytesIntoSpan(int length, Span<byte> byteSpan)
        {
            if (_position + length > _length)
            {
                ThrowMalformedMessage();
            }
            MemoryMarshal.CreateReadOnlySpan(ref Unsafe.Add(ref First, _position), length).CopyTo(byteSpan);
            _position += length;
        }

        public T Read<T>() where T : IReadableTarsType, new()
        {
            if (typeof(T).IsValueType)
            {
                Unsafe.SkipInit(out T value);
                value.ReadFrom(ref this);
                return value;
            }
            T obj = new T();
            obj.ReadFrom(ref this);
            return obj;
        }

        [DoesNotReturn]
        public static void ThrowMalformedMessage()
        {
            throw new MalformedTarsMessageException();
        }

        [DoesNotReturn]
        public static void ThrowTypeMismatch(TarsType expected, TarsType given)
        {
            throw new TarsTypeMismatchException(expected, given);
        }
    }
}
