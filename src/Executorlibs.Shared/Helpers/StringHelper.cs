#if !NETSTANDARD2_0
namespace Executorlibs.Shared.Helpers
{
    public static unsafe class StringHelper
    {
        public static string FastAllocateString(int size)
            => string.Create(size, 0, static (_, _) => { });
    }
}
#endif
