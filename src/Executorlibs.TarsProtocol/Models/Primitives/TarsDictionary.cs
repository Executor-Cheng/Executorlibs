using System;
using System.Collections.Generic;
using System.Diagnostics;
using Executorlibs.TarsProtocol.IO;

namespace Executorlibs.TarsProtocol.Models.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] {Value}")]
    public struct TarsDictionary<TKey, TValue> : ITarsType where TKey : ITarsType, new()
                                                         where TValue : ITarsType, new()
    {
        public TarsHeader Header;

        public IDictionary<TKey, TValue>? Value;

        public TarsDictionary(byte tag, IDictionary<TKey, TValue>? value)
        {
            Header = new TarsHeader(TarsType.Map, tag);
            Value = value;
        }

        public unsafe void ReadFrom(ref TarsReader reader)
        {
            Header = reader.ReadHeader();
            if (Header.Type != TarsType.Map)
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

        public void WriteTo(ref TarsWriter writer)
        {
            writer.WriteHeader(Header);
            if (Value == null)
            {
                new TarsInt32(0, 0).WriteTo(ref writer);
                return;
            }
            new TarsInt32(0, Value.Count).WriteTo(ref writer);
            foreach (KeyValuePair<TKey, TValue> t in Value)
            {
                t.Key.WriteTo(ref writer);
                t.Value.WriteTo(ref writer);
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is TarsDictionary<TKey, TValue> value && Equals(value);
        }

        public bool Equals(TarsDictionary<TKey, TValue> value)
        {
            return value.Header == Header && (value.Value?.Equals(Value) ?? false);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header, Value);
        }

        public static bool operator ==(TarsDictionary<TKey, TValue> left, TarsDictionary<TKey, TValue> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TarsDictionary<TKey, TValue> left, TarsDictionary<TKey, TValue> right)
        {
            return !(left == right);
        }
    }
}
