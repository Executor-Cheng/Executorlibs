using System.Runtime.CompilerServices;

namespace Executorlibs.TarProtocol.Protocol
{
    public struct TarHeader
    {
        public enum TarType : byte
        {
            Byte = 0,
            Short = 1,
            Int = 2,
            Long = 3,
            Float = 4,
            Double = 5,
            String1 = 6,
            String4 = 7,
            Map = 8,
            List = 9,
            StructBegin = 10,
            StructEnd = 11,
            Zero = 12,
            SimpleList = 13
        }

        /// <summary>
        /// | <see cref="Tag1"/>(4 bits) || <see cref="Type"/>(4 bits) || <see cref="Tag2"/>(1 <see cref="byte"/>) |
        /// </summary>
        /// <remarks>
        /// When <see cref="Tag1"/> &lt; 15, <see cref="Tag"/> == <see cref="Tag1"/>, otherwise, <see cref="Tag"/> == <see cref="Tag2"/>
        /// </remarks>
        public ushort Data;
        // 12 34 BE
        // || ^^
        // ||  \-- Tag2
        // |\-- Type
        // \-- Tag1

        // 34 12 LE
        // ^^ ^^
        // || |\-- Type
        // -| \-- Tag1
        //  \-- Tag2
        public TarType Type
        {
#if BIGENDIAN
            get => (TarType)(Data >> 8 & 0xF);
            set => Data = (ushort)((byte)value << 12 | Data & 0x0FFF);
#else
            get => (TarType)(Data & 0xF);
            set => Data = (ushort)(Data & 0xFFF0 | (byte)value);
#endif
        }

        public byte Tag
        {
            get
            {
                byte tag1 = Tag1;
                bool eq15 = tag1 == 15; // false = 0
                byte b = Unsafe.As<bool, byte>(ref eq15);
                return (byte)(tag1 & b - 1 | Tag2 & -b);
            }
            set
            {
                bool greater14 = value > 14;
                byte b = Unsafe.As<bool, byte>(ref greater14);
#if BIGENDIAN
                Data = (ushort)(Data & 0x0F00 | ((value & b - 1) << 12) | ((value | 0xF000) & -b));
#else
                Data = (ushort)(Data & 0x000F | value << 4 & b - 1 | (value << 8 | 0xF0) & -b);
#endif
            }
        }

        public int ActualSize
        {
            get
            {
                bool hasTag2 = Tag1 == 15;
                return 1 + Unsafe.As<bool, byte>(ref hasTag2);
            }
        }
#if BIGENDIAN
        public byte Tag1 => (byte)(Data >> 12);
#else
        public byte Tag1 => (byte)(Data >> 4 & 0xF);
#endif

#if BIGENDIAN
        public byte Tag2 => (byte)(Data & 0xFF);
#else
        public byte Tag2 => (byte)(Data >> 8);
#endif
        public TarHeader(TarType type, byte tag)
        {
            Data = 0;
            Type = type;
            Tag = tag;
        }
    }
}
