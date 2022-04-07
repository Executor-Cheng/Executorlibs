using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Executorlibs.TarProtocol.IO;

namespace Executorlibs.TarProtocol.Models.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] {Value}")]
    public unsafe struct TarInt64 : ITarType
    {
        public TarHeader Header;

        public long Value;

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TarInt64(byte tag, long value)
        {
            Header = new TarHeader(TarType.Long, tag);
            Value = value;
        }

        public void ReadFrom(ref TarReader reader)
        {
            Header = reader.ReadHeader();
            Value = Header.Type switch
            {
                TarType.Long => (long)reader.ParseRawBigEndian64(),
                TarType.Int => (int)reader.ParseRawBigEndian32(),
                TarType.Short => (short)reader.ParseRawBigEndian16(),
                TarType.Byte => reader.ReadRawByte(),
                TarType.Zero => 0,
                _ => throw new InvalidOperationException()
            };
        }

        public void WriteTo(ref TarWriter writer)
        {
            if (Value == 0)
            {
                new TarZero(Header.Tag).WriteTo(ref writer);
            }
            else if (Value >= sbyte.MinValue && Value <= byte.MaxValue)
            {
                new TarByte(Header.Tag, (byte)Value).WriteTo(ref writer);
            }
            else if (Value >= short.MinValue && Value <= short.MaxValue)
            {
                new TarInt16(Header.Tag, (short)Value).WriteTo(ref writer);
            }
            else if (Value >= int.MinValue && Value <= int.MaxValue)
            {
                new TarInt32(Header.Tag, (int)Value).WriteTo(ref writer);
            }
            else
            {
                writer.WriteHeader(Header);
                writer.WriteRawBigEndian64((ulong)Value);
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is TarInt64 value && Equals(value);
        }

        public bool Equals(TarInt64 value)
        {
            return value.Header == Header && value.Value == Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header, Value);
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
