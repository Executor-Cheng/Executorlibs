using System;
using System.Diagnostics;
using Executorlibs.TarProtocol.IO;

#pragma warning disable IDE0060 // Remove unused parameter
namespace Executorlibs.TarProtocol.Models.Primitives
{
    [DebuggerDisplay("[0] %StructEnd%")]
    public struct TarStructEnd : ITarType
    {
        public static TarStructEnd Instance => new TarStructEnd
        {
            Header = new TarHeader(TarType.StructEnd, 0)
        };

        public TarHeader Header;

        public void ReadFrom(ref TarReader Reader)
        {
            Header = Reader.ReadHeader();
            if (Header.Type != TarType.StructEnd)
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
            return obj is TarStructEnd value && Equals(value);
        }

        public bool Equals(TarStructEnd value)
        {
            return value.Header == Header;
        }

        public override int GetHashCode()
        {
            return Header.GetHashCode();
        }

        public static bool operator ==(TarStructEnd _, TarStructEnd __)
        {
            return true;
        }

        public static bool operator !=(TarStructEnd _, TarStructEnd __)
        {
            return false;
        }
    }
}
