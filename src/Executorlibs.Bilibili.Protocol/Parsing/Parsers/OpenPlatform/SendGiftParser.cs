using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Bilibili.Protocol.Models.Enums;
using Executorlibs.Shared;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers.OpenPlatform
{
    public class SendGiftParser : BilibiliMappableJsonMessageParser<ISendGiftMessage, SendGiftMessage>
    {
        private const string Command = "LIVE_OPEN_PLATFORM_SEND_GIFT";

        public override string Key => Command;

        protected override SendGiftMessage CreateMessage(JsonElement rawdata)
        {
            var message = base.CreateMessage(rawdata);
            var data = rawdata.GetProperty("data");
            message.Id = data.GetProperty("msg_id").GetUInt64();
            message.RoomId = data.GetProperty("room_id").GetUInt32();
            message.UserName = data.GetProperty("uname").GetString()!;
            message.UserId = data.GetProperty("uid").GetUInt64();
            message.GuardType = (GuardType)data.GetProperty("guard_level").GetInt32();
            message.Time = Utils.UnixTime2DateTime(data.GetProperty("timestamp").GetInt32());

            message.GiftId = data.GetProperty("gift_id").GetUInt32();
            message.GiftName = data.GetProperty("gift_name").GetString()!;
            message.GiftCount = data.GetProperty("gift_num").GetUInt32();
            message.GiftSeedPrice = data.GetProperty("price").GetUInt32();
            message.IsGoldGift = data.GetProperty("paid").GetBoolean();
            message.IsFree = !message.IsGoldGift;

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
            "cmd":"LIVE_OPEN_PLATFORM_SEND_GIFT",
            "data":{
                "room_id":1,//直播间(演播厅模式则为演播厅直播间,非演播厅模式则为收礼直播间)
                "uid":0,//送礼用户UID
                "uname":"",//送礼用户昵称
                "uface":"",//送礼用户头像
                "gift_id":0,//道具id(盲盒:爆出道具id)
                "gift_name":"",//道具名(盲盒:爆出道具名)
                "gift_num":0,//赠送道具数量
                "price":0,//礼物单价(1000 = 1元 = 10电池),盲盒:爆出道具的价值
                "paid":false,//是否是付费道具
                "fans_medal_level":0,//实际收礼人的勋章信息
                "fans_medal_name":"粉丝勋章名", //粉丝勋章名
                "fans_medal_wearing_status": true,//该房间粉丝勋章佩戴情况
                "guard_level":0,//room_id对应的大航海等级
                "timestamp":0,//收礼时间秒级时间戳
                "msg_id":"",//消息唯一id
                "anchor_info":{
                    "uid":0,//收礼主播uid
                    "uname":"",//收礼主播昵称
                    "uface":"http://i0.hdslb.com/bfs/face/4add3acfc930fcd07d06ea5e10a3a377314141c2.jpg"//收礼主播头像
                },
                "gift_icon":"http://i1.hdslb.com/dksldksldksld.jpg", //道具icon  （新增）
                "combo_gift":true,//是否是combo道具
                "combo_info":{ //ex：连击次数100，每个连击是批量送5个 既  5 * 100
                    "combo_base_num": 5,//每次连击赠送的道具数量
                    "combo_count":100,//连击次数
                    "combo_id":"xxxxxx",//连击id
                    "combo_timeout": 3,//连击有效期秒
                }
            }
        }
         */
    }
}
