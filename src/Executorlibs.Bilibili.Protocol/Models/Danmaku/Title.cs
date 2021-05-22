using System;
using SharedITitle = Executorlibs.Shared.Protocol.Models.Danmaku.ITitle;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    /// <summary>
    /// 表示头衔信息的接口
    /// </summary>
    /// <remarks>
    /// 此类型为 Bilibili 直播平台 专用<para/>
    /// 继承自<see cref="Common.ITitle"/>
    /// </remarks>
    public interface ITitle : SharedITitle
    {
        /// <summary>
        /// 从Id
        /// </summary>
        int SubId { get; }
    }

    /// <summary>
    /// 表示头衔的类
    /// </summary>
    public class Title : ITitle
    {
        /// <inheritdoc/>
        public string Name { get; set; } = null!;

        /// <inheritdoc/>
        public int Id { get; set; }

        /// <inheritdoc/>
        public int SubId { get; set; }

        /// <summary>
        /// 尝试将格式为 title-x-x 的头衔字符串处理为 <see cref="Title"/>
        /// </summary>
        /// <param name="title">格式为 title-x-x 的头衔字符串</param>
        /// <returns>对应 <paramref name="title"/> 的一个 <see cref="Title"/> 实例。如果未能成功处理则返回 <see langword="null"/></returns>
        public static Title? Parse(string? title)
        {
            ReadOnlySpan<char> titleSpan = title;
            if (!titleSpan.IsEmpty)
            {
                int first = titleSpan.IndexOf('-') + 1,
                    last = titleSpan[first..].IndexOf('-') + first;
                if (first <= last &&
                    int.TryParse(titleSpan[first..last], out int id) &&
                    int.TryParse(titleSpan[(last + 1)..], out int subId)) // SC 的 Title 字符串是 "0"
                {
                    return new Title
                    {
                        Name = "", // 以后再写
                        Id = id,
                        SubId = subId
                    };
                }
            }
            return null;
        }
    }
}
