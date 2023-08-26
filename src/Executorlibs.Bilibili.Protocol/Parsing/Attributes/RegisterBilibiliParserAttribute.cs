using System;
using Executorlibs.MessageFramework.Parsing.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers.Attributes
{
    /// <summary>
    /// 标记一个消息类、消息接口或者消息处理类所需要使用的 <see cref="IBilibiliJsonMessageParser{TMessage}"/>
    /// </summary>
    public sealed class RegisterBilibiliParserAttribute : RegisterParserAttribute
    {
        /// <summary>
        /// 使用给定的 <see cref="IBilibiliJsonMessageParser{TMessage}"/> 初始化 <see cref="RegisterBilibiliParserAttribute"/> 的新实例
        /// </summary>
        /// <param name="parserType"><see cref="IBilibiliJsonMessageParser{TMessage}"/> 的类型</param>
        public RegisterBilibiliParserAttribute(Type parserType) : this(parserType, null)
        {

        }

        public RegisterBilibiliParserAttribute(Type parserType, ServiceLifetime? lifetime) : base(parserType, lifetime)
        {

        }
    }
}
