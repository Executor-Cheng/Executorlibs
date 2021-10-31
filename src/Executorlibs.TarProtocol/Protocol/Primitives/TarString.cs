using System;
using System.Buffers.Binary;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Executorlibs.TarProtocol.IO;

namespace Executorlibs.TarProtocol.Protocol.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] {Value}")]
    public struct TarString : ITarType
    {
        public TarHeader Header;

        public string Value;

        public TarString(byte tag, string value)
        {
            Header = new TarHeader
            {
                Type = value.Length > byte.MaxValue ? TarHeader.TarType.String4 : TarHeader.TarType.String1,
                Tag = tag
            };
            Value = value;
        }

        public unsafe void ReadFrom(TarStream stream)
        {
            Span<byte> thisSpan = MemoryMarshal.CreateSpan(ref Unsafe.As<TarHeader, byte>(ref Header), sizeof(TarHeader));
            stream.PassiveReadHeader(thisSpan);
            int length = 0;
            switch (Header.Type)
            {
                case TarHeader.TarType.String1:
                    {
                        length = stream.ReadByte();
                        break;
                    }
                case TarHeader.TarType.String4:
                    {
                        stream.Read(MemoryMarshal.CreateSpan(ref Unsafe.As<int, byte>(ref length), sizeof(int)));
#if !BIGENDIAN
                        length = BinaryPrimitives.ReverseEndianness(length);
#endif
                        break;
                    }
                default:
                    {
                        throw new InvalidOperationException();
                    }
            }
            int actual = (int)stream.Position + stream._origin;
            stream.Seek(length, SeekOrigin.Current);
            Value = Encoding.UTF8.GetString(stream.GetBuffer(), actual, length);
        }

        public void WriteTo(TarStream stream)
        {
            int length = Encoding.UTF8.GetByteCount(Value);
            if (length > byte.MaxValue)
            {
                Header.Type = TarHeader.TarType.String4;
                stream.WriteHeader(ref Header);
#if !BIGENDIAN
                length = BinaryPrimitives.ReverseEndianness(length);
#endif
                stream.Write(source: MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<int, byte>(ref length), sizeof(int)));
            }
            else
            {
                Header.Type = TarHeader.TarType.String1;
                stream.WriteHeader(ref Header);
                stream.WriteByte((byte)length);
            }
            Span<byte> buffer = stackalloc byte[96];
            ReadOnlySpan<char> chars = Value;
            while (!chars.IsEmpty)
            {
                int x = Math.Min(32, chars.Length);
                stream.Write(source: buffer[0..Encoding.UTF8.GetBytes(chars[0..x], buffer)]);
                chars = chars[x..];
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is TarString t && this.Header.Data == t.Header.Data && this.Value == t.Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header.Data, Value);
        }

        public static implicit operator string(TarString t)
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
