using System;
using System.Diagnostics;
using Executorlibs.TarsProtocol.IO;

namespace Executorlibs.TarsProtocol.Models.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] {Value}")]
    public struct TarsInt16 : ITarsType
    {
        public TarsHeader Header;

        public short Value;

        public TarsInt16(byte tag, short value)
        {
            Header = new TarsHeader(TarsType.Short, tag);
            Value = value;
        }

        public void ReadFrom(ref TarsReader reader)
        {
            Header = reader.ReadHeader();
            Value = Header.Type switch
            {
                TarsType.Short => (short)reader.ParseRawBigEndian16(),
                TarsType.Byte => reader.ReadRawByte(),
                TarsType.Zero => 0,
                _ => throw new InvalidOperationException()
            };
        }

        public void WriteTo(ref TarsWriter writer)
        {
            if (Value == 0)
            {
                new TarsZero(Header.Tag).WriteTo(ref writer);
            }
            else if (Value >= sbyte.MinValue && Value <= byte.MaxValue)
            {
                new TarsByte(Header.Tag, (byte)Value).WriteTo(ref writer);
            }
            else
            {
                writer.WriteHeader(Header);
                writer.WriteRawBigEndian16((ushort)Value);
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is TarsInt16 value && Equals(value);
        }

        public bool Equals(TarsInt16 value)
        {
            return value.Header == Header && value.Value == Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header, Value);
        }

        public static implicit operator short(TarsInt16 t)
        {
            return t.Value;
        }

        public static bool operator ==(TarsInt16 left, TarsInt16 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TarsInt16 left, TarsInt16 right)
        {
            return !(left == right);
        }
    }
}
