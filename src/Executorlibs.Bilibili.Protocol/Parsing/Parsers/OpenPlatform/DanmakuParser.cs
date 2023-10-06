using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Bilibili.Protocol.Models.Enums;
using Executorlibs.Shared;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers.OpenPlatform
{
    public class DanmakuParser : BilibiliMappableJsonMessageParser<IDanmakuMessage, DanmakuMessage>
    {
        private const string Command = "LIVE_OPEN_PLATFORM_DM";

        public override string Key => Command;

        protected override DanmakuMessage CreateMessage(JsonElement rawdata)
        {
            var message = base.CreateMessage(rawdata);
            var data = rawdata.GetProperty("data");
            message.Id = data.GetProperty("msg_id").GetUInt64();
            message.RoomId = data.GetProperty("room_id").GetUInt32();
            message.UserName = data.GetProperty("uname").GetString()!;
            message.UserId = data.GetProperty("uid").GetUInt64();
            message.Comment = data.GetProperty("msg").GetString()!;
            message.GuardType = (GuardType)data.GetProperty("guard_level").GetInt32();
            message.Time = Utils.UnixTime2DateTime(data.GetProperty("timestamp").GetInt32());
            if (data.GetProperty("fans_medal_wearing_status").GetBoolean())
            {
                var medal = new Medal()
                {
                    Level = data.GetProperty("fans_medal_level").GetUInt32(),
                    Name = data.GetProperty("fans_medal_name").GetString()!,
                    RoomId = message.RoomId
                };
                message.Medal = medal;
            }
            return message;
        }

        /*
           {
                "cmd":"LIVE_OPEN_PLATFORM_DM",
                "data":{
                    "room_id":1,//弹幕接收的直播间
                    "uid":0,//用户UID
                    "uname":"",//用户昵称
                    "msg":"",//弹幕内容
                    "msg_id":"",//消息唯一id
                    "fans_medal_level":0,//对应房间勋章信息
                    "fans_medal_name":"粉丝勋章名",
                    "fans_medal_wearing_status": true,//该房间粉丝勋章佩戴情况
                    "guard_level":0,//对应房间大航海 1总督 2提督 3舰长
                    "timestamp":0,//弹幕发送时间秒级时间戳
                    "uface":""//用户头像   
                    "emoji_img_url": "", //表情包图片地址
                    "dm_type": 0,//弹幕类型 0：普通弹幕 1：表情包弹幕
                }
            }
         */
    }
}
