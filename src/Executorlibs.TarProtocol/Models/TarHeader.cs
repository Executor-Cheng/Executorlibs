using System.Runtime.CompilerServices;

namespace Executorlibs.TarProtocol.Models
{
    public struct TarHeader
    {
        internal uint _rawTag;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TarHeader(TarType type, byte tag)
        {
            Unsafe.SkipInit(out _rawTag);
            Type = type;
            Tag = tag;
        }

        public TarHeader(uint rawTag)
        {
            _rawTag = rawTag;
        }

        public TarHeader(ushort rawTag)
        {
            _rawTag = rawTag;
        }

        public TarType Type
        {
            get => (TarType)(_rawTag & 0xF);
            set => _rawTag = _rawTag & 0xFFF0 | (byte)value;
        }

        public byte Tag
        {
            get => HasTag8 ? Tag8 : Tag4;
            set => TagNoBranching = value;
        }

        public byte TagNoBranching
        {
            get
            {
                byte tag1 = Tag4;
                bool eq15 = tag1 == 15;
                byte mask = Unsafe.As<bool, byte>(ref eq15);
                return (byte)(tag1 & (mask - 1) | Tag8 & -mask);
            }
            set
            {
                //bool eq15 = value >= 15;
                //uint mask = (uint)-Unsafe.As<bool, byte>(ref eq15);
                /*
                 * cmp eax, 0xf
                 * setge dl
                 * movzx edx, dl
                 * neg edx
                 * Code size: 0xb
                 */
                uint mask = (uint)((14 - value) >> 4);
                /*
                 * mov edx, eax
                 * neg edx
                 * add edx, 0xe
                 * sar edx, 4
                 * Code size: 0xa
                 */
                uint tag = ((uint)(value << 8) | 0xF0) & mask;
                tag |= (uint)((value & 0xF) << 4);
                _rawTag &= 0x000F;
                _rawTag |= tag;
            }
        }

        public readonly byte Tag4 => (byte)(_rawTag >> 4 & 0xF);

        public readonly byte Tag8 => (byte)(_rawTag >> 8);

        public readonly bool HasTag8 => (_rawTag & 0xF0) == 0xF0;

        public readonly byte Tag8Size
        {
            get
            {
                bool hasTag2 = HasTag8;
                return Unsafe.As<bool, byte>(ref hasTag2);
            }
        }

        public readonly int ActualSize => 1 + Tag8Size;

        public override int GetHashCode()
        {
            return _rawTag.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Type} = {Tag}";
        }

        public override bool Equals(object? obj)
        {
            return obj is TarHeader t && t.Equals(this);
        }

        public bool Equals(TarHeader t)
        {
            return t._rawTag == this._rawTag;
        }

        public static bool operator ==(TarHeader left, TarHeader right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TarHeader left, TarHeader right)
        {
            return !(left == right);
        }
    }
}
