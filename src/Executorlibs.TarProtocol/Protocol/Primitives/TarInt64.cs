using System;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Executorlibs.TarProtocol.IO;

namespace Executorlibs.TarProtocol.Protocol.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] {Value}")]
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct TarInt64 : ITarType
    {
        [FieldOffset(0)]
        public fixed byte _data[10];

        [FieldOffset(0)]
        public TarHeader Header;

        [FieldOffset(2)]
        public long Value;

        public TarInt64(byte tag, long value)
        {
            Header = new TarHeader
            {
                Type = TarHeader.TarType.Long,
                Tag = tag
            };
            Value = value;
        }

        public void ReadFrom(TarStream stream)
        {
            Span<byte> thisSpan = MemoryMarshal.CreateSpan(ref _data[0], sizeof(TarInt64));
            stream.PassiveReadHeader(thisSpan);
            switch (Header.Type)
            {
                case TarHeader.TarType.Long:
                    {
                        stream.Read(thisSpan[2..10]);
#if !BIGENDIAN
                        Value = BinaryPrimitives.ReverseEndianness(Value);
#endif
                        break;
                    }
                case TarHeader.TarType.Int:
                    {
                        // !aa bb cc dd! ee ff gg hh  => BE
                        //  hh gg ff ee !dd cc bb aa! => LE
#if BIGENDIAN
                        stream.Read(thisSpan[6..10]);
#else
                        stream.Read(thisSpan[2..6]);
                        Value = BinaryPrimitives.ReverseEndianness((int)Value);
#endif
                        break;
                    }
                case TarHeader.TarType.Short:
                    {
                        
#if BIGENDIAN
                        stream.Read(thisSpan[8..10]);
#else
                        stream.Read(thisSpan[2..4]);
                        Value = BinaryPrimitives.ReverseEndianness((short)Value);
#endif
                        break;
                    }
                case TarHeader.TarType.Byte:
                    {
                        Value = (byte)stream.ReadByte();
                        break;
                    }
                case TarHeader.TarType.Zero:
                    {
                        Value = 0;
                        break;
                    }
                default:
                    {
                        throw new InvalidOperationException();
                    }
            }
        }

        public void WriteTo(TarStream stream)
        {
            if (Value == 0)
            {
                new TarZero(Header.Tag).WriteTo(stream);
            }
            else if (Value >= sbyte.MinValue && Value <= byte.MaxValue)
            {
                new TarByte(Header.Tag, (byte)Value).WriteTo(stream);
            }
            else if (Value >= short.MinValue && Value <= short.MaxValue)
            {
                new TarInt16(Header.Tag, (short)Value).WriteTo(stream);
            }
            else if (Value >= int.MinValue && Value <= int.MaxValue)
            {
                new TarInt32(Header.Tag, (int)Value).WriteTo(stream);
            }
            else
            {
                stream.WriteHeader(ref Header);
#if BIGENDIAN
                stream.Write(MemoryMarshal.CreateSpan(ref Unsafe.As<long, byte>(ref Data), sizeof(long)));
#else
                long be = BinaryPrimitives.ReverseEndianness(Value);
                stream.Write(source: MemoryMarshal.CreateSpan(ref Unsafe.As<long, byte>(ref be), sizeof(long)));
#endif
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is TarInt64 t && MemoryMarshal.CreateReadOnlySpan(ref _data[0], sizeof(TarInt64))
                .SequenceEqual(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<TarInt64, byte>(ref t), sizeof(TarInt64)));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header.Data, Value);
        }

        public static implicit operator long(TarInt64 t)
        {
            return t.Value;
        }

        public static bool operator ==(TarInt64 left, TarInt64 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TarInt64 left, TarInt64 right)
        {
            return !(left == right);
        }
    }
}
