using System;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.General;
using ISharedPopularityMessage = Executorlibs.Shared.Protocol.Models.Danmaku.IPopularityMessage;
using SharedPopularityMessage = Executorlibs.Shared.Protocol.Models.Danmaku.PopularityMessage;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    public interface IPopularityMessage : IBilibiliMessage, ISharedPopularityMessage
    {

    }

    public class PopularityMessage : SharedPopularityMessage, IPopularityMessage
    {
        public JsonElement Rawdata => throw new NotImplementedException();
    }
}
