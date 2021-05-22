using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Shared;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    public class PkStartParser : PkStartParser<IPkStartMessage, PkStartMessage>
    {
        /// <summary>
        /// 初始化 <see cref="PkStartParser"/> 类的新实例
        /// </summary>
        public PkStartParser() { }
    }

    public class PkStartParser<TMessage, TImpl> : PkBaseParser<TMessage, TImpl> where TMessage : IPkStartMessage
                                                                                where TImpl : PkStartMessage, TMessage, new()
    {
        private const string Command = "PK_BATTLE_START";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="PkStartParser{TMessage, TImpl}"/> 类的新实例
        /// </summary>
        public PkStartParser() { }

        /// <inheritdoc/>
        public override TMessage Parse(in JsonElement root)
        {
            TImpl message = (TImpl)base.Parse(root);
            message.EndTime = Utils.UnixTime2DateTime(root.GetProperty("data").GetProperty("pk_end_time").GetInt32());
            return message;
        }
    }
}
