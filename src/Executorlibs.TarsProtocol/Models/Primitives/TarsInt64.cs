using System;
using System.Diagnostics;
using Executorlibs.TarsProtocol.IO;

namespace Executorlibs.TarsProtocol.Models.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] {Value}")]
    public unsafe struct TarsInt64 : ITarsType
    {
        public TarsHeader Header;

        public long Value;

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TarsInt64(byte tag, long value)
        {
            Header = new TarsHeader(TarsType.Long, tag);
            Value = value;
        }

        public void ReadFrom(ref TarsReader reader)
        {
            Header = reader.ReadHeader();
            Value = Header.Type switch
            {
                TarsType.Long => (long)reader.ParseRawBigEndian64(),
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
            else if (Value >= int.MinValue && Value <= int.MaxValue)
            {
                new TarsInt32(Header.Tag, (int)Value).WriteTo(ref writer);
            }
            else
            {
                writer.WriteHeader(Header);
                writer.WriteRawBigEndian64((ulong)Value);
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is TarsInt64 value && Equals(value);
        }

        public bool Equals(TarsInt64 value)
        {
            return value.Header == Header && value.Value == Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header, Value);
        }

        public static implicit operator long(TarsInt64 t)
        {
            return t.Value;
        }

        public static bool operator ==(TarsInt64 left, TarsInt64 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TarsInt64 left, TarsInt64 right)
        {
            return !(left == right);
        }
    }
}
