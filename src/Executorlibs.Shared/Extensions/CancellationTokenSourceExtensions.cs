using System.Runtime.CompilerServices;
using System.Threading;

namespace Executorlibs.Shared.Extensions
{
    public static class CancellationTokenSourceExtensions
    {
        public static CancellationToken UnsafeGetToken(this CancellationTokenSource? cts)
        {
#if !NET10_0_OR_GREATER
            var token = default(CancellationToken);
            Unsafe.As<CancellationToken, CancellationTokenSource?>(ref token) = cts;
            return token;
#else
            return UnsafeCreateToken(cts);
#endif
        }

#if NET10_0_OR_GREATER
        [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
        private static extern CancellationToken UnsafeCreateToken(CancellationTokenSource? cts);
#endif
    }
}
