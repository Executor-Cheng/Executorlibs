using System;
using System.Linq;
using Executorlibs.TarProtocol.IO;
using Executorlibs.TarProtocol.Models;
using Executorlibs.TarProtocol.Models.Primitives;

namespace Executorlibs.Huya.Protocol.Models
{
    /// <summary>
    /// 表示弹幕基本信息
    /// </summary>
    /// <remarks>
    /// 对应 MessageNotice
    /// </remarks>
    public class CommentMessage : ITarType
    {
        public MessageSenderInfo Sender = null!;

        public long Tid;

        public long Sid;

        public string Content = null!;

        public int ShowMode;

        public MessageFormat Format = null!;

        public MessageBulletFormat BulletFormat = null!;

        public int TermType;

        public PerSuffixInfo[] DecorationPrefix = null!; // ?

        public PerSuffixInfo[] DecorationSuffix = null!; // ?

        public UserInfo[] AtSomeone = null!;

        public long Pid;

        public PerSuffixInfo[] BulletPrefix = null!;

        public string IconUrl = null!;

        public int Type;

        public PerSuffixInfo[] BulletSuffix = null!;

        public TagInfo[] TagInfo = null!;

        public void ReadFrom(ref TarReader reader)
        {
            Sender = reader.Read<TarStruct<MessageSenderInfo>>();
            Tid = reader.Read<TarInt64>();
            Sid = reader.Read<TarInt64>();
            Content = reader.Read<TarString>();
            ShowMode = reader.Read<TarInt32>();
            Format = reader.Read<TarStruct<MessageFormat>>();
            BulletFormat = reader.Read<TarStruct<MessageBulletFormat>>();
            TermType = reader.Read<TarInt32>();
            DecorationPrefix = reader.Read<TarList<TarStruct<PerSuffixInfo>>>().Value.Select(p => p.Value).ToArray();
            DecorationSuffix = reader.Read<TarList<TarStruct<PerSuffixInfo>>>().Value.Select(p => p.Value).ToArray();
            AtSomeone = reader.Read<TarList<TarStruct<UserInfo>>>().Value.Select(p => p.Value).ToArray();
            Pid = reader.Read<TarInt64>();
            BulletPrefix = reader.Read<TarList<TarStruct<PerSuffixInfo>>>().Value.Select(p => p.Value).ToArray();
            IconUrl = reader.Read<TarString>();
            Type = reader.Read<TarInt32>();
            BulletSuffix = reader.Read<TarList<TarStruct<PerSuffixInfo>>>().Value.Select(p => p.Value).ToArray();
            TagInfo = reader.Read<TarList<TarStruct<TagInfo>>>().Value.Select(p => p.Value).ToArray();
        }

        public void WriteTo(ref TarWriter writer)
        {
            throw new NotSupportedException();
        }
    }

    public class VerifyCookieRequest : ITarType
    {
        public long UserId;

        public string? UserAgent; // = "webh5&2008051712&websocket"

        public string? Cookie;

        public string? Guid;

        public byte AutoRegisterUserId; // = 1

        public string? AppSource; // = "HUYA&ZH&2052"

        public void ReadFrom(ref TarReader reader)
        {
            UserId = reader.Read<TarInt64>();
            UserAgent = reader.Read<TarString>();
            Cookie = reader.Read<TarString>();
            Guid = reader.Read<TarString>();
            AutoRegisterUserId = reader.Read<TarByte>();
            AppSource = reader.Read<TarString>();
        }

        public void WriteTo(ref TarWriter writer)
        {
            //ITarType userId = new TarInt64(0, UserId),
            //         userAgent = new TarString(1, UserAgent),
            //         cookie = new TarString(2, Cookie),
            //         guid = new TarString(3, Guid),
            //         autoRegisterUserId = new TarByte(4, AutoRegisterUserId),
            //         appSource = new TarString(5, AppSource);
            //userId.WriteTo(ref writer);
            //userAgent.WriteTo(ref writer);
            //cookie.WriteTo(ref writer);
            //guid.WriteTo(ref writer);
            //autoRegisterUserId.WriteTo(ref writer);
            //appSource.WriteTo(ref writer);
            new TarInt64(0, UserId).WriteTo(ref writer);
            new TarString(1, UserAgent).WriteTo(ref writer);
            new TarString(2, Cookie).WriteTo(ref writer);
            new TarString(3, Guid).WriteTo(ref writer);
            new TarByte(4, AutoRegisterUserId).WriteTo(ref writer);
            new TarString(5, AppSource).WriteTo(ref writer);
        }
    }
}
