using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Executorlibs.TarProtocol.Protocol;

namespace Executorlibs.TarProtocol.IO
{
    public unsafe class TarStream : MemoryStream
    {
        public readonly int _origin;

        public TarStream() : base() { }

        public TarStream(int capacity) : base(capacity)
        {

        }

        public TarStream(byte[] buffer) : base(buffer, 0, buffer.Length, false, true)
        {

        }

        public TarStream(ArraySegment<byte> buffer) : base(buffer.Array!, buffer.Offset, buffer.Count, false, true)
        {
            _origin = buffer.Offset;
        }

        public ref TarHeader PositiveReadHeader(ref TarHeader header)
        {
            return ref PositiveReadHeader(MemoryMarshal.CreateSpan(ref Unsafe.As<TarHeader, byte>(ref header), sizeof(TarHeader)));
        }

        public ref TarHeader PositiveReadHeader(Span<byte> data)
        {
            base.Read(data[..2]);
            ref TarHeader header = ref Unsafe.As<byte, TarHeader>(ref MemoryMarshal.GetReference(data));
            if (header.ActualSize == 1)
            {
                base.Seek(-1, SeekOrigin.Current);
            }
            return ref header;
        }

        public ref TarHeader PassiveReadHeader(ref TarHeader header)
        {
            return ref PassiveReadHeader(MemoryMarshal.CreateSpan(ref Unsafe.As<TarHeader, byte>(ref header), sizeof(TarHeader)));
        }

        public ref TarHeader PassiveReadHeader(Span<byte> data)
        {
            base.Read(data[..1]);
            ref TarHeader header = ref Unsafe.As<byte, TarHeader>(ref MemoryMarshal.GetReference(data));
            if (header.ActualSize == 2)
            {
                base.Read(data[1..2]);
            }
            return ref header;
        }

        public void WriteHeader(ref TarHeader header)
        {
            base.Write(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<TarHeader, byte>(ref header), header.ActualSize));
        }

        public T Read<T>() where T : ITarType, new()
        {
            T result = new T();
            result.ReadFrom(this);
            return result;
        }

        public void Read<T>(ref T tarType) where T : ITarType, new()
        {
            tarType.ReadFrom(this);
        }

        public void Write<T>(T tarType) where T : ITarType
        {
            tarType.WriteTo(this);
        }

        public void Write<T>(ref T tarType) where T : ITarType
        {
            tarType.WriteTo(this);
        }

        public override int Read(Span<byte> destination)
        {
            return base.Read(destination);
        }

        public override void Write(ReadOnlySpan<byte> source)
        {
            base.Write(source);
        }

        //public List<object> DeconstructStruct()
        //{
        //    Span<byte> buffer = stackalloc byte[8];
        //    ref TarHeader header = ref PassiveReadHeader(buffer);
        //    if (header.Type == TarHeader.TarType.StructBegin)
        //    {
        //        List<object> result = new List<object>();
        //        while (true)
        //        {
        //            header = ref PassiveReadHeader(buffer);
        //            switch (header.Type)
        //            {
        //                case TarHeader.TarType.Zero:
        //                    {
        //                        result.Add((byte)0);
        //                        break;
        //                    }
        //                case TarHeader.TarType.Byte:
        //                    {
        //                        result.Add((byte)base.ReadByte());
        //                        break;
        //                    }
        //                case TarHeader.TarType.Short:
        //                    {
        //                        result.Add(ReadInt16Core(buffer));
        //                        break;
        //                    }
        //                case TarHeader.TarType.Int:
        //                    {
        //                        result.Add(ReadInt32Core(buffer));
        //                        break;
        //                    }
        //                case TarHeader.TarType.Long:
        //                    {
        //                        result.Add(ReadInt64Core(buffer));
        //                        break;
        //                    }
        //                case TarHeader.TarType.String1:
        //                case TarHeader.TarType.String4:
        //                    {
        //                        result.Add(ReadStringCore(buffer, ref header));
        //                        break;
        //                    }
        //                case TarHeader.TarType.StructBegin:
        //                    {
        //                        base.Seek(-header.ActualSize, SeekOrigin.Current);
        //                        result.Add(DeconstructStruct());
        //                        break;
        //                    }
        //                case TarHeader.TarType.StructEnd:
        //                    {
        //                        return result;
        //                    }
        //            }
        //        }
        //    }
        //    throw new InvalidOperationException();
        //}
    }
}
