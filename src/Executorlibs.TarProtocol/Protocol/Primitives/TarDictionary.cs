using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Executorlibs.TarProtocol.IO;

namespace Executorlibs.TarProtocol.Protocol.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] {Value}")]
    public struct TarDictionary<TKey, TValue> : ITarType where TKey : ITarType, new()
                                                         where TValue : ITarType, new()
    {
        public TarHeader Header;

        public IDictionary<TKey, TValue> Value;

        public TarDictionary(byte tag, IDictionary<TKey, TValue> value)
        {
            Header = new TarHeader
            {
                Type = TarHeader.TarType.Map,
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
                case TarHeader.TarType.Map:
                    {
                        TarInt32 length = default;
                        length.ReadFrom(stream);
                        IDictionary<TKey, TValue> value = new Dictionary<TKey, TValue>(length);
                        for (int i = 0; i < length; i++)
                        {
                            TKey key = new TKey();
                            TValue tValue = new TValue();
                            key.ReadFrom(stream);
                            tValue.ReadFrom(stream);
                            value[key] = tValue;
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
            TarInt32 length = new TarInt32(0, Value.Count);
            length.WriteTo(stream);
            foreach (KeyValuePair<TKey, TValue> t in Value)
            {
                t.Key.WriteTo(stream);
                t.Value.WriteTo(stream);
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is TarDictionary<TKey, TValue> t && this.Header.Data == t.Header.Data && this.Value == t.Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header.Data, Value);
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
