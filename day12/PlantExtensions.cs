using System.Collections.Generic;
using System.Linq;

namespace day12
{
    static class PlantExtensions
    {
        public static string ToPlantString(this IEnumerable<bool> @this)
            => string.Concat(
                @this.Select(o => o ? '#' : '.')
            );
    }
}