using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Executorlibs.TarProtocol.IO;

namespace Executorlibs.TarProtocol.Protocol.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] %StructEnd%")]
    public unsafe struct TarStructEnd : ITarType
    {
        public static readonly TarStructEnd Singleton = new TarStructEnd
        {
            Header = new TarHeader
            {
                Type = TarHeader.TarType.StructEnd
            }
        };

        public TarHeader Header;

        public void ReadFrom(TarStream stream)
        {
            stream.PassiveReadHeader(MemoryMarshal.CreateSpan(ref Unsafe.As<TarHeader, byte>(ref Header), sizeof(TarStructEnd)));
            if (Header.Type != TarHeader.TarType.StructEnd)
            {
                throw new InvalidOperationException();
            }
        }

        public void WriteTo(TarStream stream)
        {
            stream.WriteHeader(ref Header);
        }

        public override bool Equals(object? obj)
        {
            return obj is TarStructEnd;
        }

        public override int GetHashCode()
        {
            return Header.Data.GetHashCode();
        }

        public static bool operator ==(TarStructEnd left, TarStructEnd right)
        {
            return true;
        }

        public static bool operator !=(TarStructEnd left, TarStructEnd right)
        {
            return false;
        }
    }
}
