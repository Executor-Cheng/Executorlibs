using System;
using Executorlibs.TarProtocol.IO;
using Executorlibs.TarProtocol.Models;
using Executorlibs.TarProtocol.Models.Primitives;

namespace Executorlibs.Huya.Protocol.Models
{
    public class MessageBulletFormat : ITarType // BulletFormat
    {
        public class BulletBorderGroundFormat : ITarType
        {
            public static BulletBorderGroundFormat Default => new BulletBorderGroundFormat
            {
                //EnableUse = 0,
                //BorderThickness = 0,
                BorderColor = 255,
                BorderDiaphaneity = 100,
                GroundColor = 255,
                GroundColourDiaphaneity = 100,
                AvatarDecorationUrl = string.Empty,
                FontColor = 255
            };

            public byte EnableUse;

            public int BorderThickness;

            public int BorderColor;

            public int BorderDiaphaneity;

            public int GroundColor;

            public int GroundColourDiaphaneity;

            public string AvatarDecorationUrl = null!;

            public int FontColor;

            public void ReadFrom(ref TarReader reader)
            {
                EnableUse = reader.Read<TarByte>();
                BorderThickness = reader.Read<TarInt32>();
                BorderColor = reader.Read<TarInt32>();
                BorderDiaphaneity = reader.Read<TarInt32>();
                GroundColor = reader.Read<TarInt32>();
                GroundColourDiaphaneity = reader.Read<TarInt32>();
                AvatarDecorationUrl = reader.Read<TarString>();
                FontColor = reader.Read<TarInt32>();
            }

            public void WriteTo(ref TarWriter writer)
            {
                ITarType enableUse = new TarByte(0, EnableUse),
                         borderThickness = new TarInt32(1, BorderThickness),
                         borderColor = new TarInt32(2, BorderColor),
                         borderDiaphaneity = new TarInt32(3, BorderDiaphaneity),
                         groundColor = new TarInt32(4, GroundColor),
                         groundColourDiaphaneity = new TarInt32(5, GroundColourDiaphaneity),
                         avatarDecorationUrl = new TarString(6, AvatarDecorationUrl),
                         fontColor = new TarInt32(7, FontColor);
                enableUse.WriteTo(ref writer);
                borderThickness.WriteTo(ref writer);
                borderColor.WriteTo(ref writer);
                borderDiaphaneity.WriteTo(ref writer);
                groundColor.WriteTo(ref writer);
                groundColourDiaphaneity.WriteTo(ref writer);
                avatarDecorationUrl.WriteTo(ref writer);
                fontColor.WriteTo(ref writer);
            }
        } // BulletBorderGroundFormat

        public static MessageBulletFormat Default { get; } = new MessageBulletFormat
        {
            FontColor = 255,
            FontSize = 4,
            //TextSpeed = 0,
            TransitionType = 1,
            //PopupStyle = 0,
            BorderGroundFormat = BulletBorderGroundFormat.Default,
            GuaduatedColor = new TarList<TarInt32>(6, Array.Empty<TarInt32>()),
            //AvartarFlag = 0
        };

        public int FontColor;

        public int FontSize;

        public int TextSpeed;

        public int TransitionType;

        public int PopupStyle;

        public BulletBorderGroundFormat BorderGroundFormat = null!;

        public TarList<TarInt32> GuaduatedColor;

        public int AvartarFlag;

        public int AvatarTerminalFlag;

        public void ReadFrom(ref TarReader reader)
        {
            FontColor = reader.Read<TarInt32>();
            FontSize = reader.Read<TarInt32>();
            TextSpeed = reader.Read<TarInt32>();
            TransitionType = reader.Read<TarInt32>();
            PopupStyle = reader.Read<TarInt32>();
            BorderGroundFormat = reader.Read<TarStruct<BulletBorderGroundFormat>>();
            GuaduatedColor = reader.Read<TarList<TarInt32>>();
            AvartarFlag = reader.Read<TarInt32>();
            AvatarTerminalFlag = reader.Read<TarInt32>();
        }

        public void WriteTo(ref TarWriter stream)
        {
            ITarType fontColor = new TarInt32(0, FontColor),
                     fontSize = new TarInt32(1, FontSize),
                     textSpeed = new TarInt32(2, TextSpeed),
                     transitionType = new TarInt32(3, TransitionType),
                     popupStyle = new TarInt32(4, PopupStyle),
                     borderGroundFormat = new TarStruct<BulletBorderGroundFormat>(5, BorderGroundFormat),
                     avartarFlag = new TarInt32(7, AvartarFlag),
                     avatarTerminalFlag = new TarInt32(8, AvatarTerminalFlag);
            fontColor.WriteTo(ref stream);
            fontSize.WriteTo(ref stream);
            textSpeed.WriteTo(ref stream);
            transitionType.WriteTo(ref stream);
            popupStyle.WriteTo(ref stream);
            borderGroundFormat.WriteTo(ref stream);
            GuaduatedColor.Header.Tag = 6;
            GuaduatedColor.WriteTo(ref stream);
            avartarFlag.WriteTo(ref stream);
            avatarTerminalFlag.WriteTo(ref stream);
        }
    }
}
