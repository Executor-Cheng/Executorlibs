using System;
using System.Diagnostics;
using Executorlibs.TarProtocol.IO;

namespace Executorlibs.TarProtocol.Models.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] {Value}")]
    public struct TarList<T> : ITarType where T : ITarType, new()
    {
        public TarHeader Header;

        public T[] Value;

        public TarList(byte tag, T[] value)
        {
            Header = new TarHeader(TarType.List, tag);
            Value = value;
        }

        public void ReadFrom(ref TarReader reader)
        {
            Header = reader.ReadHeader();
            if (Header.Type != TarType.List)
            {
                throw new InvalidOperationException();
            }
            ulong length = reader.ReadVarInt();
            T[] value = new T[length];
            for (ulong i = 0; i < length; i++)
            {
                T t = new T();
                t.ReadFrom(ref reader);
                value[i] = t;
            }
            Value = value;
        }

        public void WriteTo(ref TarWriter writer)
        {
            writer.WriteHeader(Header);
            new TarInt32(0, Value.Length).WriteTo(ref writer);
            foreach (T t in Value)
            {
                t.WriteTo(ref writer);
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is TarList<T> value && Equals(value);
        }

        public bool Equals(TarList<T> value)
        {
            return value.Header == Header && value.Value == Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header, Value);
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
