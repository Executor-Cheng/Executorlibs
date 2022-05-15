using System.Threading;

namespace Microsoft.Extensions.Caching.Memory
{
    internal static class CacheEntryHelper<TKey, TValue> where TKey : notnull
    {
        private static readonly AsyncLocal<CacheEntry<TKey, TValue>> _current = new AsyncLocal<CacheEntry<TKey, TValue>>();

        internal static CacheEntry<TKey, TValue> Current
        {
            get => _current.Value!;
            private set => _current.Value = value;
        }

        internal static CacheEntry<TKey, TValue> EnterScope(CacheEntry<TKey, TValue> current)
        {
            CacheEntry<TKey, TValue> previous = Current;
            Current = current;
            return previous;
        }

        internal static void ExitScope(CacheEntry<TKey, TValue> current, CacheEntry<TKey, TValue> previous)
        {
            Current = previous;
        }
    }
}
