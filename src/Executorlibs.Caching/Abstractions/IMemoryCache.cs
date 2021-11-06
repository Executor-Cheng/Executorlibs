using System;

namespace Microsoft.Extensions.Caching.Memory
{
    /// <summary>
    /// Represents a local in-memory cache whose values are not serialized.
    /// </summary>
    public interface IMemoryCache<TKey, TValue> : IDisposable where TKey : notnull
    {
        /// <summary>
        /// Gets the item associated with this key if present.
        /// </summary>
        /// <param name="key">An object identifying the requested entry.</param>
        /// <param name="value">The located value or null.</param>
        /// <returns>True if the key was found.</returns>
        bool TryGetValue(TKey key, out TValue? value);

        /// <summary>
        /// Create or overwrite an entry in the cache.
        /// </summary>
        /// <param name="key">An object identifying the entry.</param>
        /// <returns>The newly created <see cref="ICacheEntry"/> instance.</returns>
        ICacheEntry<TKey, TValue> CreateEntry(TKey key);

        /// <summary>
        /// Removes the object associated with the given key.
        /// </summary>
        /// <param name="key">An object identifying the entry.</param>
        void Remove(TKey key);
    }
}
