using System;
using System.Diagnostics;
using Executorlibs.TarProtocol.IO;

namespace Executorlibs.TarProtocol.Models.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] {Value}")]
    public struct TarInt32 : ITarType
    {
        public TarHeader Header;

        public int Value;

        public TarInt32(byte tag, int value)
        {
            Header = new TarHeader(TarType.Int, tag);
            Value = value;
        }

        public void ReadFrom(ref TarReader reader)
        {
            Header = reader.ReadHeader();
            Value = Header.Type switch
            {
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
            else
            {
                writer.WriteHeader(Header);
                writer.WriteRawBigEndian32((uint)Value);
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is TarInt32 value && Equals(value);
        }

        public bool Equals(TarInt32 value)
        {
            return value.Header == Header && value.Value == Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header, Value);
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
