using System.Globalization;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Bilibili.Protocol.Models.Enums;
using Executorlibs.Shared;
using Executorlibs.Shared.Extensions;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    /// <summary>
    /// 处理普通弹幕的 <see cref="IBilibiliJsonMessageParser{TMessage}"/>
    /// </summary>
    public abstract class DanmakuParserBase<TMessage, TImpl> : BilibiliJsonMessageParser<TMessage, TImpl> where TMessage : IDanmakuMessage
                                                                                                          where TImpl : DanmakuMessage, TMessage, new()
    {
        protected const string Command = "DANMU_MSG";

        protected override TImpl CreateMessage(JsonElement rawdata)
        {
            var message = base.CreateMessage(rawdata);
            JsonElement info = rawdata.GetProperty("info"),
                        info_0 = info[0],
                        info_2 = info[2],
                        info_3 = info[3],
                        info_4 = info[4],
                        info_5 = info[5];
            uint id = uint.Parse(info[9].GetProperty("ct").GetString()!, NumberStyles.HexNumber);
            message.Time = Utils.UnixTime2DateTime(info_0[4].GetInt64());
            message.IsLotteryDanmaku = info_0[9].GetInt32() > 0;
            message.Comment = info[1].GetString()!;
            message.UserId = info_2[0].GetUInt64();
            message.UserName = info_2[1].GetString()!;
            message.IsAdmin = info_2[2].GetInt32() == 1;
            message.LordType = info_2[3].GetInt32() == 1 ? (info_2[4].GetInt32() == 1 ? LordType.Yearly : LordType.Monthly) : LordType.None;
            message.Medal = info_3.HasValues() ? new Medal
            {
                Level = info_3[0].GetUInt32(),
                Name = info_3[1].GetString()!,
                Master = info_3[2].GetString()!,
                MasterId = info_3[12].GetUInt64(),
                RoomId = info_3[3].GetUInt32(),
                Color = info_3[4].GetUInt32(),
                // Badge = null 以后再整
            } : null;
            message.Level = info_4[0].GetUInt32();
            message.Rank = info_4[3].ValueKind == JsonValueKind.Number ? info_4[3].GetUInt32() : default(uint?);
            message.Title = info_5.HasValues() ? Title.Parse(info_5[1].GetString()) : null;
            message.GuardType = (GuardType)info[7].GetInt32();
            message.Id = id;
            message.Token = id;
            message.Mode = (DanmakuMode)info_0[1].GetInt32();
            return message;
        }
    }
}

/*
 {
    "cmd": "DANMU_MSG",
    "dm_v2": "",
    "info": [
        [0, 1, 25, 16750592, 1772689533403, 1772689533, 0, "70cc3adf", 0, 0, 0, "", 0, "{}", "{}", {
            "extra": "{\"send_from_me\":false,\"master_player_hidden\":false,\"mode\":0,\"color\":16750592,\"dm_type\":0,\"font_size\":25,\"player_mode\":1,\"show_player_type\":0,\"content\":\"test\",\"user_hash\":\"1892432607\",\"emoticon_unique\":\"\",\"bulge_display\":0,\"recommend_score\":7,\"dm_score\":0,\"chronos_force_display\":0,\"main_state_dm_color\":\"\",\"objective_state_dm_color\":\"\",\"direction\":0,\"pk_direction\":0,\"quartet_direction\":0,\"anniversary_crowd\":0,\"yeah_space_type\":\"\",\"yeah_space_url\":\"\",\"jump_to_url\":\"\",\"space_type\":\"\",\"space_url\":\"\",\"animation\":{},\"emots\":null,\"is_audited\":false,\"id_str\":\"2dbaa40dab5fab6a3f3f496d7169a9182936\",\"icon\":null,\"show_reply\":true,\"reply_mid\":0,\"reply_uname\":\"\",\"reply_uname_color\":\"\",\"reply_is_mystery\":false,\"reply_type_enum\":0,\"hit_combo\":0,\"esports_jump_url\":\"\",\"is_mirror\":false,\"is_collaboration_member\":false,\"card\":{\"card_type\":0,\"oid_str\":\"\",\"oid_str_1\":\"\",\"origin_oid_str\":\"\",\"share_id\":\"\",\"share_origin\":\"\",\"from\":\"\",\"card_content\":null},\"voice\":null,\"background_type\":0}",
            "mode": 0,
            "show_player_type": 0,
            "user": {
                "base": {
                    "face": "https://i2.hdslb.com/bfs/face/98ce9529bda794050e077d4aa06cc6189e35ae93.png",
                    "is_mystery": false,
                    "name": "西井QAQ",
                    "name_color": 0,
                    "name_color_str": "",
                    "official_info": {
                        "desc": "",
                        "role": 0,
                        "title": "",
                        "type": -1
                    },
                    "origin_info": {
                        "face": "https://i2.hdslb.com/bfs/face/98ce9529bda794050e077d4aa06cc6189e35ae93.png",
                        "name": "西井QAQ"
                    },
                    "risk_ctrl_info": null
                },
                "guard": null,
                "guard_leader": {
                    "is_guard_leader": false
                },
                "medal": {
                    "color": 1725515,
                    "color_border": 12632256,
                    "color_end": 12632256,
                    "color_start": 12632256,
                    "guard_icon": "",
                    "guard_level": 0,
                    "honor_icon": "",
                    "id": 45154,
                    "is_light": 0,
                    "level": 22,
                    "name": "稽气人",
                    "ruid": 8455326,
                    "score": 3514,
                    "typ": 0,
                    "user_receive_count": 0,
                    "v2_medal_color_border": "#919298CC",
                    "v2_medal_color_end": "#919298CC",
                    "v2_medal_color_level": "#919298E6",
                    "v2_medal_color_start": "#919298CC",
                    "v2_medal_color_text": "#FFFFFF"
                },
                "title": {
                    "old_title_css_id": "ice-dust",
                    "title_css_id": "title-48-1"
                },
                "uhead_frame": null,
                "uid": 35744708,
                "wealth": null
            }
        }, {
            "activity_identity": "",
            "activity_source": 0,
            "not_show": 0
        }, 0], "test", [35744708, "西井QAQ", 1, 0, 0, 10000, 1, ""],
        [22, "稽气人", "机器工具人", 164725, 1725515, "", 0, 12632256, 12632256, 12632256, 0, 0, 8455326],
        [53, 0, 16752445, 3706, 0],
        ["ice-dust", "title-48-1"], 0, 0, null, {
            "ct": "58EB754B",
            "ts": 1772689533
        },
        0, 0, null, null, 0, 546, [21], null
    ]
}
{
	"cmd": "DANMU_MSG",
	"dm_v2": "",
	"info": [
		[0, 1, 25, 8322816, 1772691327418, 1772691300, 0, "33d8d4b4", 0, 0, 0, "", 0, "{}", "{}", {
			"extra": "{\"send_from_me\":false,\"master_player_hidden\":false,\"mode\":0,\"color\":8322816,\"dm_type\":0,\"font_size\":25,\"player_mode\":1,\"show_player_type\":0,\"content\":\"test\",\"user_hash\":\"869848244\",\"emoticon_unique\":\"\",\"bulge_display\":0,\"recommend_score\":8,\"dm_score\":0,\"chronos_force_display\":0,\"main_state_dm_color\":\"\",\"objective_state_dm_color\":\"\",\"direction\":0,\"pk_direction\":0,\"quartet_direction\":0,\"anniversary_crowd\":0,\"yeah_space_type\":\"\",\"yeah_space_url\":\"\",\"jump_to_url\":\"\",\"space_type\":\"\",\"space_url\":\"\",\"animation\":{},\"emots\":null,\"is_audited\":false,\"id_str\":\"48ac0bb0405cbe7d25f8ae329f69a91f7459\",\"icon\":null,\"show_reply\":true,\"reply_mid\":35744708,\"reply_uname\":\"西井QAQ\",\"reply_uname_color\":\"#FB7299\",\"reply_is_mystery\":false,\"reply_type_enum\":1,\"hit_combo\":0,\"esports_jump_url\":\"\",\"is_mirror\":false,\"is_collaboration_member\":false,\"card\":{\"card_type\":0,\"oid_str\":\"\",\"oid_str_1\":\"\",\"origin_oid_str\":\"\",\"share_id\":\"\",\"share_origin\":\"\",\"from\":\"\",\"card_content\":null},\"voice\":null,\"background_type\":0}",
			"mode": 0,
			"show_player_type": 0,
			"user": {
				"base": {
					"face": "https://i0.hdslb.com/bfs/face/0d22d13df433b9f9d2fbd01a679f53414ca6a787.png",
					"is_mystery": false,
					"name": "机器工具人",
					"name_color": 0,
					"name_color_str": "",
					"official_info": {
						"desc": "",
						"role": 0,
						"title": "",
						"type": -1
					},
					"origin_info": {
						"face": "https://i0.hdslb.com/bfs/face/0d22d13df433b9f9d2fbd01a679f53414ca6a787.png",
						"name": "机器工具人"
					},
					"risk_ctrl_info": null
				},
				"guard": null,
				"guard_leader": {
					"is_guard_leader": false
				},
				"medal": {
					"color": 1725515,
					"color_border": 12632256,
					"color_end": 12632256,
					"color_start": 12632256,
					"guard_icon": "",
					"guard_level": 0,
					"honor_icon": "",
					"id": 123,
					"is_light": 0,
					"level": 22,
					"name": "电音",
					"ruid": 11153765,
					"score": 3097,
					"typ": 0,
					"user_receive_count": 0,
					"v2_medal_color_border": "#919298CC",
					"v2_medal_color_end": "#919298CC",
					"v2_medal_color_level": "#919298E6",
					"v2_medal_color_start": "#919298CC",
					"v2_medal_color_text": "#FFFFFF"
				},
				"title": {
					"old_title_css_id": "title-58-1",
					"title_css_id": "title-58-1"
				},
				"uhead_frame": null,
				"uid": 8455326,
				"wealth": null
			}
		}, {
			"activity_identity": "",
			"activity_source": 0,
			"not_show": 0
		}, 0], "test", [8455326, "机器工具人", 0, 0, 0, 10000, 1, ""],
		[22, "电音", "3号直播间", 23058, 1725515, "", 0, 12632256, 12632256, 12632256, 0, 0, 11153765],
		[44, 0, 16746162, 25543, 0],
		["title-58-1", "title-58-1"], 0, 0, null, {
			"ct": "69A69216",
			"ts": 1772691327
		},
		0, 0, null, null, 0, 240, [3], null
	]
}
{
	"cmd": "DANMU_MSG",
	"dm_v2": "",
	"info": [
		[0, 1, 25, 8322816, 1772691341647, 1772691300, 0, "33d8d4b4", 0, 0, 0, "", 1, {
			"bulge_display": 0,
			"emoticon_unique": "official_104",
			"height": 60,
			"in_player_area": 1,
			"is_dynamic": 1,
			"url": "http://i0.hdslb.com/bfs/live/18af5576a4582535a3c828c3ae46a7855d9c6070.png",
			"width": 156
		}, "{}", {
			"extra": "{\"send_from_me\":false,\"master_player_hidden\":false,\"mode\":0,\"color\":8322816,\"dm_type\":1,\"font_size\":25,\"player_mode\":1,\"show_player_type\":0,\"content\":\"暗中观察\",\"user_hash\":\"869848244\",\"emoticon_unique\":\"official_104\",\"bulge_display\":0,\"recommend_score\":0,\"dm_score\":0,\"chronos_force_display\":0,\"main_state_dm_color\":\"\",\"objective_state_dm_color\":\"\",\"direction\":0,\"pk_direction\":0,\"quartet_direction\":0,\"anniversary_crowd\":0,\"yeah_space_type\":\"\",\"yeah_space_url\":\"\",\"jump_to_url\":\"\",\"space_type\":\"\",\"space_url\":\"\",\"animation\":{},\"emots\":null,\"is_audited\":false,\"id_str\":\"5ce4c246af140b1558b01a963d69a91f5921\",\"icon\":null,\"show_reply\":true,\"reply_mid\":0,\"reply_uname\":\"\",\"reply_uname_color\":\"\",\"reply_is_mystery\":false,\"reply_type_enum\":0,\"hit_combo\":0,\"esports_jump_url\":\"\",\"is_mirror\":false,\"is_collaboration_member\":false,\"card\":{\"card_type\":0,\"oid_str\":\"\",\"oid_str_1\":\"\",\"origin_oid_str\":\"\",\"share_id\":\"\",\"share_origin\":\"\",\"from\":\"\",\"card_content\":null},\"voice\":null,\"background_type\":0}",
			"mode": 0,
			"show_player_type": 0,
			"user": {
				"base": {
					"face": "https://i0.hdslb.com/bfs/face/0d22d13df433b9f9d2fbd01a679f53414ca6a787.png",
					"is_mystery": false,
					"name": "机器工具人",
					"name_color": 0,
					"name_color_str": "",
					"official_info": {
						"desc": "",
						"role": 0,
						"title": "",
						"type": -1
					},
					"origin_info": {
						"face": "https://i0.hdslb.com/bfs/face/0d22d13df433b9f9d2fbd01a679f53414ca6a787.png",
						"name": "机器工具人"
					},
					"risk_ctrl_info": null
				},
				"guard": null,
				"guard_leader": {
					"is_guard_leader": false
				},
				"medal": {
					"color": 1725515,
					"color_border": 12632256,
					"color_end": 12632256,
					"color_start": 12632256,
					"guard_icon": "",
					"guard_level": 0,
					"honor_icon": "",
					"id": 123,
					"is_light": 0,
					"level": 22,
					"name": "电音",
					"ruid": 11153765,
					"score": 3097,
					"typ": 0,
					"user_receive_count": 0,
					"v2_medal_color_border": "#919298CC",
					"v2_medal_color_end": "#919298CC",
					"v2_medal_color_level": "#919298E6",
					"v2_medal_color_start": "#919298CC",
					"v2_medal_color_text": "#FFFFFF"
				},
				"title": {
					"old_title_css_id": "title-58-1",
					"title_css_id": "title-58-1"
				},
				"uhead_frame": null,
				"uid": 8455326,
				"wealth": null
			}
		}, {
			"activity_identity": "",
			"activity_source": 0,
			"not_show": 0
		}, 0], "暗中观察", [8455326, "机器工具人", 0, 0, 0, 10000, 1, ""],
		[22, "电音", "3号直播间", 23058, 1725515, "", 0, 12632256, 12632256, 12632256, 0, 0, 11153765],
		[44, 0, 16746162, 25543, 0],
		["title-58-1", "title-58-1"], 0, 0, null, {
			"ct": "2AEBBC9A",
			"ts": 1772691341
		},
		0, 0, null, null, 0, 220, [3], null
	]
}
 */
