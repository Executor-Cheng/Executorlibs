using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Executorlibs.TarProtocol.IO;

namespace Executorlibs.TarProtocol.Protocol.Primitives
{
    [DebuggerDisplay("[{Header.Tag}] {Value}")]
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct TarFloat : ITarType
    {
        [FieldOffset(0)]
        public fixed byte _data[6];

        [FieldOffset(0)]
        public TarHeader Header;

        [FieldOffset(2)]
        public float Value;

        public TarFloat(byte tag, float value)
        {
            Header = new TarHeader
            {
                Type = TarHeader.TarType.Float,
                Tag = tag
            };
            Value = value;
        }

        public void ReadFrom(TarStream stream)
        {
            Span<byte> thisSpan = MemoryMarshal.CreateSpan(ref _data[0], sizeof(TarInt16));
            stream.PassiveReadHeader(thisSpan);
            switch (Header.Type)
            {
                case TarHeader.TarType.Float:
                    {
                        stream.Read(thisSpan[2..6]);
                        break;
                    }
                case TarHeader.TarType.Zero:
                    {
                        Value = 0f;
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
            if (Value == 0f)
            {
                new TarZero(Header.Tag).WriteTo(stream);
            }
            else
            {
                stream.Write(source: MemoryMarshal.CreateSpan(ref Unsafe.As<float, byte>(ref Value), sizeof(float)));
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is TarFloat t && MemoryMarshal.CreateReadOnlySpan(ref _data[0], sizeof(TarFloat))
                .SequenceEqual(MemoryMarshal.CreateReadOnlySpan(ref _data[0], sizeof(TarFloat)));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header.Data, Value);
        }

        public static implicit operator float(TarFloat t)
        {
            return t.Value;
        }

        public static bool operator ==(TarFloat left, TarFloat right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TarFloat left, TarFloat right)
        {
            return !(left == right);
        }
    }
}
