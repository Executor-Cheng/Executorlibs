using System;
using System.Diagnostics;
using Executorlibs.TarProtocol.IO;

namespace Executorlibs.TarProtocol.Models.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] {Value}")]
    public struct TarFloat : ITarType
    {
        public TarHeader Header;

        public float Value;

        public TarFloat(byte tag, float value)
        {
            Header = new TarHeader(TarType.Float, tag);
            Value = value;
        }

        public void ReadFrom(ref TarReader reader)
        {
            Header = reader.ReadHeader();
            Value = Header.Type switch
            {
                TarType.Double => reader.ParseRawBigEndianSingle(),
                TarType.Zero => 0f,
                _ => throw new InvalidOperationException()
            };
        }

        public void WriteTo(ref TarWriter writer)
        {
            if (Value == 0f)
            {
                new TarZero(Header.Tag).WriteTo(ref writer);
                return;
            }
            writer.WriteHeader(Header);
            writer.WriteRawBigEndianSingle(Value);
        }

        public override bool Equals(object? obj)
        {
            return obj is TarFloat value && Equals(value);
        }

        public bool Equals(TarFloat value)
        {
            return value.Header == Header && value.Value == Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header, Value);
        }

        public static implicit operator float(TarFloat t)
        {
            return t.Value;
        }

        public static bool operator ==(TarFloat left, TarFloat right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TarFloat left, TarFloat right)
        {
            return !(left == right);
        }
    }
}
