using System.Runtime.CompilerServices;
using System.Threading;

namespace Executorlibs.MessageFramework.Subscriptions
{
    public partial class DefaultMessageSubscription<TClient, TMessage>
    {
        /// <summary>
        /// 读写互斥锁。由符号位表示写锁, 其余位表示读锁
        /// </summary>
        /// <remarks>
        /// 不正确地使用本锁将导致方法永远不会返回
        /// </remarks>
        protected internal struct UInt32Lock
        {
            private uint _lock;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static uint CompareExchange(ref uint location1, uint value, uint comparand)
            {
#if !NET5_0_OR_GREATER
                return (uint)Interlocked.CompareExchange(ref Unsafe.As<uint, int>(ref location1), (int)value, (int)comparand);
#else
                return Interlocked.CompareExchange(ref location1, value, comparand);
#endif
            }

            public void EnterWriteLock()
            {
                while (CompareExchange(ref _lock, 0x80000000, 0) != 0) { }
            }

            public void ExitWriteLock()
            {
                Volatile.Write(ref _lock, 0);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void EnterReadLock()
            {
                uint value = Volatile.Read(ref _lock);
                while (true)
                {
                    value &= 0x7FFFFFFF;
                    uint original = CompareExchange(ref _lock, value + 1, value);
                    if (value == original)
                    {
                        break;
                    }
                    value = original;
                }
            }

            public void ExitReadLock()
            {
#if NETSTANDARD
                Interlocked.Decrement(ref Unsafe.As<uint, int>(ref _lock));
#else
                Interlocked.Decrement(ref _lock);
#endif
            }
        }
    }
}
