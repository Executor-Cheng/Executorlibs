using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Dispatchers;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.MessageFramework.Builders
{
    public class RawdataDispatcherServiceBuilder<TClient, TRawdata, TDispatcher> : ParsingComponentBuilder<TClient, TRawdata, TDispatcher>
                                                                                   where TClient : class, IMessageClient
                                                                                   where TDispatcher : class, IRawdataDispatcher<TClient, TRawdata>
    {
        protected override ServiceLifetime ComponentLifetime => ServiceLifetime.Scoped;

        public RawdataDispatcherServiceBuilder(ParsingServiceBuilder<TClient, TRawdata> builder) : base(builder, new ServiceBuilder<TDispatcher>(builder.Services))
        {
            
        }

        public RawdataDispatcherServiceBuilder(RawdataDispatcherServiceBuilder<TClient, TRawdata, TDispatcher> builder) : base(builder)
        {
            
        }

        //public virtual RawdataDispatcherServiceBuilder<TClient, TRawdata, TDispatcher> AddDispatcher<TDispatcherImpl>(ServiceLifetime lifetime = ServiceLifetime.Scoped) where TDispatcherImpl : class, TDispatcher
        //{
        //    _dispatcherBuilder.AddService<TDispatcherImpl>(lifetime);
        //    return this;
        //}

        //public virtual RawdataDispatcherServiceBuilder<TClient, TRawdata, TDispatcher> AddDispatcher(TDispatcher dispatcherInstance)
        //{
        //    _dispatcherBuilder.AddService(dispatcherInstance);
        //    return this;
        //}

        //public virtual RawdataDispatcherServiceBuilder<TClient, TRawdata, TDispatcher> AddDispatcher(Func<IServiceProvider, TDispatcher> factory, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        //{
        //    _dispatcherBuilder.AddService(factory, lifetime);
        //    return this;
        //}
    }
}
