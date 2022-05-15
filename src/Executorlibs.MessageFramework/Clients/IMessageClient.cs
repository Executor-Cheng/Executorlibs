using Executorlibs.MessageFramework.Handlers;
using Executorlibs.MessageFramework.Invoking;

namespace Executorlibs.MessageFramework.Clients
{
    public interface IMessageClient
    {
        PluginResistration AddPlugin(IMessageHandler handler);
    }
}
