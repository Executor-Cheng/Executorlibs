using System;
using Executorlibs.MessageFramework.Parsers.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Executorlibs.Bilibili.Protocol.Invokers;

namespace Executorlibs.Bilibili.Protocol.Parsers.Attributes
{
    /// <summary>
    /// 标记一个 <see cref="BilibiliMessageHandlerInvoker"/> 所需要使用的 <see cref="IBilibiliMessageParserResolver"/>
    /// </summary>
    public class RegisterBilibiliParserResolverAttribute : RegisterParserResolverAttribute
    {
        /// <inheritdoc cref="RegisterBilibiliParserResolverAttribute(Type, ServiceLifetime?)"/>
        public RegisterBilibiliParserResolverAttribute(Type implementationType) : this(implementationType, null)
        {

        }

        /// <summary>
        /// 使用给定的 <paramref name="implementationType"/> 初始化 <see cref="RegisterBilibiliParserResolverAttribute"/> 的新实例
        /// </summary>
        /// <param name="implementationType"><see cref="IBilibiliMessageParserResolver"/> 的实现类类型</param>
        /// <param name="lifetime"><paramref name="implementationType"/> 的生命周期</param>
        public RegisterBilibiliParserResolverAttribute(Type implementationType, ServiceLifetime? lifetime) : base(implementationType, lifetime)
        {

        }

        protected override Type GetServiceType(Type implementationType)
        {
            var interfaceType = typeof(IBilibiliMessageParserResolver);
            if (interfaceType.IsAssignableFrom(implementationType))
            {
                return interfaceType;
            }
            throw new ArgumentException($"给定的 {implementationType.Name} 不实现 {interfaceType.Name}", nameof(implementationType));
        }
    }
}
