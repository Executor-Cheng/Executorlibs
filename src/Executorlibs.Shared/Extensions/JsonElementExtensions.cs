using System.Linq;
using System.Text.Json;

namespace Executorlibs.Shared.Extensions
{
    public static class JsonElementExtensions
    {
        public static bool HasValues(this JsonElement j)
            => j.ValueKind == JsonValueKind.Array ? j.EnumerateArray().Any() : j.ValueKind == JsonValueKind.Object && j.EnumerateObject().Any();
    }
}
