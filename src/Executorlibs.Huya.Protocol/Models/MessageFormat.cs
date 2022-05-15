using Executorlibs.TarProtocol.IO;
using Executorlibs.TarProtocol.Models;
using Executorlibs.TarProtocol.Models.Primitives;

namespace Executorlibs.Huya.Protocol.Models
{
    public class MessageFormat : ITarType // ContentFormat
    {
        public static MessageFormat Default { get; } = new MessageFormat
        {
            FontColor = 255,
            FontSize = 4,
            // PopupStyle = 0,
            UserNameColor = 255,
            DarkFontColor = 255,
            DarkUserNameColor = 255
        };

        public int FontColor;

        public int FontSize;

        public int PopupStyle;

        public int UserNameColor;

        public int DarkFontColor;

        public int DarkUserNameColor;

        public void ReadFrom(ref TarReader reader)
        {
            FontColor = reader.Read<TarInt32>();
            FontSize = reader.Read<TarInt32>();
            PopupStyle = reader.Read<TarInt32>();
            UserNameColor = reader.Read<TarInt32>();
            DarkFontColor = reader.Read<TarInt32>();
            DarkUserNameColor = reader.Read<TarInt32>();
        }

        public void WriteTo(ref TarWriter writer)
        {
            ITarType fontColor = new TarInt32(0, FontColor),
                     fontSize = new TarInt32(1, FontSize),
                     popupStyle = new TarInt32(2, PopupStyle),
                     userNameColor = new TarInt32(3, UserNameColor),
                     darkFontColor = new TarInt32(4, DarkFontColor),
                     darkUserNameColor = new TarInt32(5, DarkUserNameColor);
            fontColor.WriteTo(ref writer);
            fontSize.WriteTo(ref writer);
            popupStyle.WriteTo(ref writer);
            userNameColor.WriteTo(ref writer);
            darkFontColor.WriteTo(ref writer);
            darkUserNameColor.WriteTo(ref writer);
        }
    }
}
