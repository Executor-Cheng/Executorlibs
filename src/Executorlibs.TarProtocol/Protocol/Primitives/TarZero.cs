using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Executorlibs.TarProtocol.IO;

namespace Executorlibs.TarProtocol.Protocol.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] 0")]
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct TarZero : ITarType
    {
        [FieldOffset(0)]
        public fixed byte _data[2];

        [FieldOffset(0)]
        public TarHeader Header;

        public TarZero(byte tag)
        {
            Header = new TarHeader
            {
                Type = TarHeader.TarType.Zero,
                Tag = tag
            };
        }

        public void ReadFrom(TarStream stream)
        {
            stream.PassiveReadHeader(MemoryMarshal.CreateSpan(ref _data[0], sizeof(TarZero)));
            if (Header.Type != TarHeader.TarType.Zero)
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
            return obj is TarZero t && MemoryMarshal.CreateReadOnlySpan(ref _data[0], sizeof(TarZero))
                .SequenceEqual(MemoryMarshal.CreateReadOnlySpan(ref t._data[0], sizeof(TarZero)));
        }

        public override int GetHashCode()
        {
            return Header.Data.GetHashCode();
        }

        public static implicit operator byte(TarZero t)
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
