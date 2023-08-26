using Executorlibs.MessageFramework.Handlers;

// see: https://source.dot.net/#System.Private.CoreLib/src/libraries/System.Private.CoreLib/src/System/Threading/CancellationTokenSource.cs,130a9536ac96b392
namespace Executorlibs.MessageFramework.Subscriptions
{
    public partial class DefaultMessageSubscription<TClient, TMessage>
    {
        protected internal sealed class RegistrationNode
        {
            public readonly Registrations Registrations;
            public long Id;

            public RegistrationNode? Prev;
            public RegistrationNode? Next;

            public IMessageHandler<TClient, TMessage>? Handler;

            public RegistrationNode(Registrations registrations)
            {
                Registrations = registrations;
            }
        }
    }
}
