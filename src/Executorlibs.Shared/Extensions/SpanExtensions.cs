using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Executorlibs.Shared.Extensions
{
    public static unsafe class ReadOnlySpanExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> UnsafeSlice<T>(this Span<T> span, int start)
        {
            return span.UnsafeSlice(start, span.Length - start);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> UnsafeSlice<T>(this Span<T> span, int start, int length)
        {
#if !NETSTANDARD2_0
            return MemoryMarshal.CreateSpan(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), start), length);
#else
            return span.Slice(start, length);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> UnsafeSlice<T>(this ReadOnlySpan<T> span, int start)
        {
            return span.UnsafeSlice(start, span.Length - start);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> UnsafeSlice<T>(this ReadOnlySpan<T> span, int start, int length)
        {
#if !NETSTANDARD2_0
            return MemoryMarshal.CreateReadOnlySpan(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), start), length);
#else
            return span.Slice(start, length);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T UnsafeGet<T>(this Span<T> span, int index)
        {
            return Unsafe.Add(ref MemoryMarshal.GetReference(span), index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T UnsafeGet<T>(this ReadOnlySpan<T> span, int index)
        {
            return Unsafe.Add(ref MemoryMarshal.GetReference(span), index);
        }
    }
}
