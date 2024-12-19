using System;
using System.Diagnostics;
using Executorlibs.TarsProtocol.IO;

namespace Executorlibs.TarsProtocol.Models.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] %StructBegin%")]
    public struct TarsStructBegin : ITarsType
    {
        public TarsHeader Header;

        public TarsStructBegin(byte tag)
        {
            Header = new TarsHeader(TarsType.StructBegin, tag);
        }

        public void ReadFrom(ref TarsReader reader)
        {
            Header = reader.ReadHeader();
            if (Header.Type != TarsType.StructBegin)
            {
                throw new InvalidOperationException();
            }
        }

        public void WriteTo(ref TarsWriter writer)
        {
            writer.WriteHeader(Header);
        }

        public override bool Equals(object? obj)
        {
            return obj is TarsStructBegin value && Equals(value);
        }

        public bool Equals(TarsStructBegin value)
        {
            return value.Header == Header;
        }

        public override int GetHashCode()
        {
            return Header.GetHashCode();
        }

        public static bool operator ==(TarsStructBegin left, TarsStructBegin right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TarsStructBegin left, TarsStructBegin right)
        {
            return !(left == right);
        }
    }
}
