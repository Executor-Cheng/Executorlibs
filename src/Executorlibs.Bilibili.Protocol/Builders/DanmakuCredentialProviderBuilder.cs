using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Services;
using Executorlibs.MessageFramework.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.Bilibili.Protocol.Builders
{
    public class DanmakuCredentialProviderBuilder : ParsingComponentBuilder<IDanmakuClient, byte[], IDanmakuServerProvider>
    {
        protected override ServiceLifetime DefaultLifetime => ServiceLifetime.Scoped;

        public DanmakuCredentialProviderBuilder(ParsingServiceBuilder<IDanmakuClient, byte[]> builder) : base(builder)
        {
            
        }

        public DanmakuCredentialProviderBuilder(DanmakuCredentialProviderBuilder builder) : base(builder)
        {
            
        }
    }
}
