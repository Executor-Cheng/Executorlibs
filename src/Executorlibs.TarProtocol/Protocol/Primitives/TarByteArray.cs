using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Executorlibs.TarProtocol.IO;

namespace Executorlibs.TarProtocol.Protocol.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] {Value}")]
    public struct TarByteArray : ITarType
    {
        public TarHeader Header;

        public ArraySegment<byte> Value;

        public TarByteArray(byte tag, ArraySegment<byte> value)
        {
            Header = new TarHeader
            {
                Type = TarHeader.TarType.SimpleList,
                Tag = tag
            };
            Value = value;
        }

        public unsafe void ReadFrom(TarStream stream)
        {
            Span<byte> thisSpan = MemoryMarshal.CreateSpan(ref Unsafe.As<TarHeader, byte>(ref Header), sizeof(TarHeader));
            stream.PassiveReadHeader(thisSpan);
            switch (Header.Type)
            {
                case TarHeader.TarType.SimpleList:
                    {
                        if (stream.ReadByte() != 0)
                        {
                            goto default;
                        }
                        TarInt32 length = default;
                        length.ReadFrom(stream);
                        int actual = (int)stream.Position + stream._origin;
                        stream.Seek(length, SeekOrigin.Current);
                        Value = new ArraySegment<byte>(stream.GetBuffer(), actual, length);
                        break;
                    }
                default:
                    {
                        throw new InvalidOperationException();
                    }
            }
        }

        public void WriteTo(TarStream stream)
        {
            stream.WriteHeader(ref Header);
            stream.WriteByte(0);
            TarInt32 length = new TarInt32(0, Value.Count);
            length.WriteTo(stream);
            stream.Write(Value.Array!, Value.Offset, Value.Count);
        }

        public override bool Equals(object? obj)
        {
            return obj is TarByteArray t && this.Header.Data == t.Header.Data && this.Value == t.Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header.Data, Value);
        }

        public static implicit operator ArraySegment<byte>(TarByteArray t)
        {
            return t.Value;
        }

        public static bool operator ==(TarByteArray left, TarByteArray right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TarByteArray left, TarByteArray right)
        {
            return !(left == right);
        }
    }

    [DebuggerDisplay("[{Header.Tag}] {Value}")]
    public struct TarByteArray<T> : ITarType where T : ITarType, new()
    {
        public TarHeader Header;

        public T Value;

        public TarByteArray(byte tag, T value)
        {
            Header = new TarHeader(TarHeader.TarType.SimpleList, tag);
            Value = value;
        }

        public unsafe void ReadFrom(TarStream stream)
        {
            Span<byte> thisSpan = MemoryMarshal.CreateSpan(ref Unsafe.As<TarHeader, byte>(ref Header), sizeof(TarHeader));
            stream.PassiveReadHeader(thisSpan);
            switch (Header.Type)
            {
                case TarHeader.TarType.SimpleList:
                    {
                        if (stream.ReadByte() == 0)
                        {
                            TarInt32 length = default;
                            length.ReadFrom(stream);
                            int current = (int)stream.Position;
                            Value = new T();
                            Value.ReadFrom(stream);
                            stream.Seek(length + current - (int)stream.Position, SeekOrigin.Current); // 已经读完了 T Data, 如果有剩, 全部跳过
                            return;
                        }
                        throw new InvalidOperationException();
                    }
                default:
                    {
                        throw new InvalidOperationException();
                    }
            }
        }

        public void WriteTo(TarStream stream)
        {
            using TarStream innerStream = new TarStream(128);
            Value.WriteTo(innerStream);
            innerStream.Seek(0, SeekOrigin.Begin);
            stream.WriteHeader(ref Header);
            stream.WriteByte(0);
            TarInt32 length = new TarInt32(0, (int)innerStream.Length);
            length.WriteTo(stream);
            innerStream.CopyTo(stream);
        }

        public override bool Equals(object? obj)
        {
            return obj is TarByteArray<T> t && this.Header.Data == t.Header.Data && this.Value.Equals(t.Value);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header.Data, Value);
        }

        public static implicit operator T(TarByteArray<T> t)
        {
            return t.Value;
        }

        public static bool operator ==(TarByteArray<T> left, TarByteArray<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TarByteArray<T> left, TarByteArray<T> right)
        {
            return !(left == right);
        }
    }
}
