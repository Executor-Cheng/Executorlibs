using System;
using System.Diagnostics;
using Executorlibs.TarsProtocol.IO;

namespace Executorlibs.TarsProtocol.Models.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] {Value}")]
    public struct TarsFloat : ITarsType
    {
        public TarsHeader Header;

        public float Value;

        public TarsFloat(byte tag, float value)
        {
            Header = new TarsHeader(TarsType.Float, tag);
            Value = value;
        }

        public void ReadFrom(ref TarsReader reader)
        {
            Header = reader.ReadHeader();
            Value = Header.Type switch
            {
                TarsType.Double => reader.ParseRawBigEndianSingle(),
                TarsType.Zero => 0f,
                _ => throw new InvalidOperationException()
            };
        }

        public void WriteTo(ref TarsWriter writer)
        {
            if (Value == 0f)
            {
                new TarsZero(Header.Tag).WriteTo(ref writer);
                return;
            }
            writer.WriteHeader(Header);
            writer.WriteRawBigEndianSingle(Value);
        }

        public override bool Equals(object? obj)
        {
            return obj is TarsFloat value && Equals(value);
        }

        public bool Equals(TarsFloat value)
        {
            return value.Header == Header && value.Value == Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header, Value);
        }

        public static implicit operator float(TarsFloat t)
        {
            return t.Value;
        }

        public static bool operator ==(TarsFloat left, TarsFloat right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TarsFloat left, TarsFloat right)
        {
            return !(left == right);
        }
    }
}
