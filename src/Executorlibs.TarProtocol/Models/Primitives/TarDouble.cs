using System;
using System.Diagnostics;
using Executorlibs.TarProtocol.IO;

namespace Executorlibs.TarProtocol.Models.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] {Value}")]
    public struct TarDouble : ITarType
    {
        public TarHeader Header;

        public double Value;

        public TarDouble(byte tag, double value)
        {
            Header = new TarHeader(TarType.Double, tag);
            Value = value;
        }

        public void ReadFrom(ref TarReader reader)
        {
            Header = reader.ReadHeader();
            Value = Header.Type switch
            {
                TarType.Double => reader.ParseRawBigEndianDouble(),
                TarType.Zero => 0d,
                _ => throw new InvalidOperationException()
            };
        }

        public void WriteTo(ref TarWriter writer)
        {
            if (Value == 0d)
            {
                new TarZero(Header.Tag).WriteTo(ref writer);
                return;
            }
            writer.WriteHeader(Header);
            writer.WriteRawBigEndianDouble(Value);
        }

        public override bool Equals(object? obj)
        {
            return obj is TarDouble value && Equals(value);
        }

        public bool Equals(TarDouble value)
        {
            return value.Header == Header && value.Value == Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header, Value);
        }

        public static implicit operator double(TarDouble t)
        {
            return t.Value;
        }

        public static bool operator ==(TarDouble left, TarDouble right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TarDouble left, TarDouble right)
        {
            return !(left == right);
        }
    }
}
