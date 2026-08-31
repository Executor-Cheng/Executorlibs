using System.Text.Json.Serialization;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.Shared.JsonConverters;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    /// <summary>
    /// 表示具有头衔信息的消息
    /// </summary>
    public interface ITitleMessage : IBilibiliJsonMessage
    {
        /// <summary>
        /// 头衔信息
        /// </summary>
        [JsonConverter(typeof(ChangeTypeJsonConverter<Title, ITitle>))]
        ITitle? Title { get; }
    }
}
