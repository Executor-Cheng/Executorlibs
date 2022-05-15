using System;
using Executorlibs.Huya.Protocol.Invokers;
using Executorlibs.MessageFramework.Parsers.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.Huya.Protocol.Parsers.Attributes
{
    /// <summary>
    /// 标记一个 <see cref="HuyaMessageHandlerInvoker"/> 所需要使用的 <see cref="IHuyaMessageParserResolver"/>
    /// </summary>
    public class RegisterHuyaMessageParserResolverAttribute : RegisterParserResolverAttribute
    {
        /// <inheritdoc cref="RegisterHuyaMessageParserResolverAttribute(Type, ServiceLifetime?)"/>
        public RegisterHuyaMessageParserResolverAttribute(Type implementationType) : this(implementationType, null)
        {

        }

        /// <summary>
        /// 使用给定的 <paramref name="implementationType"/> 初始化 <see cref="RegisterHuyaMessageParserResolverAttribute"/> 的新实例
        /// </summary>
        /// <param name="implementationType"><see cref="IHuyaMessageParserResolver"/> 的实现类类型</param>
        /// <param name="lifetime"><paramref name="implementationType"/> 的生命周期</param>
        public RegisterHuyaMessageParserResolverAttribute(Type implementationType, ServiceLifetime? lifetime) : base(implementationType, lifetime)
        {

        }

        protected override Type GetServiceType(Type implementationType)
        {
            var interfaceType = typeof(IHuyaMessageParserResolver);
            if (interfaceType.IsAssignableFrom(implementationType))
            {
                return interfaceType;
            }
            throw new ArgumentException($"给定的 {implementationType.Name} 不实现 {interfaceType.Name}", nameof(implementationType));
        }
    }
}
