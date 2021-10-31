using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Executorlibs.TarProtocol.IO;

namespace Executorlibs.TarProtocol.Protocol.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] {Value}")]
    public struct TarList<T> : ITarType where T : ITarType, new()
    {
        public TarHeader Header;

        public T[] Value;

        public TarList(byte tag, T[] value)
        {
            Header = new TarHeader
            {
                Type = TarHeader.TarType.List,
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
                case TarHeader.TarType.List:
                    {
                        TarInt32 length = default;
                        length.ReadFrom(stream);
                        T[] value = new T[length];
                        for (int i = 0; i < length; i++)
                        {
                            T t = new T();
                            t.ReadFrom(stream);
                            value[i] = t;
                        }
                        Value = value;
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
            TarInt32 length = new TarInt32(0, Value.Length);
            length.WriteTo(stream);
            foreach (T t in Value)
            {
                t.WriteTo(stream);
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is TarList<T> t && this.Header.Data == t.Header.Data && this.Value == t.Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header.Data, Value);
        }

        public static implicit operator T[](TarList<T> t)
        {
            return t.Value;
        }

        public static bool operator ==(TarList<T> left, TarList<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TarList<T> left, TarList<T> right)
        {
            return !(left == right);
        }
    }
}
