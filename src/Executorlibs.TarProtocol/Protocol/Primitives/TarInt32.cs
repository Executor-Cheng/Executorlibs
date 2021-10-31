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
    public unsafe struct TarInt32 : ITarType
    {
        [FieldOffset(0)]
        public fixed byte _data[6];

        [FieldOffset(0)]
        public TarHeader Header;

        [FieldOffset(2)]
        public int Value;

        public TarInt32(byte tag, int value)
        {
            Header = new TarHeader
            {
                Type = TarHeader.TarType.Int,
                Tag = tag
            };
            Value = value;
        }

        public void ReadFrom(TarStream stream)
        {
            Span<byte> thisSpan = MemoryMarshal.CreateSpan(ref _data[0], sizeof(TarInt32));
            stream.PassiveReadHeader(thisSpan);
            switch (Header.Type)
            {
                case TarHeader.TarType.Int:
                    {
                        stream.Read(thisSpan[2..6]);
#if !BIGENDIAN
                        Value = BinaryPrimitives.ReverseEndianness(Value);
#endif
                        break;
                    }
                case TarHeader.TarType.Short:
                    {
                        // !ww xx! yy zz  => BE
                        //  zz yy !xx ww! => LE
#if BIGENDIAN
                        stream.Read(thisSpan[4..6]);
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
            else
            {
                stream.WriteHeader(ref Header);
#if BIGENDIAN
                stream.Write(MemoryMarshal.CreateSpan(ref Unsafe.As<int, byte>(ref Data), sizeof(int)));
#else
                int be = BinaryPrimitives.ReverseEndianness(Value);
                stream.Write(source: MemoryMarshal.CreateSpan(ref Unsafe.As<int, byte>(ref be), sizeof(int)));
#endif
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is TarInt32 t && MemoryMarshal.CreateReadOnlySpan(ref _data[0], sizeof(TarInt32))
                .SequenceEqual(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<TarInt32, byte>(ref t), sizeof(TarInt32)));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header.Data, Value);
        }

        public static implicit operator int(TarInt32 t)
        {
            return t.Value;
        }

        public static bool operator ==(TarInt32 left, TarInt32 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TarInt32 left, TarInt32 right)
        {
            return !(left == right);
        }
    }
}
