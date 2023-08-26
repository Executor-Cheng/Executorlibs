using Executorlibs.MessageFramework.Handlers;
using System.Collections;
using System.Collections.Generic;

namespace Executorlibs.MessageFramework.Subscriptions
{
    public partial class DefaultMessageSubscription<TClient, TMessage>
    {
        public struct Enumerator : IEnumerator<IMessageHandler<TClient, TMessage>>
        {
            private readonly DefaultMessageSubscription<TClient, TMessage> _subscription;

            private IMessageHandler<TClient, TMessage>? _current;

            private int _staticIdx;

            private RegistrationNode? _dynamicNode;

            public Enumerator(DefaultMessageSubscription<TClient, TMessage> subscription)
            {
                _subscription = subscription;
                _current = null;
                _staticIdx = 0;
                _dynamicNode = subscription._registrations?.EffictiveNodeList;
            }

            public IMessageHandler<TClient, TMessage> Current => _current!;

            object IEnumerator.Current => _current!;

            public bool MoveNext()
            {
                if (_staticIdx < _subscription._staticHandlers.Length)
                {
                    _current = _subscription._staticHandlers[_staticIdx++];
                    return true;
                }
                if (_dynamicNode != null)
                {
                    _current = _dynamicNode.Handler;
                    _dynamicNode = _dynamicNode.Next;
                    return _current != null;
                }
                return false;
            }

            public void Reset()
            {
                _current = null;
                _staticIdx = 0;
                _dynamicNode = _subscription._registrations?.EffictiveNodeList;
            }

            public void Dispose()
            {

            }
        }
    }
}
