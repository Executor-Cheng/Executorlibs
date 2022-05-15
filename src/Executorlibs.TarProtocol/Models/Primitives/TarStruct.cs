using System;
using System.Diagnostics;
using Executorlibs.TarProtocol.IO;

namespace Executorlibs.TarProtocol.Models.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] {Value}")]
    public struct TarStruct<T> : ITarType where T : ITarType, new()
    {
        private TarHeader Header => new TarHeader(TarType.StructBegin, Tag);

        public byte Tag;

        public T Value;

        public TarStruct(byte tag, T value)
        {
            Tag = tag;
            Value = value;
        }

        public void ReadFrom(ref TarReader reader)
        {
            TarStructBegin begin = default;
            begin.ReadFrom(ref reader);
            Tag = begin.Header.Tag;

            Value = new T();
            Value.ReadFrom(ref reader);

            reader.SkipGroup(); // discard remaining members

            //TarStructEnd end = default;
            //end.ReadFrom(ref reader);
        }

        public void WriteTo(ref TarWriter writer)
        {
            new TarStructBegin(Tag).WriteTo(ref writer);
            Value.WriteTo(ref writer);
            TarStructEnd.Instance.WriteTo(ref writer);
        }

        public override bool Equals(object? obj)
        {
            return obj is TarStruct<T> value && Equals(value);
        }

        public bool Equals(TarStruct<T> value)
        {
            return value.Tag == Tag && value.Value.Equals(Value);
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
