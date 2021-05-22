using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.MessageFramework.Parsers;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    /// <summary>
    /// 以 <see cref="string"/> 作为键的 <see cref="IBilibiliMessageParser"/>
    /// </summary>
    public interface IMappableBilibiliMessageParser : IMappableMessageParser<string>, IBilibiliMessageParser
    {

    }

    /// <summary>
    /// 以 <see cref="string"/> 作为键的 <see cref="IBilibiliMessageParser{TMessage}"/>
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IMappableBilibiliMessageParser<TMessage> : IMappableMessageParser<string, JsonElement, TMessage>,
                                                                IBilibiliMessageParser<TMessage>,
                                                                IMappableBilibiliMessageParser where TMessage : IBilibiliMessage
    {

    }
}
