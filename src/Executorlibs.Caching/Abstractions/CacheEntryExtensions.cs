using System;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Caching.Memory
{
    public static class CacheEntryExtensions
    {
        /// <summary>
        /// Sets the priority for keeping the cache entry in the cache during a memory pressure tokened cleanup.
        /// </summary>
        /// <param name="entry">The entry to set the priority for.</param>
        /// <param name="priority">The <see cref="CacheItemPriority"/> to set on the entry.</param>
        /// <returns>The <see cref="ICacheEntry{TKey, TValue}"/> for chaining.</returns>
        public static ICacheEntry<TKey, TValue> SetPriority<TKey, TValue>(
            this ICacheEntry<TKey, TValue> entry,
            CacheItemPriority priority) where TKey : notnull
        {
            entry.Priority = priority;
            return entry;
        }

        /// <summary>
        /// Expire the cache entry if the given <see cref="IChangeToken"/> expires.
        /// </summary>
        /// <param name="entry">The <see cref="ICacheEntry{TKey, TValue}"/>.</param>
        /// <param name="expirationToken">The <see cref="IChangeToken"/> that causes the cache entry to expire.</param>
        /// <returns>The <see cref="ICacheEntry{TKey, TValue}"/> for chaining.</returns>
        public static ICacheEntry<TKey, TValue> AddExpirationToken<TKey, TValue>(
            this ICacheEntry<TKey, TValue> entry,
            IChangeToken expirationToken) where TKey : notnull
        {
            if (expirationToken == null)
            {
                throw new ArgumentNullException(nameof(expirationToken));
            }

            entry.ExpirationTokens.Add(expirationToken);
            return entry;
        }

        /// <summary>
        /// Sets an absolute expiration time, relative to now.
        /// </summary>
        /// <param name="entry">The <see cref="ICacheEntry{TKey, TValue}"/>.</param>
        /// <param name="relative">The <see cref="TimeSpan"/> representing the expiration time relative to now.</param>
        /// <returns>The <see cref="ICacheEntry{TKey, TValue}"/> for chaining.</returns>
        public static ICacheEntry<TKey, TValue> SetAbsoluteExpiration<TKey, TValue>(
            this ICacheEntry<TKey, TValue> entry,
            TimeSpan relative) where TKey : notnull
        {
            entry.AbsoluteExpirationRelativeToNow = relative;
            return entry;
        }

        /// <summary>
        /// Sets an absolute expiration date for the cache entry.
        /// </summary>
        /// <param name="entry">The <see cref="ICacheEntry{TKey, TValue}"/>.</param>
        /// <param name="absolute">A <see cref="DateTimeOffset"/> representing the expiration time in absolute terms.</param>
        /// <returns>The <see cref="ICacheEntry{TKey, TValue}"/> for chaining.</returns>
        public static ICacheEntry<TKey, TValue> SetAbsoluteExpiration<TKey, TValue>(
            this ICacheEntry<TKey, TValue> entry,
            DateTimeOffset absolute) where TKey : notnull
        {
            entry.AbsoluteExpiration = absolute;
            return entry;
        }

        /// <summary>
        /// Sets how long the cache entry can be inactive (e.g. not accessed) before it will be removed.
        /// This will not extend the entry lifetime beyond the absolute expiration (if set).
        /// </summary>
        /// <param name="entry">The <see cref="ICacheEntry{TKey, TValue}"/>.</param>
        /// <param name="offset">A <see cref="TimeSpan"/> representing a sliding expiration.</param>
        /// <returns>The <see cref="ICacheEntry{TKey, TValue}"/> for chaining.</returns>
        public static ICacheEntry<TKey, TValue> SetSlidingExpiration<TKey, TValue>(
            this ICacheEntry<TKey, TValue> entry,
            TimeSpan offset) where TKey : notnull
        {
            entry.SlidingExpiration = offset;
            return entry;
        }

        /// <summary>
        /// The given callback will be fired after the cache entry is evicted from the cache.
        /// </summary>
        /// <param name="entry">The <see cref="ICacheEntry{TKey, TValue}"/>.</param>
        /// <param name="callback">The callback to run after the entry is evicted.</param>
        /// <returns>The <see cref="ICacheEntry{TKey, TValue}"/> for chaining.</returns>
        public static ICacheEntry<TKey, TValue> RegisterPostEvictionCallback<TKey, TValue>(
            this ICacheEntry<TKey, TValue> entry,
            PostEvictionDelegate<TKey, TValue> callback) where TKey : notnull
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            return entry.RegisterPostEvictionCallback(callback, state: null);
        }

        /// <summary>
        /// The given callback will be fired after the cache entry is evicted from the cache.
        /// </summary>
        /// <param name="entry">The <see cref="ICacheEntry{TKey, TValue}"/>.</param>
        /// <param name="callback">The callback to run after the entry is evicted.</param>
        /// <param name="state">The state to pass to the post-eviction callback.</param>
        /// <returns>The <see cref="ICacheEntry{TKey, TValue}"/> for chaining.</returns>
        public static ICacheEntry<TKey, TValue> RegisterPostEvictionCallback<TKey, TValue>(
            this ICacheEntry<TKey, TValue> entry,
            PostEvictionDelegate<TKey, TValue> callback,
            object? state) where TKey : notnull
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            entry.PostEvictionCallbacks.Add(new PostEvictionCallbackRegistration<TKey, TValue>()
            {
                EvictionCallback = callback,
                State = state
            });
            return entry;
        }

        /// <summary>
        /// Sets the value of the cache entry.
        /// </summary>
        /// <param name="entry">The <see cref="ICacheEntry{TKey, TValue}"/>.</param>
        /// <param name="value">The value to set on the <paramref name="entry"/>.</param>
        /// <returns>The <see cref="ICacheEntry{TKey, TValue}"/> for chaining.</returns>
        public static ICacheEntry<TKey, TValue> SetValue<TKey, TValue>(
            this ICacheEntry<TKey, TValue> entry,
            TValue value) where TKey : notnull
        {
            entry.Value = value;
            return entry;
        }

        /// <summary>
        /// Sets the size of the cache entry value.
        /// </summary>
        /// <param name="entry">The <see cref="ICacheEntry{TKey, TValue}"/>.</param>
        /// <param name="size">The size to set on the <paramref name="entry"/>.</param>
        /// <returns>The <see cref="ICacheEntry{TKey, TValue}"/> for chaining.</returns>
        public static ICacheEntry<TKey, TValue> SetSize<TKey, TValue>(
            this ICacheEntry<TKey, TValue> entry,
            long size) where TKey : notnull
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size), size, $"{nameof(size)} must be non-negative.");
            }

            entry.Size = size;
            return entry;
        }

        /// <summary>
        /// Applies the values of an existing <see cref="MemoryCacheEntryOptions"/> to the entry.
        /// </summary>
        /// <param name="entry">The <see cref="ICacheEntry{TKey, TValue}"/>.</param>
        /// <param name="options">Set the values of these options on the <paramref name="entry"/>.</param>
        /// <returns>The <see cref="ICacheEntry{TKey, TValue}"/> for chaining.</returns>
        public static ICacheEntry<TKey, TValue> SetOptions<TKey, TValue>(this ICacheEntry<TKey, TValue> entry, MemoryCacheEntryOptions<TKey, TValue> options) where TKey : notnull
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            entry.AbsoluteExpiration = options.AbsoluteExpiration;
            entry.AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow;
            entry.SlidingExpiration = options.SlidingExpiration;
            entry.Priority = options.Priority;
            entry.Size = options.Size;

            foreach (IChangeToken expirationToken in options.ExpirationTokens)
            {
                entry.AddExpirationToken(expirationToken);
            }

            foreach (PostEvictionCallbackRegistration<TKey, TValue> postEvictionCallback in options.PostEvictionCallbacks)
            {
                entry.RegisterPostEvictionCallback(postEvictionCallback.EvictionCallback, postEvictionCallback.State);
            }

            return entry;
        }
    }
}
