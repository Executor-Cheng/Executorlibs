using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Dispatchers;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.MessageFramework.Builders
{
    public class RawdataDispatcherServiceBuilder<TClient, TRawdata, TDispatcher> : ParsingComponentBuilder<TClient, TRawdata, TDispatcher>
                                                                                   where TClient : class, IMessageClient
                                                                                   where TDispatcher : class, IRawdataDispatcher<TClient, TRawdata>
    {
        protected override ServiceLifetime DefaultLifetime => ServiceLifetime.Scoped;

        public RawdataDispatcherServiceBuilder(ParsingServiceBuilder<TClient, TRawdata> builder) : base(builder)
        {
            
        }

        public RawdataDispatcherServiceBuilder(RawdataDispatcherServiceBuilder<TClient, TRawdata, TDispatcher> builder) : base(builder)
        {
            
        }
    }
}
