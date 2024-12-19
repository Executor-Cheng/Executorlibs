using System;
using System.Diagnostics;
using Executorlibs.TarsProtocol.IO;

#pragma warning disable IDE0060 // Remove unused parameter
namespace Executorlibs.TarsProtocol.Models.Primitives
{
    [DebuggerDisplay("[0] %StructEnd%")]
    public struct TarsStructEnd : ITarsType
    {
        public static TarsStructEnd Instance => new TarsStructEnd
        {
            Header = new TarsHeader(TarsType.StructEnd, 0)
        };

        public TarsHeader Header;

        public void ReadFrom(ref TarsReader Reader)
        {
            Header = Reader.ReadHeader();
            if (Header.Type != TarsType.StructEnd)
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
            return obj is TarsStructEnd value && Equals(value);
        }

        public bool Equals(TarsStructEnd value)
        {
            return value.Header == Header;
        }

        public override int GetHashCode()
        {
            return Header.GetHashCode();
        }

        public static bool operator ==(TarsStructEnd _, TarsStructEnd __)
        {
            return true;
        }

        public static bool operator !=(TarsStructEnd _, TarsStructEnd __)
        {
            return false;
        }
    }
}
