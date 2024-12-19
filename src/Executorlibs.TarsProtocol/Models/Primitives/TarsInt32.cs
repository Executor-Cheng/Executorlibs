using System;
using System.Diagnostics;
using Executorlibs.TarsProtocol.IO;

namespace Executorlibs.TarsProtocol.Models.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] {Value}")]
    public struct TarsInt32 : ITarsType
    {
        public TarsHeader Header;

        public int Value;

        public TarsInt32(byte tag, int value)
        {
            Header = new TarsHeader(TarsType.Int, tag);
            Value = value;
        }

        public void ReadFrom(ref TarsReader reader)
        {
            Header = reader.ReadHeader();
            Value = Header.Type switch
            {
                TarsType.Int => (int)reader.ParseRawBigEndian32(),
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
            else if (Value >= short.MinValue && Value <= short.MaxValue)
            {
                new TarsInt16(Header.Tag, (short)Value).WriteTo(ref writer);
            }
            else
            {
                writer.WriteHeader(Header);
                writer.WriteRawBigEndian32((uint)Value);
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is TarsInt32 value && Equals(value);
        }

        public bool Equals(TarsInt32 value)
        {
            return value.Header == Header && value.Value == Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header, Value);
        }

        public static implicit operator int(TarsInt32 t)
        {
            return t.Value;
        }

        public static bool operator ==(TarsInt32 left, TarsInt32 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TarsInt32 left, TarsInt32 right)
        {
            return !(left == right);
        }
    }
}
