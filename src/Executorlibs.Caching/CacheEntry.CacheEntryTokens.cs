using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Caching.Memory
{
    internal partial class CacheEntry<TKey, TValue>
    {
        // this type exists just to reduce average CacheEntry size
        // which typically is not using expiration tokens or callbacks
        private sealed class CacheEntryTokens
        {
            private List<IChangeToken>? _expirationTokens;
            private List<IDisposable>? _expirationTokenRegistrations;
            private List<PostEvictionCallbackRegistration<TKey, TValue>>? _postEvictionCallbacks; // this is not really related to tokens, but was moved here to shrink typical CacheEntry size

            internal List<IChangeToken> ExpirationTokens => _expirationTokens ??= new List<IChangeToken>();
            internal List<PostEvictionCallbackRegistration<TKey, TValue>> PostEvictionCallbacks => _postEvictionCallbacks ??= new List<PostEvictionCallbackRegistration<TKey, TValue>>();

            internal void AttachTokens(CacheEntry<TKey, TValue> cacheEntry)
            {
                if (_expirationTokens != null)
                {
                    lock (this)
                    {
                        for (int i = 0; i < _expirationTokens.Count; i++)
                        {
                            IChangeToken expirationToken = _expirationTokens[i];
                            if (expirationToken.ActiveChangeCallbacks)
                            {
                                _expirationTokenRegistrations ??= new List<IDisposable>(1);
                                IDisposable registration = expirationToken.RegisterChangeCallback(ExpirationCallback, cacheEntry);
                                _expirationTokenRegistrations.Add(registration);
                            }
                        }
                    }
                }
            }

            internal bool CheckForExpiredTokens(CacheEntry<TKey, TValue> cacheEntry)
            {
                if (_expirationTokens != null)
                {
                    for (int i = 0; i < _expirationTokens.Count; i++)
                    {
                        IChangeToken expiredToken = _expirationTokens[i];
                        if (expiredToken.HasChanged)
                        {
                            cacheEntry.SetExpired(EvictionReason.TokenExpired);
                            return true;
                        }
                    }
                }
                return false;
            }

            internal bool CanPropagateTokens() => _expirationTokens != null;

            internal void PropagateTokens(CacheEntry<TKey, TValue> parentEntry)
            {
                if (_expirationTokens != null)
                {
                    lock (this)
                    {
                        lock (parentEntry.GetOrCreateTokens())
                        {
                            foreach (IChangeToken expirationToken in _expirationTokens)
                            {
                                parentEntry.AddExpirationToken(expirationToken);
                            }
                        }
                    }
                }
            }

            internal void DetachTokens()
            {
                // _expirationTokenRegistrations is not checked for null, because AttachTokens might initialize it under lock
                // instead we are checking for _expirationTokens, because if they are not null, then _expirationTokenRegistrations might also be not null
                if (_expirationTokens != null)
                {
                    lock (this)
                    {
                        List<IDisposable>? registrations = _expirationTokenRegistrations;
                        if (registrations != null)
                        {
                            _expirationTokenRegistrations = null;
                            for (int i = 0; i < registrations.Count; i++)
                            {
                                IDisposable registration = registrations[i];
                                registration.Dispose();
                            }
                        }
                    }
                }
            }

            internal void InvokeEvictionCallbacks(CacheEntry<TKey, TValue> cacheEntry)
            {
                if (_postEvictionCallbacks != null)
                {
                    Task.Factory.StartNew(state => InvokeCallbacks((CacheEntry<TKey, TValue>)state!), cacheEntry,
                        CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
                }
            }

            private static void InvokeCallbacks(CacheEntry<TKey, TValue> entry)
            {
                List<PostEvictionCallbackRegistration<TKey, TValue>>? callbackRegistrations = Interlocked.Exchange(ref entry._tokens!._postEvictionCallbacks, null);

                if (callbackRegistrations == null)
                {
                    return;
                }

                for (int i = 0; i < callbackRegistrations.Count; i++)
                {
                    PostEvictionCallbackRegistration<TKey, TValue> registration = callbackRegistrations[i];

                    try
                    {
                        registration.EvictionCallback?.Invoke(entry.Key, entry.Value, entry.EvictionReason, registration.State);
                    }
                    catch (Exception e)
                    {
                        // This will be invoked on a background thread, don't let it throw.
                        entry._cache._logger.LogError(e, "EvictionCallback invoked failed");
                    }
                }
            }
        }
    }
}
