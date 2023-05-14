using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Executorlibs.TarProtocol.IO;

namespace Executorlibs.TarProtocol.Models.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] {Value}")]
    public struct TarString : ITarType
    {
        public TarHeader Header;

        public string? Value;

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TarString(byte tag, string? value)
        {
            bool useString4 = value != null && value.Length > byte.MaxValue;
            Header = new TarHeader(TarType.String1 + Unsafe.As<bool, byte>(ref useString4), tag);
            Value = value;
        }

        public void ReadFrom(ref TarReader reader)
        {
            Header = reader.ReadHeader();
            uint length = Header.Type switch
            {
                TarType.String4 => reader.ParseRawBigEndian32(),
                TarType.String1 => reader.ReadRawByte(),
                _ => throw new InvalidOperationException()
            };
            if (length == 0)
            {
                return;
            }
            ArraySegment<byte> segment = reader.ReadBytes((int)length);
            Value = Encoding.UTF8.GetString(segment.Array!, segment.Offset, segment.Count);
        }

        public void WriteTo(ref TarWriter writer)
        {
            int length = string.IsNullOrEmpty(Value) ? 0 : Encoding.UTF8.GetByteCount(Value);
            if (length > byte.MaxValue)
            {
                Header.Type = TarType.String4;
                writer.WriteHeader(Header);
                writer.WriteRawBigEndian32((uint)length);
            }
            else
            {
                Header.Type = TarType.String1;
                writer.WriteHeader(Header);
                writer.WriteRawByte((byte)length);
                if (length == 0)
                {
                    return;
                }
            }
            Span<byte> buffer = stackalloc byte[93];
            ReadOnlySpan<char> chars = Value;
            while (true)
            {
                bool c = 31 < chars.Length;
                int readLimit = chars.Length ^ ((31 ^ chars.Length) & -Unsafe.As<bool, byte>(ref c));
                int written = Encoding.UTF8.GetBytes(chars[..readLimit], buffer);
                writer.WriteBytes(MemoryMarshal.CreateReadOnlySpan(ref MemoryMarshal.GetReference(buffer), written));
                int remaining = chars.Length - readLimit;
                if (remaining == 0)
                {
                    break;
                }
                chars = MemoryMarshal.CreateSpan(ref Unsafe.Add(ref MemoryMarshal.GetReference(chars), readLimit), remaining);
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is TarString value && Equals(value);
        }

        public bool Equals(TarString value)
        {
            return value.Header == Header && value.Value == Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header, Value);
        }

        public static implicit operator string?(TarString t)
        {
            return t.Value;
        }

        public static bool operator ==(TarString left, TarString right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TarString left, TarString right)
        {
            return !(left == right);
        }
    }
}
