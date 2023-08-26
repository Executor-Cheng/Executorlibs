using System;
using Executorlibs.MessageFramework.Parsing.Parsers;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.MessageFramework.Parsing.Attributes
{
    /// <summary>
    /// 标记一个消息类、消息接口或者消息处理类所需要使用的 <see cref="IMessageParser{TRawdata, TMessage}"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
    public class RegisterParserAttribute : Attribute
    {
        public Type ParserType { get; }

        public ServiceLifetime? Lifetime { get; }

        /// <summary>
        /// 使用给定的 <see cref="IMessageParser{TRawdata, TMessage}"/> 初始化 <see cref="RegisterParserAttribute"/> 的新实例
        /// </summary>
        /// <param name="parserType"><see cref="IMessageParser{TRawdata, TMessage}"/> 的类型</param>
        public RegisterParserAttribute(Type parserType) : this(parserType, null)
        {

        }

        public RegisterParserAttribute(Type parserType, ServiceLifetime? lifetime) 
        {
            ParserType = parserType;
            Lifetime = lifetime;
        }
    }
}
