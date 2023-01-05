using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if NET5_0_OR_GREATER
namespace Executorlibs.Shared
{
    internal sealed class RawData
    {
        public nuint Data;
    }

    public static unsafe class UnsafeHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static ref nuint GetRawDataReference(this object obj)
        {
            return ref Unsafe.As<RawData>(obj).Data;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static void* GetRawData(object obj)
        {
            return Unsafe.AsPointer(ref Unsafe.As<RawData>(obj).Data);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static ref MethodTable GetMethodTableReference(object obj)
        {
            return ref Unsafe.AsRef<MethodTable>(GetMethodTable(obj));
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static MethodTable* GetMethodTable(object obj)
        {
            return (MethodTable*)Unsafe.Add(ref GetRawDataReference(obj), -1);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static void* GetPointer(object obj)
        {
            return (void*)Unsafe.AsPointer(ref Unsafe.Add(ref GetRawDataReference(obj), -1));
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static void* GetData(object obj)
        {
            MethodTable* pMT = GetMethodTable(obj);
            return (void**)GetPointer(obj) + 1 + (pMT->Flags >> 31);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static nuint GetRawObjectDataSize(object obj)
        {
            MethodTable* pMT = (MethodTable*)GetMethodTable(obj);
            nuint rawSize = pMT->BaseSize - (uint)(2 * sizeof(nuint));
            if ((pMT->Flags >> 31) != 0)
            {
                rawSize += Unsafe.As<nuint, uint>(ref GetRawDataReference(obj)) * (nuint)pMT->ComponentSize;
            }
            return rawSize;
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct MethodTable
    {
        private const int PtrSize =
#if TARGET_64BIT
            8
#else
            4
#endif
            ;

        [FieldOffset(0)]
        public ushort ComponentSize;

        [FieldOffset(0)]
        public uint Flags;

        [FieldOffset(4)]
        public uint BaseSize;

        // 0x8: m_wFlags2

        // 0xA: m_wToken

        [FieldOffset(0xC)]
        public ushort VirtualsCount;

        [FieldOffset(0xE)]
        public ushort InterfaceCount;

        [FieldOffset(0x10)]
        public MethodTable* ParentMethodTable;

        [FieldOffset(0x10 + 4 * PtrSize)]
        public void* ElementType;

        [FieldOffset(0x10 + 5 * PtrSize)]
        public MethodTable** InterfaceMap;
    }
}
#endif
