using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Bilibili.Protocol.Models.Enums;
using Executorlibs.Shared;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers.OpenPlatform
{
    public class GuardBuyParser : BilibiliMappableJsonMessageParser<IGuardBuyMessage, GuardBuyMessage>
    {
        private const string Command = "LIVE_OPEN_PLATFORM_GUARD";

        public override string Key => Command;

        protected override GuardBuyMessage CreateMessage(JsonElement rawdata)
        {
            var message = base.CreateMessage(rawdata);
            var data = rawdata.GetProperty("data");
            var user = data.GetProperty("user_info");

            message.UserName = user.GetProperty("uname").GetString()!;
            message.UserId = user.GetProperty("uid").GetUInt64();

            message.Id = data.GetProperty("msg_id").GetUInt64();
            message.RoomId = data.GetProperty("room_id").GetUInt32();
            message.GuardType = (GuardType)data.GetProperty("guard_level").GetInt32();
            message.GiftName = message.GuardType switch
            {
                GuardType.Captain => "舰长",
                GuardType.Praefect => "提督",
                GuardType.Governor => "总督",
                GuardType.WeekCaptain => "周舰长",
                _ => "未知"
            };

            message.GiftCount = data.GetProperty("guard_num").GetUInt32();
            message.IsGoldGift = true;

            message.Time = Utils.UnixTime2DateTime(data.GetProperty("timestamp").GetInt32());
            return message;
        }

        /*
            {
                "cmd": "LIVE_OPEN_PLATFORM_GUARD",
                "data": {
                    "user_info": {
                        "uid": 110000331, //用户uid
                        "uname":"",//用户昵称
                        "uface": "http://i0.hdslb.com/bfs/face/4add3acfc930fcd07d06ea5e10a3a377314141c2.jpg" //用户头像
                    },
                    "guard_level": 3, //对应的大航海等级 1总督 2提督 3舰长
                    "guard_num": 1,
                    "guard_unit": "月", // (个月)
                    "fans_medal_level": 24, //粉丝勋章等级
                    "fans_medal_name": "aw4ifC", //粉丝勋章名
                    "fans_medal_wearing_status": false, //该房间粉丝勋章佩戴情况
                    "timestamp": 1653555128,
                    "room_id": 460695,
                    "msg_id":""//消息唯一id
                }
            }
         */
    }
}
