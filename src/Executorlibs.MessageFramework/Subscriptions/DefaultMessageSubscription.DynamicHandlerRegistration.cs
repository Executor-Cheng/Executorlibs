using System;

// see: https://source.dot.net/#System.Private.CoreLib/CancellationTokenRegistration.cs,2d80bf84f593cc0c
namespace Executorlibs.MessageFramework.Subscriptions
{
    public partial class DefaultMessageSubscription<TClient, TMessage>
    {
        public readonly struct DynamicHandlerRegistration : IEquatable<DynamicHandlerRegistration>, IDisposable
        {
            private readonly long _id;

            private readonly RegistrationNode _node;

            internal DynamicHandlerRegistration(long id, RegistrationNode node)
            {
                _id = id;
                _node = node;
            }

            public void Dispose()
            {
                if (_node is RegistrationNode node)
                {
                    _node.Registrations.Unregister(_id, node);
                }
            }

            public static bool operator ==(DynamicHandlerRegistration left, DynamicHandlerRegistration right) => left.Equals(right);

            public static bool operator !=(DynamicHandlerRegistration left, DynamicHandlerRegistration right) => !left.Equals(right);

            public override bool Equals(object? obj) => obj is DynamicHandlerRegistration other && Equals(other);

            public bool Equals(DynamicHandlerRegistration other) => _node == other._node && _id == other._id;

            public override int GetHashCode() => _node != null ? _node.GetHashCode() ^ _id.GetHashCode() : _id.GetHashCode();
        }
    }
}
