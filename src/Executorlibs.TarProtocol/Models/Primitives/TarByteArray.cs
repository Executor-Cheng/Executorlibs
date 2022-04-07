using System;
using System.Diagnostics;
using System.IO;
using Executorlibs.TarProtocol.IO;

namespace Executorlibs.TarProtocol.Models.Primitives
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
                Type = TarType.SimpleList,
                Tag = tag
            };
            Value = value;
        }

        public void ReadFrom(ref TarReader reader)
        {
            Header = reader.ReadHeader();
            switch (Header.Type)
            {
                case TarType.SimpleList:
                    {
                        if (reader.ReadRawByte() != 0)
                        {
                            throw new InvalidDataException();
                        }
                        int length = (int)(uint)reader.ReadVarInt();
                        Value = reader.ReadBytes(length);
                        break;
                    }
                default:
                    {
                        throw new InvalidOperationException();
                    }
            }
        }

        public void WriteTo(ref TarWriter writer)
        {
            writer.WriteHeader(Header);
            writer.WriteRawByte(0);
            new TarInt32(0, Value.Count).WriteTo(ref writer);
            writer.WriteBytes(Value);
        }

        public override bool Equals(object? obj)
        {
            return obj is TarByteArray value && Equals(value);
        }

        public bool Equals(TarByteArray value)
        {
            return value.Header == Header && value.Value == Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header, Value);
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
            Header = new TarHeader(TarType.SimpleList, tag);
            Value = value;
        }

        public void ReadFrom(ref TarReader reader)
        {
            Header = reader.ReadHeader();
            switch (Header.Type)
            {
                case TarType.SimpleList:
                    {
                        if (reader.ReadRawByte() != 0)
                        {
                            throw new InvalidDataException();
                        }
                        int length = (int)(uint)reader.ReadVarInt();
                        ArraySegment<byte> segment = reader.ReadBytes(length);
                        TarReader segementReader = new TarReader(segment);
                        Value = new T();
                        Value.ReadFrom(ref segementReader);
                        break;
                    }
                default:
                    {
                        throw new InvalidOperationException();
                    }
            }
        }

        public void WriteTo(ref TarWriter writer)
        {
            writer.WriteHeader(Header);
            writer.WriteRawByte(0);

            MemoryStream ms = new MemoryStream(1024);
            TarWriter tWriter = new TarWriter(ms);
            Value.WriteTo(ref tWriter);
            tWriter.Flush();

            int length = (int)ms.Length;
            new TarInt32(0, length).WriteTo(ref writer);
            writer.WriteBytes(ms.GetBuffer(), 0, length);
        }

        public override bool Equals(object? obj)
        {
            return obj is TarByteArray<T> value && Equals(value);
        }

        public bool Equals(TarByteArray<T> value)
        {
            return value.Header == Header && value.Value.Equals(Value);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header, Value);
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
