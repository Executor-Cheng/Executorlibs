using System.Collections.Generic;
using Executorlibs.MessageFramework.Invoking;

namespace Executorlibs.Bilibili.Protocol.Clients
{
    public struct PluginResistration
    {
        internal LinkedList<DynamicHandlerRegistration>? _registrations;

        public void Dispose()
        {
            if (_registrations is LinkedList<DynamicHandlerRegistration> registrations)
            {
                foreach (var registration in registrations)
                {
                    registration.Dispose();
                }
            }
        }
    }
}
