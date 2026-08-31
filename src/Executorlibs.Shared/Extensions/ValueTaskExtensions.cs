using System.Runtime.CompilerServices;
#if NET8_0_OR_GREATER
using System.Threading.Tasks;
#else
using System.Runtime.InteropServices;
#endif

namespace Executorlibs.Shared.Extensions
{
#if NET8_0_OR_GREATER
    public static class ValueTaskExtensions
    {
        [UnsafeAccessor(UnsafeAccessorKind.StaticMethod)]
        public static extern ValueTask DangerousCreateFromTypedValueTask<TResult>(ValueTask _, ValueTask<TResult> vt); // born .NET 7

        public static ValueTask DangerousAsValueTask<TResult>(this ValueTask<TResult> valueTask)
        {
            return DangerousCreateFromTypedValueTask(default, valueTask);
        }
    }
#else
    public static class ValueTaskExtensions
    {
        [StructLayout(LayoutKind.Auto)]
        private readonly struct ValueTask
        {
            private readonly object? _obj;
            private readonly short _token;
            private readonly bool _continueOnCapturedContext;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ValueTask(object? obj, short token, bool continueOnCapturedContext)
            {
                _obj = obj;
                _token = token;
                _continueOnCapturedContext = continueOnCapturedContext;
            }
        }

        [StructLayout(LayoutKind.Auto)]
        private readonly struct ValueTask<TResult>
        {
            public readonly object? _obj;
            public readonly TResult? _result;
            public readonly short _token;
            public readonly bool _continueOnCapturedContext;
        }

        public static System.Threading.Tasks.ValueTask DangerousAsValueTask<TResult>(this System.Threading.Tasks.ValueTask<TResult> valueTask)
        {
            ref var vtr = ref Unsafe.As<System.Threading.Tasks.ValueTask<TResult>, ValueTask<TResult>>(ref Unsafe.AsRef(in valueTask));
            var vt = new ValueTask(vtr._obj, vtr._token, vtr._continueOnCapturedContext);
            return Unsafe.As<ValueTask, System.Threading.Tasks.ValueTask>(ref vt);
        }
    }
#endif
}
