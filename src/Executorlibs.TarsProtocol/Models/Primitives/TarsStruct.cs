using System;
using System.Diagnostics;
using Executorlibs.TarsProtocol.IO;

namespace Executorlibs.TarsProtocol.Models.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] {Value}")]
    public struct TarsStruct<T> : ITarsType where T : ITarsType, new()
    {
        private TarsHeader Header => new TarsHeader(TarsType.StructBegin, Tag);

        public byte Tag;

        public T Value;

        public TarsStruct(byte tag, T value)
        {
            Tag = tag;
            Value = value;
        }

        public void ReadFrom(ref TarsReader reader)
        {
            TarsStructBegin begin = default;
            begin.ReadFrom(ref reader);
            Tag = begin.Header.Tag;

            Value = new T();
            Value.ReadFrom(ref reader);

            reader.SkipGroup(); // discard remaining members
        }

        public void WriteTo(ref TarsWriter writer)
        {
            new TarsStructBegin(Tag).WriteTo(ref writer);
            Value.WriteTo(ref writer);
            TarsStructEnd.Instance.WriteTo(ref writer);
        }

        public override bool Equals(object? obj)
        {
            return obj is TarsStruct<T> value && Equals(value);
        }

        public bool Equals(TarsStruct<T> value)
        {
            return value.Tag == Tag && value.Value.Equals(Value);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Tag, Value);
        }

        public static implicit operator T(TarsStruct<T> t)
        {
            return t.Value;
        }

        public static bool operator ==(TarsStruct<T> left, TarsStruct<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TarsStruct<T> left, TarsStruct<T> right)
        {
            return !(left == right);
        }
    }
}
