using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Bilibili.Protocol.Models.Enums;
using Executorlibs.Shared;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers.OpenPlatform
{
    public class SuperChatParser : BilibiliMappableJsonMessageParser<ISuperChatMessage, SuperChatMessage>
    {
        private const string Command = "LIVE_OPEN_PLATFORM_SUPER_CHAT";

        public override string Key => Command;

        protected override SuperChatMessage CreateMessage(JsonElement rawdata)
        {
            var message = base.CreateMessage(rawdata);
            var data = rawdata.GetProperty("data");

            message.Id = data.GetProperty("message_id").GetUInt64();
            message.RoomId = data.GetProperty("room_id").GetUInt32();
            message.UserName = data.GetProperty("uname").GetString()!;
            message.UserId = data.GetProperty("uid").GetUInt64();
            message.GuardType = (GuardType)data.GetProperty("guard_level").GetInt32();
            message.Time = Utils.UnixTime2DateTime(data.GetProperty("start_time").GetInt32());

            message.GiftSeedPrice = data.GetProperty("rmb").GetUInt32() * 1000u;

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
         * {
                "cmd":"LIVE_OPEN_PLATFORM_SUPER_CHAT",
                "data":{
                    "room_id":1,//直播间id
                    "uid":0,//购买用户UID
                    "uname":"",//购买的用户昵称
                    "uface":"",//购买用户头像
                    "message_id":0,//留言id(风控场景下撤回留言需要)
                    "message":"",//留言内容
                    "msg_id":"",//消息唯一id
                    "rmb":0,//支付金额(元)
                    "timestamp":0,//赠送时间秒级
                    "start_time":0,//生效开始时间
                    "end_time":0,//生效结束时间
                    "guard_level": 2, //对应房间大航海登记    (新增) 
                    "fans_medal_level": 26, //对应房间勋章信息  (新增) 
                    "fans_medal_name": "aw4ifC", //对应房间勋章名字  (新增) 
                    "fans_medal_wearing_status": true //该房间粉丝勋章佩戴情况   (新增)
                }
            }
         */
    }
}
