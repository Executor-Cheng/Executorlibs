using System;
using Executorlibs.MessageFramework.Parsers.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.Huya.Protocol.Parsers.Attributes
{

    /// <summary>
    /// 标记一个消息类、消息接口或者消息处理类所需要使用的 <see cref="IHuyaMessageParser{TMessage}"/>
    /// </summary>
    public sealed class RegisterHuyaParserAttribute : RegisterParserAttribute
    {
        /// <summary>
        /// 使用给定的 <see cref="IHuyaMessageParser{TMessage}"/> 初始化 <see cref="RegisterHuyaParserAttribute"/> 的新实例
        /// </summary>
        /// <param name="implementationType"><see cref="IHuyaMessageParser{TMessage}"/> 的类型</param>
        public RegisterHuyaParserAttribute(Type implementationType) : this(implementationType, null)
        {

        }

        public RegisterHuyaParserAttribute(Type implementationType, ServiceLifetime? lifetime) : base(implementationType, lifetime)
        {

        }

        protected override Type GetServiceType(Type implementationType)
        {
            Type openGeneric = typeof(IHuyaMessageParser<>);
            foreach (Type interfaceType in implementationType.GetInterfaces())
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == openGeneric)
                {
                    return typeof(IHuyaMessageParser);
                }
            }
            throw new ArgumentException($"给定的 {implementationType.Name} 不实现 {openGeneric.Name}", nameof(implementationType));
        }
    }
}
