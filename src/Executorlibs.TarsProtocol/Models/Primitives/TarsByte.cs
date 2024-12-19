using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Executorlibs.TarsProtocol.IO;

namespace Executorlibs.TarsProtocol.Models.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] {Value}")]
    public struct TarsByte : ITarsType
    {
        public TarsHeader Header;

        public byte Value;

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TarsByte(byte tag, byte value)
        {
            Header = new TarsHeader(TarsType.Byte, tag);
            Value = value;
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TarsByte(byte tag, bool value) : this(tag, Unsafe.As<bool, byte>(ref value))
        {

        }

        public void ReadFrom(ref TarsReader reader)
        {
            Header = reader.ReadHeader();
            Value = Header.Type switch
            {
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
                return;
            }
            writer.WriteHeader(Header);
            writer.WriteRawByte(Value);
        }

        public override bool Equals(object? obj)
        {
            return obj is TarsByte value && Equals(value);
        }

        public bool Equals(TarsByte value)
        {
            return value.Header == Header && value.Value == Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header.GetHashCode(), Value);
        }

        public static implicit operator byte(TarsByte value)
        {
            return value.Value;
        }

        public static bool operator ==(TarsByte left, TarsByte right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TarsByte left, TarsByte right)
        {
            return !(left == right);
        }
    }
}
