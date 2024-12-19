using System;
using System.Diagnostics;
using Executorlibs.TarsProtocol.IO;

namespace Executorlibs.TarsProtocol.Models.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] {Value}")]
    public struct TarsDouble : ITarsType
    {
        public TarsHeader Header;

        public double Value;

        public TarsDouble(byte tag, double value)
        {
            Header = new TarsHeader(TarsType.Double, tag);
            Value = value;
        }

        public void ReadFrom(ref TarsReader reader)
        {
            Header = reader.ReadHeader();
            Value = Header.Type switch
            {
                TarsType.Double => reader.ParseRawBigEndianDouble(),
                TarsType.Zero => 0d,
                _ => throw new InvalidOperationException()
            };
        }

        public void WriteTo(ref TarsWriter writer)
        {
            if (Value == 0d)
            {
                new TarsZero(Header.Tag).WriteTo(ref writer);
                return;
            }
            writer.WriteHeader(Header);
            writer.WriteRawBigEndianDouble(Value);
        }

        public override bool Equals(object? obj)
        {
            return obj is TarsDouble value && Equals(value);
        }

        public bool Equals(TarsDouble value)
        {
            return value.Header == Header && value.Value == Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header, Value);
        }

        public static implicit operator double(TarsDouble t)
        {
            return t.Value;
        }

        public static bool operator ==(TarsDouble left, TarsDouble right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TarsDouble left, TarsDouble right)
        {
            return !(left == right);
        }
    }
}
