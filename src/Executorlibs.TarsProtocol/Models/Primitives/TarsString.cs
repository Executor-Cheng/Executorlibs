using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Executorlibs.TarsProtocol.IO;

namespace Executorlibs.TarsProtocol.Models.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] {Value}")]
    public struct TarsString : ITarsType
    {
        public TarsHeader Header;

        public string? Value;

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TarsString(byte tag, string? value)
        {
            bool useString4 = value != null && value.Length > byte.MaxValue;
            Header = new TarsHeader(TarsType.String1 + Unsafe.As<bool, byte>(ref useString4), tag);
            Value = value;
        }

        public void ReadFrom(ref TarsReader reader)
        {
            Header = reader.ReadHeader();
            uint length = Header.Type switch
            {
                TarsType.String4 => reader.ParseRawBigEndian32(),
                TarsType.String1 => reader.ReadRawByte(),
                _ => throw new InvalidOperationException()
            };
            if (length == 0)
            {
                return;
            }
            ArraySegment<byte> segment = reader.ReadBytes((int)length);
            Value = Encoding.UTF8.GetString(segment.Array!, segment.Offset, segment.Count);
        }

        public void WriteTo(ref TarsWriter writer)
        {
            int length = string.IsNullOrEmpty(Value) ? 0 : Encoding.UTF8.GetByteCount(Value);
            if (length > byte.MaxValue)
            {
                Header.Type = TarsType.String4;
                writer.WriteHeader(Header);
                writer.WriteRawBigEndian32((uint)length);
            }
            else
            {
                Header.Type = TarsType.String1;
                writer.WriteHeader(Header);
                writer.WriteRawByte((byte)length);
                if (length == 0)
                {
                    return;
                }
            }
            Span<byte> buffer = stackalloc byte[96];
            ReadOnlySpan<char> chars = Value;
            while (true)
            {
                bool c = 32 < chars.Length;
                int readLimit = chars.Length ^ ((32 ^ chars.Length) & -Unsafe.As<bool, byte>(ref c));
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
            return obj is TarsString value && Equals(value);
        }

        public bool Equals(TarsString value)
        {
            return value.Header == Header && value.Value == Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header, Value);
        }

        public static implicit operator string?(TarsString t)
        {
            return t.Value;
        }

        public static bool operator ==(TarsString left, TarsString right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TarsString left, TarsString right)
        {
            return !(left == right);
        }
    }
}
