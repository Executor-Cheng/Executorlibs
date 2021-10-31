using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Executorlibs.TarProtocol.IO;

namespace Executorlibs.TarProtocol.Protocol.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] {Value}")]
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct TarByte : ITarType
    {
        [FieldOffset(0)]
        public fixed byte _data[3];

        [FieldOffset(0)]
        public TarHeader Header;

        [FieldOffset(2)]
        public byte Value;

        public TarByte(byte tag, byte value)
        {
            Header = new TarHeader
            {
                Type = TarHeader.TarType.Byte,
                Tag = tag
            };
            Value = value;
        }

        public void ReadFrom(TarStream stream)
        {
            stream.PassiveReadHeader(MemoryMarshal.CreateSpan(ref _data[0], sizeof(TarByte)));
            Value = Header.Type switch
            {
                TarHeader.TarType.Byte => (byte)stream.ReadByte(),
                TarHeader.TarType.Zero => 0,
                _ => throw new InvalidOperationException()
            };
        }

        public void WriteTo(TarStream stream)
        {
            if (Value == 0)
            {
                new TarZero(Header.Tag).WriteTo(stream);
            }
            else
            {
                stream.WriteHeader(ref Header);
                stream.WriteByte(Value);
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is TarByte t && MemoryMarshal.CreateReadOnlySpan(ref _data[0], sizeof(TarByte))
                .SequenceEqual(MemoryMarshal.CreateReadOnlySpan(ref _data[0], sizeof(TarByte)));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header.Data, Value);
        }

        public static implicit operator byte(TarByte t)
        {
            return t.Value;
        }

        public static bool operator ==(TarByte left, TarByte right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TarByte left, TarByte right)
        {
            return !(left == right);
        }
    }
}
