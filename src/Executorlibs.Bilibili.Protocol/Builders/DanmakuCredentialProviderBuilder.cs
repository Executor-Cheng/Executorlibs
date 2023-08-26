using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Services;
using Executorlibs.MessageFramework.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.Bilibili.Protocol.Builders
{
    public class DanmakuCredentialProviderBuilder : ParsingComponentBuilder<IDanmakuClient, byte[], IDanmakuServerProvider>
    {
        protected override ServiceLifetime ComponentLifetime => ServiceLifetime.Scoped;

        public DanmakuCredentialProviderBuilder(ParsingServiceBuilder<IDanmakuClient, byte[]> builder) : base(builder, new ServiceBuilder<IDanmakuServerProvider>(builder.Services))
        {
            
        }

        public DanmakuCredentialProviderBuilder(DanmakuCredentialProviderBuilder builder) : base(builder)
        {
            
        }

        //public virtual DanmakuCredentialProviderBuilder AddDanmakuCredentialProvider<TProvider>(ServiceLifetime lifetime = ServiceLifetime.Scoped) where TProvider : class, IDanmakuServerProvider
        //{
        //    _providerBuilder.AddService<TProvider>(lifetime);
        //    return this;
        //}

        //public virtual DanmakuCredentialProviderBuilder AddDanmakuCredentialProvider(IDanmakuServerProvider instance)
        //{
        //    _providerBuilder.AddService(instance);
        //    return this;
        //}

        //public virtual DanmakuCredentialProviderBuilder AddDanmakuCredentialProvider(Func<IServiceProvider, IDanmakuServerProvider> factory, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        //{
        //    _providerBuilder.AddService(factory, lifetime);
        //    return this;
        //}
    }
}
