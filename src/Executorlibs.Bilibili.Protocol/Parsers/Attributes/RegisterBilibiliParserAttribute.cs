using System;
using Executorlibs.MessageFramework.Parsers.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.Bilibili.Protocol.Parsers.Attributes
{

    /// <summary>
    /// 标记一个消息类、消息接口或者消息处理类所需要使用的 <see cref="IBilibiliMessageParser{TMessage}"/>
    /// </summary>
    public sealed class RegisterBilibiliParserAttribute : RegisterParserAttribute
    {
        /// <summary>
        /// 使用给定的 <see cref="IBilibiliMessageParser{TMessage}"/> 初始化 <see cref="RegisterBilibiliParserAttribute"/> 的新实例
        /// </summary>
        /// <param name="implementationType"><see cref="IBilibiliMessageParser{TMessage}"/> 的类型</param>
        public RegisterBilibiliParserAttribute(Type implementationType) : this(implementationType, null)
        {

        }

        public RegisterBilibiliParserAttribute(Type implementationType, ServiceLifetime? lifetime) : base(implementationType, lifetime)
        {

        }

        protected override Type GetServiceType(Type implementationType)
        {
            Type openGeneric = typeof(IBilibiliMessageParser<>);
            foreach (Type interfaceType in implementationType.GetInterfaces())
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == openGeneric)
                {
                    return typeof(IBilibiliMessageParser);
                }
            }
            throw new ArgumentException($"给定的 {implementationType.Name} 不实现 {openGeneric.Name}", nameof(implementationType));
        }
    }
}
