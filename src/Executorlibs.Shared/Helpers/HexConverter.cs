using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Executorlibs.Shared.Helpers
{
    public static unsafe class HexConverter
    {
        public enum Casing : uint
        {
            Upper = 0,

            Lower = 0x2020U,
        }

        private static readonly delegate* managed<ReadOnlySpan<byte>, Span<char>, Casing, void> _EncodeToUtf16Ptr;

        private static readonly delegate* managed<ReadOnlySpan<char>, Span<byte>, out int, bool> _TryDecodeFromUtf16Ptr;

        static HexConverter()
        {
            Type hexConverter = typeof(object).Assembly.GetType("System.HexConverter")!;
            _EncodeToUtf16Ptr = (delegate* managed<ReadOnlySpan<byte>, Span<char>, Casing, void>)hexConverter.GetMethod("EncodeToUtf16", BindingFlags.Static | BindingFlags.Public)!.MethodHandle.GetFunctionPointer();
            _TryDecodeFromUtf16Ptr = (delegate* managed<ReadOnlySpan<char>, Span<byte>, out int, bool>)hexConverter.GetMethod("TryDecodeFromUtf16", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(ReadOnlySpan<char>), typeof(Span<byte>), typeof(int).MakeByRefType() }, null)!.MethodHandle.GetFunctionPointer();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EncodeToUtf16(ReadOnlySpan<byte> bytes, Span<char> chars, Casing casing = Casing.Upper)
        {
            _EncodeToUtf16Ptr(bytes, chars, casing);
        }

        public static bool TryDecodeFromUtf16(ReadOnlySpan<char> chars, Span<byte> bytes)
        {
            return TryDecodeFromUtf16(chars, bytes, out _);
        }

        public static bool TryDecodeFromUtf16(ReadOnlySpan<char> chars, Span<byte> bytes, out int charsProcessed)
        {
            return _TryDecodeFromUtf16Ptr(chars, bytes, out charsProcessed);
        }
    }
}
