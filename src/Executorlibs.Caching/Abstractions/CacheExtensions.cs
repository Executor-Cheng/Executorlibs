using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Caching.Memory
{
    public static class CacheExtensions
    {
        public static TValue? Get<TKey, TValue>(this IMemoryCache<TKey, TValue> cache, TKey key) where TKey : notnull
        {
            cache.TryGetValue(key, out TValue? value);
            return value;
        }

        public static TValue Set<TKey, TValue>(this IMemoryCache<TKey, TValue> cache, TKey key, TValue value) where TKey : notnull
        {
            using ICacheEntry<TKey, TValue> entry = cache.CreateEntry(key);
            entry.Value = value;

            return value;
        }

        public static TValue Set<TKey, TValue>(this IMemoryCache<TKey, TValue> cache, TKey key, TValue value, DateTimeOffset absoluteExpiration) where TKey : notnull
        {
            using ICacheEntry<TKey, TValue> entry = cache.CreateEntry(key);
            entry.AbsoluteExpiration = absoluteExpiration;
            entry.Value = value;

            return value;
        }

        public static TValue Set<TKey, TValue>(this IMemoryCache<TKey, TValue> cache, TKey key, TValue value, TimeSpan absoluteExpirationRelativeToNow) where TKey : notnull
        {
            using ICacheEntry<TKey, TValue> entry = cache.CreateEntry(key);
            entry.AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow;
            entry.Value = value;

            return value;
        }

        public static TValue Set<TKey, TValue>(this IMemoryCache<TKey, TValue> cache, TKey key, TValue value, IChangeToken expirationToken) where TKey : notnull
        {
            using ICacheEntry<TKey, TValue> entry = cache.CreateEntry(key);
            entry.AddExpirationToken(expirationToken);
            entry.Value = value;

            return value;
        }

        public static TValue Set<TKey, TValue>(this IMemoryCache<TKey, TValue> cache, TKey key, TValue value, MemoryCacheEntryOptions<TKey, TValue> options) where TKey : notnull
        {
            using ICacheEntry<TKey, TValue> entry = cache.CreateEntry(key);
            if (options != null)
            {
                entry.SetOptions(options);
            }

            entry.Value = value;

            return value;
        }

        public static TValue? GetOrCreate<TKey, TValue>(this IMemoryCache<TKey, TValue> cache, TKey key, Func<ICacheEntry<TKey, TValue>, TValue?> factory) where TKey : notnull
        {
            return cache.GetOrCreate(key, factory, out _);
        }

        public static TValue? GetOrCreate<TKey, TValue>(this IMemoryCache<TKey, TValue> cache, TKey key, Func<ICacheEntry<TKey, TValue>, TValue?> factory, out bool created) where TKey : notnull
        {
            created = !cache.TryGetValue(key, out TValue? result);
            if (created)
            {
                using ICacheEntry<TKey, TValue> entry = cache.CreateEntry(key);

                result = factory(entry);
                entry.Value = result;
            }
            return result;
        }

        public static async Task<TValue?> GetOrCreateAsync<TKey, TValue>(this IMemoryCache<TKey, TValue> cache, TKey key, Func<ICacheEntry<TKey, TValue>, Task<TValue?>> factory) where TKey : notnull
        {
            if (!cache.TryGetValue(key, out TValue? result))
            {
                using ICacheEntry<TKey, TValue> entry = cache.CreateEntry(key);

                result = await factory(entry).ConfigureAwait(false);
                entry.Value = result;
            }

            return result;
        }
    }
}
