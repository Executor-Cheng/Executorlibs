using System;
using System.Diagnostics;
using Executorlibs.TarProtocol.IO;

namespace Executorlibs.TarProtocol.Protocol.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] {Value}")]
    public struct TarStruct<T> : ITarType where T : ITarType, new()
    {
        private TarHeader Header => new TarHeader(TarHeader.TarType.StructBegin, Tag);

        public byte Tag;

        public T Value;

        public TarStruct(byte tag, T value)
        {
            Tag = tag;
            Value = value;
        }

        public void ReadFrom(TarStream stream)
        {
            stream.Read<TarStructBegin>();
            Value = new T();
            stream.Read(ref Value);
            stream.Read<TarStructEnd>();
        }

        public void WriteTo(TarStream stream)
        {
            new TarStructBegin(Tag).WriteTo(stream);
            Value.WriteTo(stream);
            TarStructEnd.Singleton.WriteTo(stream);
        }

        public override bool Equals(object? obj)
        {
            return obj is TarStruct<T> t && this.Tag == t.Tag && this.Value.Equals(t.Value);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Tag, Value);
        }

        public static implicit operator T(TarStruct<T> t)
        {
            return t.Value;
        }

        public static bool operator ==(TarStruct<T> left, TarStruct<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TarStruct<T> left, TarStruct<T> right)
        {
            return !(left == right);
        }
    }
}
