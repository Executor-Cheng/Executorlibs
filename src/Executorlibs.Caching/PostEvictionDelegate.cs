namespace Microsoft.Extensions.Caching.Memory
{
    /// <summary>
    /// Signature of the callback which gets called when a cache entry expires.
    /// </summary>
    /// <param name="key">The key of the entry being evicted.</param>
    /// <param name="value">The value of the entry being evicted.</param>
    /// <param name="reason">The <see cref="EvictionReason"/>.</param>
    /// <param name="state">The information that was passed when registering the callback.</param>
    public delegate void PostEvictionDelegate<TKey, TValue>(TKey key, TValue? value, EvictionReason reason, object? state) where TKey : notnull;
}
