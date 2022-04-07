namespace Executorlibs.TarProtocol.Models
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
}
