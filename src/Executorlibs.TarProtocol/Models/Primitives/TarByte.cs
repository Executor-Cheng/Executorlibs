using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Executorlibs.TarProtocol.IO;

namespace Executorlibs.TarProtocol.Models.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] {Value}")]
    public struct TarByte : ITarType
    {
        public TarHeader Header;

        public byte Value;

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TarByte(byte tag, byte value)
        {
            Header = new TarHeader(TarType.Byte, tag);
            Value = value;
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TarByte(byte tag, bool value) : this(tag, Unsafe.As<bool, byte>(ref value))
        {

        }

        public void ReadFrom(ref TarReader reader)
        {
            Header = reader.ReadHeader();
            Value = Header.Type switch
            {
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
                return;
            }
            writer.WriteHeader(Header);
            writer.WriteRawByte(Value);
        }

        public override bool Equals(object? obj)
        {
            return obj is TarByte value && Equals(value);
        }

        public bool Equals(TarByte value)
        {
            return value.Header == Header && value.Value == Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header.GetHashCode(), Value);
        }

        public static implicit operator byte(TarByte value)
        {
            return value.Value;
        }

        public static bool operator ==(TarByte left, TarByte right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TarByte left, TarByte right)
        {
            return !(left == right);
        }
    }
}
