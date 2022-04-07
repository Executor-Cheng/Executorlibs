using System;
using System.Collections.Generic;
using System.Diagnostics;
using Executorlibs.TarProtocol.IO;

namespace Executorlibs.TarProtocol.Models.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] {Value}")]
    public struct TarDictionary<TKey, TValue> : ITarType where TKey : ITarType, new()
                                                         where TValue : ITarType, new()
    {
        public TarHeader Header;

        public IDictionary<TKey, TValue>? Value;

        public TarDictionary(byte tag, IDictionary<TKey, TValue>? value)
        {
            Header = new TarHeader(TarType.Map, tag);
            Value = value;
        }

        public unsafe void ReadFrom(ref TarReader reader)
        {
            Header = reader.ReadHeader();
            if (Header.Type != TarType.Map)
            {
                throw new InvalidOperationException();
            }
            int length = (int)(uint)reader.ReadVarInt();
            IDictionary<TKey, TValue> value = new Dictionary<TKey, TValue>(length);
            for (int i = 0; i < length; i++)
            {
                TKey key = new TKey();
                TValue tValue = new TValue();
                key.ReadFrom(ref reader);
                tValue.ReadFrom(ref reader);
                value[key] = tValue;
            }
            Value = value;
        }

        public void WriteTo(ref TarWriter writer)
        {
            writer.WriteHeader(Header);
            if (Value == null)
            {
                new TarInt32(0, 0).WriteTo(ref writer);
                return;
            }
            new TarInt32(0, Value.Count).WriteTo(ref writer);
            foreach (KeyValuePair<TKey, TValue> t in Value)
            {
                t.Key.WriteTo(ref writer);
                t.Value.WriteTo(ref writer);
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is TarDictionary<TKey, TValue> value && Equals(value);
        }

        public bool Equals(TarDictionary<TKey, TValue> value)
        {
            return value.Header == Header && (value.Value?.Equals(Value) ?? false);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header, Value);
        }

        public static bool operator ==(TarDictionary<TKey, TValue> left, TarDictionary<TKey, TValue> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TarDictionary<TKey, TValue> left, TarDictionary<TKey, TValue> right)
        {
            return !(left == right);
        }
    }
}
