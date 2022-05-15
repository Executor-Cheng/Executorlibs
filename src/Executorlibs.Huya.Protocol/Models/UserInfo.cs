using System;
using Executorlibs.TarProtocol.IO;
using Executorlibs.TarProtocol.Models;
using Executorlibs.TarProtocol.Models.Primitives;

namespace Executorlibs.Huya.Protocol.Models
{
    /// <summary>
    /// 表示用户的基本信息。即用户名和用户Id
    /// </summary>
    /// <remarks>
    /// 对应 UidNickName
    /// </remarks>
    public class UserInfo : ITarType
    {
        public long UserId;

        public string UserName = null!;

        public void ReadFrom(ref TarReader reader)
        {
            UserId = reader.Read<TarInt64>();
            UserName = reader.Read<TarString>();
        }

        public void WriteTo(ref TarWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
