using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Executorlibs.TarProtocol.IO;

namespace Executorlibs.TarProtocol.Protocol.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] %StructBegin%")]
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct TarStructBegin : ITarType
    {
        [FieldOffset(0)]
        public fixed byte _data[2];

        [FieldOffset(0)]
        public TarHeader Header;

        public TarStructBegin(byte tag)
        {
            Header = new TarHeader
            {
                Type = TarHeader.TarType.StructBegin,
                Tag = tag
            };
        }

        public void ReadFrom(TarStream stream)
        {
            stream.PassiveReadHeader(MemoryMarshal.CreateSpan(ref _data[0], sizeof(TarStructBegin)));
            if (Header.Type != TarHeader.TarType.StructBegin)
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
            return obj is TarStructBegin t && MemoryMarshal.CreateReadOnlySpan(ref _data[0], sizeof(TarStructBegin))
                .SequenceEqual(MemoryMarshal.CreateReadOnlySpan(ref t._data[0], sizeof(TarStructBegin)));
        }

        public override int GetHashCode()
        {
            return Header.Data.GetHashCode();
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
