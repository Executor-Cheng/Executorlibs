using System;
using System.Diagnostics;
using System.IO;
using Executorlibs.TarsProtocol.IO;

namespace Executorlibs.TarsProtocol.Models.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] {Value}")]
    public struct TarsByteArray : ITarsType
    {
        public TarsHeader Header;

        public ArraySegment<byte> Value;

        public TarsByteArray(byte tag, ArraySegment<byte> value)
        {
            Header = new TarsHeader
            {
                Type = TarsType.SimpleList,
                Tag = tag
            };
            Value = value;
        }

        public void ReadFrom(ref TarsReader reader)
        {
            Header = reader.ReadHeader();
            switch (Header.Type)
            {
                case TarsType.SimpleList:
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

        public void WriteTo(ref TarsWriter writer)
        {
            writer.WriteHeader(Header);
            writer.WriteRawByte(0);
            new TarsInt32(0, Value.Count).WriteTo(ref writer);
            writer.WriteBytes(Value);
        }

        public override bool Equals(object? obj)
        {
            return obj is TarsByteArray value && Equals(value);
        }

        public bool Equals(TarsByteArray value)
        {
            return value.Header == Header && value.Value == Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header, Value);
        }

        public static implicit operator ArraySegment<byte>(TarsByteArray t)
        {
            return t.Value;
        }

        public static bool operator ==(TarsByteArray left, TarsByteArray right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TarsByteArray left, TarsByteArray right)
        {
            return !(left == right);
        }
    }

    [DebuggerDisplay("[{Header.Tag}] {Value}")]
    public struct TarByteArray<T> : ITarsType where T : ITarsType, new()
    {
        public TarsHeader Header;

        public T Value;

        public TarByteArray(byte tag, T value)
        {
            Header = new TarsHeader(TarsType.SimpleList, tag);
            Value = value;
        }

        public void ReadFrom(ref TarsReader reader)
        {
            Header = reader.ReadHeader();
            switch (Header.Type)
            {
                case TarsType.SimpleList:
                    {
                        if (reader.ReadRawByte() != 0)
                        {
                            throw new InvalidDataException();
                        }
                        int length = (int)(uint)reader.ReadVarInt();
                        ArraySegment<byte> segment = reader.ReadBytes(length);
                        TarsReader segementReader = new TarsReader(segment);
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

        public void WriteTo(ref TarsWriter writer)
        {
            writer.WriteHeader(Header);
            writer.WriteRawByte(0);

            MemoryStream ms = new MemoryStream(1024);
            TarsWriter tWriter = new TarsWriter(ms);
            Value.WriteTo(ref tWriter);
            tWriter.Flush();

            int length = (int)ms.Length;
            new TarsInt32(0, length).WriteTo(ref writer);
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
