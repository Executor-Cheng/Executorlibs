using System;
using System.Diagnostics;
using Executorlibs.TarProtocol.IO;

namespace Executorlibs.TarProtocol.Models.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] 0")]
    public struct TarZero : ITarType
    {
        public TarHeader Header;

        public TarZero(byte tag)
        {
            Header = new TarHeader(TarType.Zero, tag);
        }

        public void ReadFrom(ref TarReader reader)
        {
            Header = reader.ReadHeader();
            if (Header.Type != TarType.Zero)
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
            return obj is TarZero value && Equals(value);
        }

        public bool Equals(TarZero value)
        {
            return value.Header == Header;
        }

        public override int GetHashCode()
        {
            return Header.GetHashCode();
        }

        public static implicit operator byte(TarZero _)
        {
            return 0;
        }

        public static bool operator ==(TarZero left, TarZero right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TarZero left, TarZero right)
        {
            return !(left == right);
        }
    }
}
