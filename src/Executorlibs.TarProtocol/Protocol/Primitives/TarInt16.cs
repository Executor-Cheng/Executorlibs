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
    public unsafe struct TarInt16 : ITarType
    {
        [FieldOffset(0)]
        public fixed byte _data[4];

        [FieldOffset(0)]
        public TarHeader Header;

        [FieldOffset(2)]
        public short Value;

        public TarInt16(byte tag, short value)
        {
            Header = new TarHeader
            {
                Type = TarHeader.TarType.Short,
                Tag = tag
            };
            Value = value;
        }

        public void ReadFrom(TarStream stream)
        {
            Span<byte> thisSpan = MemoryMarshal.CreateSpan(ref _data[0], sizeof(TarInt16));
            stream.PassiveReadHeader(thisSpan);
            switch (Header.Type)
            {
                case TarHeader.TarType.Short:
                    {
                        stream.Read(thisSpan[2..4]);
#if !BIGENDIAN
                        Value = BinaryPrimitives.ReverseEndianness(Value);
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
            else
            {
                stream.WriteHeader(ref Header);
#if BIGENDIAN
                stream.Write(MemoryMarshal.CreateSpan(ref Unsafe.As<short, byte>(ref Data), sizeof(short)));
#else
                short be = BinaryPrimitives.ReverseEndianness(Value);
                stream.Write(source: MemoryMarshal.CreateSpan(ref Unsafe.As<short, byte>(ref be), sizeof(short)));
#endif
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is TarInt16 t && MemoryMarshal.CreateReadOnlySpan(ref _data[0], sizeof(TarInt16))
                .SequenceEqual(MemoryMarshal.CreateReadOnlySpan(ref _data[0], sizeof(TarInt16)));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header.Data, Value);
        }

        public static implicit operator short(TarInt16 t)
        {
            return t.Value;
        }

        public static bool operator ==(TarInt16 left, TarInt16 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TarInt16 left, TarInt16 right)
        {
            return !(left == right);
        }
    }
}
