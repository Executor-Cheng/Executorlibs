using System;
using System.Diagnostics;
using Executorlibs.TarProtocol.IO;

namespace Executorlibs.TarProtocol.Models.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] %StructBegin%")]
    public struct TarStructBegin : ITarType
    {
        public TarHeader Header;

        public TarStructBegin(byte tag)
        {
            Header = new TarHeader(TarType.StructBegin, tag);
        }

        public void ReadFrom(ref TarReader reader)
        {
            Header = reader.ReadHeader();
            if (Header.Type != TarType.StructBegin)
            {
                throw new InvalidOperationException();
            }
        }

        public void WriteTo(ref TarWriter writer)
        {
            writer.WriteHeader(Header);
        }

        public override bool Equals(object? obj)
        {
            return obj is TarStructBegin value && Equals(value);
        }

        public bool Equals(TarStructBegin value)
        {
            return value.Header == Header;
        }

        public override int GetHashCode()
        {
            return Header.GetHashCode();
        }

        public static bool operator ==(TarStructBegin left, TarStructBegin right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TarStructBegin left, TarStructBegin right)
        {
            return !(left == right);
        }
    }
}
