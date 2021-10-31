using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Executorlibs.TarProtocol.IO;

namespace Executorlibs.TarProtocol.Protocol.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] {Value}")]
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct TarDouble : ITarType
    {
        [FieldOffset(0)]
        public fixed byte _data[10];

        [FieldOffset(0)]
        public TarHeader Header;

        [FieldOffset(2)]
        public double Value;

        public TarDouble(byte tag, double value)
        {
            Header = new TarHeader
            {
                Type = TarHeader.TarType.Double,
                Tag = tag
            };
            Value = value;
        }

        public void ReadFrom(TarStream stream)
        {
            Span<byte> thisSpan = MemoryMarshal.CreateSpan(ref _data[0], sizeof(TarDouble));
            stream.PassiveReadHeader(thisSpan);
            switch (Header.Type)
            {
                case TarHeader.TarType.Double:
                    {
                        stream.Read(thisSpan[2..10]);
                        break;
                    }
                case TarHeader.TarType.Zero:
                    {
                        Value = 0d;
                        break;
                    }
                default:
                    {
                        throw new InvalidOperationException();
                    }
            }
        }

        public void WriteTo(TarStream stream)
        {
            if (Value == 0d)
            {
                new TarZero(Header.Tag).WriteTo(stream);
            }
            else
            {
                stream.Write(source: MemoryMarshal.CreateSpan(ref Unsafe.As<double, byte>(ref Value), sizeof(double)));
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is TarDouble t && MemoryMarshal.CreateReadOnlySpan(ref _data[0], sizeof(TarDouble))
                .SequenceEqual(MemoryMarshal.CreateReadOnlySpan(ref _data[0], sizeof(TarDouble)));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header.Data, Value);
        }

        public static implicit operator double(TarDouble t)
        {
            return t.Value;
        }

        public static bool operator ==(TarDouble left, TarDouble right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TarDouble left, TarDouble right)
        {
            return !(left == right);
        }
    }
}
