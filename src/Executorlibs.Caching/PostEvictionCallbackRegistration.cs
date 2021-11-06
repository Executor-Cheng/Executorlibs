namespace Microsoft.Extensions.Caching.Memory
{
    public class PostEvictionCallbackRegistration<TKey, TValue> where TKey : notnull
    {
        public PostEvictionDelegate<TKey, TValue> EvictionCallback { get; set; } = null!;

        public object? State { get; set; }
    }
}
