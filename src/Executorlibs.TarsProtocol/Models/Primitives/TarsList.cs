using System;
using System.Diagnostics;
using Executorlibs.TarsProtocol.IO;

namespace Executorlibs.TarsProtocol.Models.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] {Value}")]
    public struct TarsList<T> : ITarsType where T : ITarsType, new()
    {
        public TarsHeader Header;

        public T[] Value;

        public TarsList(byte tag, T[] value)
        {
            Header = new TarsHeader(TarsType.List, tag);
            Value = value;
        }

        public void ReadFrom(ref TarsReader reader)
        {
            Header = reader.ReadHeader();
            if (Header.Type != TarsType.List)
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

        public void WriteTo(ref TarsWriter writer)
        {
            writer.WriteHeader(Header);
            new TarsInt32(0, Value.Length).WriteTo(ref writer);
            foreach (T t in Value)
            {
                t.WriteTo(ref writer);
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is TarsList<T> value && Equals(value);
        }

        public bool Equals(TarsList<T> value)
        {
            return value.Header == Header && value.Value == Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header, Value);
        }

        public static implicit operator T[](TarsList<T> t)
        {
            return t.Value;
        }

        public static bool operator ==(TarsList<T> left, TarsList<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TarsList<T> left, TarsList<T> right)
        {
            return !(left == right);
        }
    }
}
