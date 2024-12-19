using System;
using System.Diagnostics;
using Executorlibs.TarsProtocol.IO;

namespace Executorlibs.TarsProtocol.Models.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] 0")]
    public struct TarsZero : ITarsType
    {
        public TarsHeader Header;

        public TarsZero(byte tag)
        {
            Header = new TarsHeader(TarsType.Zero, tag);
        }

        public void ReadFrom(ref TarsReader reader)
        {
            Header = reader.ReadHeader();
            if (Header.Type != TarsType.Zero)
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
            return obj is TarsZero value && Equals(value);
        }

        public bool Equals(TarsZero value)
        {
            return value.Header == Header;
        }

        public override int GetHashCode()
        {
            return Header.GetHashCode();
        }

        public static implicit operator byte(TarsZero _)
        {
            return 0;
        }

        public static bool operator ==(TarsZero left, TarsZero right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TarsZero left, TarsZero right)
        {
            return !(left == right);
        }
    }
}
