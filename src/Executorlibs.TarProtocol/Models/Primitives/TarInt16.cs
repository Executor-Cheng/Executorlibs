using System;
using System.Diagnostics;
using Executorlibs.TarProtocol.IO;

namespace Executorlibs.TarProtocol.Models.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] {Value}")]
    public struct TarInt16 : ITarType
    {
        public TarHeader Header;

        public short Value;

        public TarInt16(byte tag, short value)
        {
            Header = new TarHeader(TarType.Short, tag);
            Value = value;
        }

        public void ReadFrom(ref TarReader reader)
        {
            Header = reader.ReadHeader();
            Value = Header.Type switch
            {
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
            else
            {
                writer.WriteHeader(Header);
                writer.WriteRawBigEndian16((ushort)Value);
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is TarInt16 value && Equals(value);
        }

        public bool Equals(TarInt16 value)
        {
            return value.Header == Header && value.Value == Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header, Value);
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
